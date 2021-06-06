using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CricBookingsWebsite.Models;
using CricBookingsWebsite.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace CricBookingsWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationContext _context;
        private readonly UserManager<User> userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationContext context, UserManager<User> userManager)
        {
            _logger = logger;
            _context = context;
            this.userManager = userManager;
        }

        public async Task<User> GetCurrentUser()
        {
            var currentUser = await userManager.GetUserAsync(HttpContext.User);
            return currentUser;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> States()
        {
            var states = await _context.State.ToListAsync();
            return View(states);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public async Task<IActionResult> TestUsers()
        {
            var testUsers = await _context.TestUsers.ToListAsync();
            return View(testUsers);
        }

        [Authorize(Roles = "Administrator, User")]
        [HttpGet]
        public async Task<IActionResult> BookTimeSlot()
        {
            List<State> states = new List<State>();

            states = await _context.State.ToListAsync();

            List<string> stateNames = new List<string>();

            foreach (var state in states)
            {
                stateNames.Add(state.Name);
            }

            SelectList statesList = new SelectList(stateNames, "StateName");


            ViewBag.ListOfStates = statesList;

            return View();
        }

        public async Task<string> GetCity(string StateName)
        {
            List<City> cityList = new List<City>();

            cityList = await _context.City
                .Where(city => city.State.Name == StateName)
                .Include(city => city.State)
                .ToListAsync();

            //List<string> cityNames = new List<string>();

            //foreach (var city in cityList)
            //{
            //    cityNames.Add(city.Name);
            //}

            return JsonConvert.SerializeObject(cityList, Formatting.None,
                new JsonSerializerSettings()
                { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }

        public async Task<string> GetCenter(int cityId)
        {
            List<Center> centerList = new List<Center>();

            centerList = await _context.Center
                .Where(center => center.City.Id == cityId)
                .Include(center => center.City)
                .ToListAsync();

            //List<string> centerNames = new List<string>();

            //foreach (var center in centerList)
            //{
            //    centerNames.Add(center.Name);
            //}

            return JsonConvert.SerializeObject(centerList, Formatting.None,
                new JsonSerializerSettings()
                { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }

        public async Task<string> GetTimeSlot(int centerId, string chosenDate)
        {
            //var state = await _context.State
            //    .Where(center => center.Name == centerName)
            //    .Include(center => center.City)
            //    .ThenInclude(city => city.State)
            //    .FirstOrDefaultAsync();



            var center = await _context.Center
                .Where(center => center.Id == centerId)
                .FirstOrDefaultAsync();

            

            BookTimeSlotViewModel model = new BookTimeSlotViewModel
            {
                //stateId = state.Id,
                //stateName = state.Name,
                //cityId = state.City.FirstOrDefault().Id,
                //cityName = state.City.FirstOrDefault().Name,
                centerId = center.Id,
                //centerName = centerName,
                chosenDate = chosenDate
            };

            DateTime date = DateTime.Parse(model.chosenDate);

            List<TimeSlot> timeSlots = await _context.TimeSlot
                .Where(timeslot => (timeslot.CenterId == model.centerId) || (timeslot.Center.Name == model.centerName))
                .Where(timeSlot => timeSlot.SlotDateTime.Day == date.Day)
                .Where(timeSlot => timeSlot.Booking.Count == 0)
                .Include(timeslot => timeslot.Lane)
                .Include(timeslot => timeslot.Center)
                .ToListAsync();

            if (timeSlots.Count == 0)
            {
                // TODO - change from hard-coded 60 mins to variable duration
                await InsertTimeSlots(model.centerId, model.centerName, 60, model.chosenDate);
                timeSlots = await _context.TimeSlot
                .Where(timeslot => (timeslot.CenterId == model.centerId) || (timeslot.Center.Name == model.centerName))
                .Where(timeSlot => timeSlot.SlotDateTime.Day == date.Day)
                .ToListAsync();
            }

            //string str = Json(timeSlots).ToString();
            //Console.WriteLine("************\n" + str + "\n***************");
            string json = JsonConvert.SerializeObject(timeSlots, Formatting.None,
                new JsonSerializerSettings()
                { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return json;
        }

        public async Task InsertTimeSlots(int centerId, string centerName, int durationInMinutes, string stringDate)
        {
            // this is temporary - will work this around to make sure duration is actually variable instead of just assigning it
            //. durationInMinutes = 60;

            DateTime date = DateTime.Parse(stringDate);

            List<Lane> lanesInCenter = await _context.Lane
                .Where(lane => lane.CenterId == centerId || lane.Center.Name == centerName)
                .ToListAsync();

            DateTime loopStartTime = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
            DateTime loopEndTime = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
            

            foreach (var lane in lanesInCenter)
            {
                while (DateTime.Compare(loopStartTime, loopEndTime) < 0)
                {
                    TimeSlot timeSlot = new TimeSlot
                    {
                        CenterId = lane.CenterId,
                        LaneId = lane.Id,
                        SlotDateTime = loopStartTime
                    };

                    await _context.AddAsync(timeSlot);
                    await _context.SaveChangesAsync();
                    loopStartTime = loopStartTime.AddMinutes(60);
                }
            }

            
        }

        public async Task<bool> IsTimeSlotsInADay(Center center, DateTime day)
        {
            var timeSlots = await _context.TimeSlot
                .Where(timeSlot => (timeSlot.CenterId == center.Id) || (timeSlot.Center.Name == center.Name))
                .Where(timeSlot => timeSlot.SlotDateTime == day)
                .ToListAsync();

            if (timeSlots.Count == 0)
            {
                return false;
            }

            else
            {
                return true;
            }
        }

        [AcceptVerbs("Post")]
        public async Task<IActionResult> CreateAndInsertBooking(int timeSlotId)
        {
            var user = await GetCurrentUser();
            Booking booking = new Booking();
            booking.TimeSlotId = timeSlotId;
            booking.UserId = user.Id;

            try
            {
                await _context.AddAsync(booking);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            return View("ListBookings");
        }

        [Authorize(Roles = "Administrator, User")]
        [HttpGet]
        public async Task<IActionResult> ListBookings()
        {
            var user = await GetCurrentUser();
            var bookings = await _context.Booking
            .Include(booking => booking.TimeSlot)
            .Where(booking => booking.UserId == user.Id)
            .ToListAsync();

            return View(bookings);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Globalization;
using CricBookingsWebsite.Models;
using CricBookingsWebsite.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;

namespace CricBookingsWebsite.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdministratorController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<User> userManager;
        private readonly ApplicationContext _context;

        public AdministratorController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, ApplicationContext context)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            _context = context;
        }

        

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View("NotFound");
            }

            else
            {
                var result = await roleManager.DeleteAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View("ListRoles");
            }

        }

        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }
            else
            {
                var result = await userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View("ListUsers");
            }
        }
        
        [HttpGet]
        public IActionResult ListUsers()
        {
            var users = userManager.Users;
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }

            var userClaims = await userManager.GetClaimsAsync(user);
            var userRoles = await userManager.GetRolesAsync(user);

            var model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Claims = userClaims.Select(c => c.Value).ToList(),
                Roles = userRoles
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.Id} cannot be found";
                return View("NotFound");
            }

            else
            {
                user.Email = model.Email;
                user.UserName = model.UserName;

                var result = await userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }
        }

        // [Authorize]
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }


        
        // [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };

                IdentityResult result = await roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Administrator");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }


            return View(model);
        }

        
        // [Authorize]
        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = roleManager.Roles;

            return View(roles);
        }

        
        // [Authorize]
        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            
            var role = await roleManager.FindByIdAsync(id);


            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View("NotFound");
            }


            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };

            foreach (var user in await userManager.Users.ToListAsync())
            {
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }

            return View(model);
        }

       
        // [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {

            var role = await roleManager.FindByIdAsync(model.Id);


            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {model.Id} cannot be found";
                return View("NotFound");
            }

            else
            {
                role.Name = model.RoleName;
                var result = await roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }
        }

        // [Authorize]
        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.roleId = roleId;
            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }

            var users = await userManager.Users.ToListAsync();
            var model = new List<UserRoleViewModel>();

            foreach (var user in users)
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    Username = user.UserName
                };

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }

                model.Add(userRoleViewModel);
            }

            return View(model);

        }

        
        // [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with ID = {roleId} cannot be found";
                return View("NotFound");
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await userManager.FindByIdAsync(model[i].UserId);

                IdentityResult result = null;

                if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }

                else if (!model[i].IsSelected && await userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                    {
                        continue;
                    }
                    else
                    {
                        return RedirectToAction("EditRole", new { Id = roleId });
                    }
                }
            }

            return RedirectToAction("EditRole", new { Id = roleId });
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        // TODO - In the future, try to search by Id rather than Name, as it will be faster (although doing Name was, by itself, a huge pain, doing it through id should not be very tough) 
        // Change CreateCenter() and GetCity() to reflect this

        [HttpGet]
        public async Task<IActionResult> CreateCenter()
        {
            List<State> states = new List<State>();

            states = await _context.State.ToListAsync();

            // states.Insert(0, new State { Id = 0, Name = "Select a State" });

            List<string> stateNames = new List<string>();

            foreach (var state in states)
            {
                stateNames.Add(state.Name);
            }

            SelectList statesList = new SelectList(stateNames, "StateName");


            ViewBag.ListOfStates = statesList;

            // List<City> cities = await _context.City.ToListAsync();

            return View();
        }


        public async Task<JsonResult> GetCity(string StateName)
        {
            List<City> cityList = new List<City>();

            cityList = await _context.City.Where(city => city.State.Name == StateName).ToListAsync();

            List<string> cityNames = new List<string>();

            foreach (var city in cityList)
            {
                cityNames.Add(city.Name);
            }

            return Json(cityNames);
        }

        /* [HttpGet]
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
        } */

        [HttpPost]
        public async Task<IActionResult> CreateCenter(CreateCenterViewModel model)
        {
            try
            {
                Center center = new Center();
                //City city = new City();
                //center.City = city; 
                //center.City.State.Name = model.StateName;
                // center.City.State.Id = model.StateId;
                //center.City.Name = model.CityName;
                
                // center.CityId = model.CityId;
                center.Name = model.Name;
                center.Addr1 = model.Address1;
                center.Addr2 = model.Address2;
                center.Zip = model.Zip;

                var cityName = model.CityName;

                var city = await _context.City
                    .Where(city => city.Name == cityName).FirstOrDefaultAsync();

                var cityId = city.Id;
                // var cityList = await _context.City.FromSqlRaw($"SELECT * FROM city WHERE city.name = {cityName}").ToListAsync();
                //foreach (var city in cityList)
                //{
                //    //center.City.Id = city.Id;
                //    center.CityId = city.Id;
                //    center.City = city;
                //    //center.City.StateId = city.StateId;
                //    //center.City.Name = model.CityName;

                //}

                //center.City.Id = id;

                center.City = city;
                center.CityId = cityId;

                var duplicateCenters = await _context.Center
                    .Where(center => center.CityId == cityId)
                    .Where(center => center.Name == model.Name)
                    .ToListAsync();

                if (duplicateCenters.Count != 0)
                {
                    ModelState.AddModelError("", "Center With That Name Already Exists In This City (Work In Progress)");
                    return View();
                }

                else
                {
                    await _context.AddAsync(center);

                    await _context.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
       
            return RedirectToAction("ListCenters");
        }

        [HttpGet]
        public async Task<IActionResult> ListCenters()
        {
            //var centers = await _context.Center.ToListAsync();

            // .Include() allows us to reference the cities and states from the other tables (foreign key is used)

            var centers = await _context.Center
                .Include(center => center.City)
                .Include(center => center.City.State)
                .ToListAsync();


            //foreach (var center in centers)
            //{
            //    center.City = "";
            //}
            
            return View(centers);
        }

        [HttpGet]
        public async Task<IActionResult> EditCenter(int Id)
        {
            int centerId = Id;
            var center = await _context.Center
                .Where(center => center.Id == centerId).FirstOrDefaultAsync();

            if (center == null)
            {
                ViewBag.ErrorMessage = $"Center with Id = {centerId} cannot be found";
                return View("NotFound");
            }

            var model = new EditCenterViewModel
            {
                centerId = center.Id,
                cityId = center.CityId,
                centerName = center.Name,
                addr1 = center.Addr1,
                addr2 = center.Addr2,
                zip = center.Zip
            };

            return View(model);
        }
        

        [HttpPost]
        public async Task<IActionResult> EditCenter(EditCenterViewModel model)
        {
            //int centerId = model.centerId;
            var center = await _context.Center
                .Where(center => center.Id == model.centerId).FirstOrDefaultAsync();

            var duplicateCenters = await _context.Center
                .Where(center => center.Id != model.centerId)
                .Where(center => center.CityId == model.cityId)
                .Where(center => center.Name == model.centerName)
                .ToListAsync();

            if (center == null)
            {
                ViewBag.ErrorMessage = $"Center with Id = {model.centerId} cannot be found";
                return View("NotFound");
            }

            else if (duplicateCenters.Count != 0)
            {
                ModelState.AddModelError("", "Center With That Name Already Exists In This City");
                return View();
            }

            else
            {
                center.Name = model.centerName;
                center.Addr1 = model.addr1;
                center.Addr2 = model.addr2;
                center.Zip = model.zip;

                try
                {
                    await _context.SaveChangesAsync();

                }
                catch (Exception ex)
                {
                   Console.WriteLine(ex.Message);
                   ModelState.AddModelError("", "Unable to save changes.");
                   throw;
                }

                return RedirectToAction("Index", "Home");
          
                
            }
        }

        [HttpGet]
        public IActionResult CreateLanes()
        {
            return View();
        }

        [HttpPost]
        [Route("Administrator/CreateLanes/{id}")]
        public async Task<IActionResult> CreateLanes(CreateLanesViewModel model, int id)
        {
            model.centerId = id;
            Lane lane = new Lane();
            lane.CenterId = model.centerId;
            lane.Name = model.laneName;
            lane.Inactive = model.isInactive;

            var duplicateLanes = await _context.Lane
                .Where(lane => lane.CenterId == model.centerId)
                .Where(lane => lane.Name == model.laneName)
                .ToListAsync();

            if (duplicateLanes.Count != 0)
            {
                ModelState.AddModelError("", "Lane With That Name Already Exists In This Center");
                return View();
            }

            else
            {
                try
                {
                    await _context.AddAsync(lane);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }

            return RedirectToAction("ListCenters");
            // Console.WriteLine("********************Lane created and properties set");
            // Console.WriteLine("centerId = " + lane.CenterId + ", name = " + lane.Name + ", inactive = " + lane.Inactive);
        }

        [HttpGet]
        public async Task<IActionResult> ListLanes(int Id)
        {
            int centerId = Id;
            var lanes = await _context.Lane
                .Where(lane => lane.CenterId == centerId)
                .Include(lane => lane.Center)
                .ToListAsync();
            return View(lanes);
        }

        [HttpGet]
        public async Task<IActionResult> EditLane(int Id)
        {
            int laneId = Id;
            var lane = await _context.Lane
               .Where(lane => lane.Id == laneId).FirstOrDefaultAsync();

            if (lane == null)
            {
                ViewBag.ErrorMessage = $"Lane with Id = {laneId} cannot be found";
                return View("NotFound");
            }

            var model = new EditLaneViewModel
            {
                laneId = lane.Id,
                centerId = lane.CenterId,
                laneName = lane.Name,
                isInactive = lane.Inactive
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditLane(EditLaneViewModel model)
        {
            var lane = await _context.Lane
                .Where(lane => lane.Id == model.laneId).FirstOrDefaultAsync();

            var duplicateLanes = await _context.Lane
                .Where(lane => lane.CenterId == model.centerId)
                .Where(lane => lane.Name == model.laneName)
                .ToListAsync();

            if (lane == null)
            {
                ViewBag.ErrorMessage = $"Lane with Id = {model.laneId} cannot be found";
                return View("NotFound");
            }

            else if (duplicateLanes.Count != 0)
            {
                ModelState.AddModelError("", "Lane With That Name Already Exists In This Center");
                return View();
            }

            else
            {
                lane.Name = model.laneName;
                lane.CenterId = model.centerId;
                lane.Inactive = model.isInactive;

                try
                {
                    await _context.SaveChangesAsync();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    ModelState.AddModelError("", "Unable to save changes.");
                    throw;
                }

                return RedirectToAction("ListCenters");
            }
        }

        [HttpGet]
        public IActionResult CreateSchedule(int Id)
        {
            int centerId = Id;
            var model = new CreateScheduleViewModel
            {
                centerId = centerId
            };

            return View(model);
        }

        [HttpPost]
        [Route("Administrator/CreateSchedule/{id}")]
        public async Task<IActionResult> CreateSchedule(int id, CreateScheduleViewModel model)
        {
            int centerId = id;
            model.centerId = centerId;
            // the following are the values actually stored in the DB, as the values in the table are startTime and endTime (will be converted back to hours and minutes when getting editing)
            int startTimeMinutes = (60 * model.startTimeHours) + model.startTimeMinutes;
            int endTimeMinutes = (60 * model.endTimeHours) + model.endTimeMinutes;

            var sameDaySchedules = await _context.Schedule
                .Where(schedule => schedule.CenterId == model.centerId)
                .Where(schedule => schedule.DayOfWeek == model.dayOfWeek)
                .ToListAsync();

            // TODO - Make sure to check if times overlap

            //foreach (var schedule in sameDaySchedules)
            //{
            //    if (startTimeMinutes < schedule.EndTime)
            //    {
            //        ModelState.AddModelError("", "Starting time of this schedule overlaps with another one");
            //        return View();
            //    }

            //    else if (endTimeMinutes > sch)
            //}

            if (startTimeMinutes > endTimeMinutes)
            {
                ModelState.AddModelError("", "Starting time is after ending time");
                return View();
            }


            else
            {
                Schedule schedule = new Schedule
                {
                    CenterId = centerId,
                    StartTime = startTimeMinutes,
                    EndTime = endTimeMinutes,
                    DayOfWeek = model.dayOfWeek
                };

                try
                {
                    await _context.AddAsync(schedule);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }

                return RedirectToAction("ListCenters");
            }
            

        }

        [HttpGet]
        public async Task<IActionResult> ListSchedules(int Id)
        {
            var schedules = await _context.Schedule
                .Where(schedule => schedule.CenterId == Id)
                .ToListAsync();

            return View(schedules);
        }
    }
}

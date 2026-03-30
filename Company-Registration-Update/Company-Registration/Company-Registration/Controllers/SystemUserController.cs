using Company_Registration.APIServices;
using Company_Registration.Models.DTO;
using Company_Registration.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Company_Registration.Controllers
{
    public class SystemUserController : Controller
    {
        private readonly ISystemUserService _systemUserService;

        public SystemUserController()
        {
            _systemUserService = new SystemUserService();
        }

        // GET: SystemUser/CreateUpdate/5
        public ActionResult Profile(int id = 0)
        {
            // id = 0 means new user
            var model = new CreateUpdateUserDto();
            return View(model);
        }

        // POST: SystemUser/CreateUpdate/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateUpdate(int id, CreateUpdateUserDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                // Call service
                var response = await _systemUserService.CreateUpdateSystemUser(id, model);

                if (!response.IsSuccess)
                {
                    ModelState.AddModelError("", string.IsNullOrEmpty(response.Message)
                        ? "Failed to create/update user"
                        : response.Message);
                    return View(model);
                }

                // Optional: Deserialize if needed
                // var user = JsonConvert.DeserializeObject<CreateUpdateUserDto>(response.Data.ToString());

                TempData["SuccessMessage"] = model.IsUpdate
                    ? "User updated successfully."
                    : "User created successfully.";

                // Redirect to user list or details page
                return RedirectToAction("Profile", "SystemUser");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error: " + ex.Message);
                return View(model);
            }
        }
        // GET: SystemUser
        public async Task<ActionResult> SystemUser()
        {
            try
            {
                var response = await _systemUserService.GetAllUsers();
                //var users = response.IsSuccess
                //            ? JsonConvert.DeserializeObject<List<CreateUpdateUserDto>>(response.Data.ToString())
                //            : new List<CreateUpdateUserDto>();
                return View(response);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to load users: " + ex.Message;
                return View(new List<CreateUpdateUserDto>());
            }
        }

        //// GET: SystemUser
        //public ActionResult Profile()
        //{
        //    return View();
        //}

    }
}
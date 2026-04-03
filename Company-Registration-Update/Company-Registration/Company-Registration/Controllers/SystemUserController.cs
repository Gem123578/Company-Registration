using Company_Registration.APIServices;
using Company_Registration.Models.DTO;
using Company_Registration.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
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

        // GET: SystemUser/CreateUpdatePartial/0
        public async Task<ActionResult> CreateUpdatePartial(int id = 0)
        {
            var model = new CreateUpdateUserDto();

            if (id > 0)
            {
                // Get user by ID from API
                var response = await _systemUserService.GetUserById(id);
                if (response.IsSuccess && response.Data != null)
                {
                    model = JsonConvert.DeserializeObject<CreateUpdateUserDto>(response.Data.ToString());
                    model.IsUpdate = true;
                }
            }

            return PartialView("_CreateUpdateUser", model); // modal partial
        }

        // POST: SystemUser/CreateUpdate/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateUpdate(int id, CreateUpdateUserDto model)
        {
            if (!ModelState.IsValid)
                return PartialView("_CreateUpdateUser", model);

            try
            {
                // Call service
                var response = await _systemUserService.CreateUpdateSystemUser(id, model);

                if (!response.IsSuccess)
                {
                    ModelState.AddModelError("", string.IsNullOrEmpty(response.Message)
                        ? "Failed to create/update user"
                        : response.Message);
                    return PartialView("_CreateUpdateUser", model);
                }

                // Optional: Deserialize if needed
                // var user = JsonConvert.DeserializeObject<CreateUpdateUserDto>(response.Data.ToString());

                TempData["SuccessMessage"] = model.IsUpdate
                    ? "User updated successfully."
                    : "User created successfully.";

                // Redirect to user list or details page
                return Json(response.IsSuccess = true); // indicate success for AJAX
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error: " + ex.Message);
                return PartialView("_CreateUpdateUser", model);
            }
        }
        // DELETE: SystemUser/Delete/5
        [HttpPost]
        public async Task<ActionResult> DeleteUser(long id)
        {
            try
            {
                // Get the current logged-in user's ID from session or claims
                var loginUserId = Convert.ToInt64(Session["UserId"]); // adjust if you store it differently

                // Call service to delete
                var response = await _systemUserService.DeleteUser(id);

                if (!response.IsSuccess)
                {
                    TempData["ErrorMessage"] = string.IsNullOrEmpty(response.Message)
                        ? "Failed to delete user."
                        : response.Message;
                    return Json(new { success = false, message = TempData["ErrorMessage"] });
                }

                TempData["SuccessMessage"] = "User deleted successfully.";
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        // GET: SystemUser
        public async Task<ActionResult> SystemUserGrid()
        {
            try
            {
                var response = await _systemUserService.GetAllUsers();
                var users = new List<CreateUpdateUserDto>();

                if (response.IsSuccess && response.Data != null)
                {
                    users = JsonConvert.DeserializeObject<List<CreateUpdateUserDto>>(response.Data.ToString());
                }

                return PartialView("_SystemUserGrid", users);
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
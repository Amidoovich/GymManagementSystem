using GymManagementBLL.Services.Classes;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementPL.Controllers
{
    [Authorize]
    public class SessionController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        public ActionResult Index()
        {

            var Sessions = _sessionService.GetAllSessions();

            return View(Sessions);
        }

        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "id must be valid";
                return RedirectToAction(nameof(Index));
            }


            var Session = _sessionService.GetSessionById(id);

            if (Session is null)
            {
                TempData["ErrorMessage"] = "Session not found";
                return RedirectToAction(nameof(Index));
            }

            return View(Session);

        }


        public ActionResult Create()
        {

            LoadTrainersDropDowns();
            LoadCategoriesDropDowns();


            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateSessionViewModel createSession)
        {
            if (!ModelState.IsValid)
            {

                LoadTrainersDropDowns();
                LoadCategoriesDropDowns();
                return View(createSession);
            }

            var Result = _sessionService.CreateSession(createSession);

            if (Result)
            {
                TempData["SuccessMessage"] = "Session Created Successfully";

                return RedirectToAction(nameof(Index));

            }
            else
            {
                TempData["ErrorMessage"] = "Session Created Failed";
                LoadTrainersDropDowns();
                LoadCategoriesDropDowns();
                return View(createSession);
            }

        }


        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "id must be valid";
                return RedirectToAction(nameof(Index));
            }

            var Session = _sessionService.GetSessionToUpdate(id);
            

            if (Session is null)
            {
                TempData["ErrorMessage"] = "Session not found";
                return RedirectToAction(nameof(Index));
            }

            LoadTrainersDropDowns();

            return View(Session);

        }

        [HttpPost]

        public ActionResult Edit([FromRoute] int id , SessionToUpdateViewModel sessionToUpdate)
        {
            if (!ModelState.IsValid)
            {

                LoadTrainersDropDowns();
                return View(sessionToUpdate);
            }

            var Result = _sessionService.UpdateSession(id, sessionToUpdate);

            if (Result)
            {
                TempData["SuccessMessage"] = "Session Updated Successfully";

                return RedirectToAction(nameof(Index));

            }
            else
            {
                TempData["ErrorMessage"] = "Session Updated Failed";
                LoadTrainersDropDowns();
                return View(sessionToUpdate);
            }

        }

        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "id must be valid";
                return RedirectToAction(nameof(Index));
            }


            var Session = _sessionService.GetSessionById(id);

            if (Session is null)
            {
                TempData["ErrorMessage"] = "Session not found";
                return RedirectToAction(nameof(Index));
            }



            ViewBag.SessionId = id;
            return View();

        }

        [HttpPost]
        public ActionResult DeleteConfirmed(int id)
        {
            var Result = _sessionService.RemoveSession(id);
            
            if (Result)
                TempData["SuccessMessage"] = "Session Deleted Successfully";

            else
                TempData["ErrorMessage"] = "Session Deleted Failed";

            return RedirectToAction(nameof(Index));

        }


        #region Helper 


        private void LoadTrainersDropDowns()
        {

            var Trainers = _sessionService.GetAllTrainersForDropDown();
            ViewBag.Trainers = new SelectList(Trainers, "Id", "Name");
        }
        private void LoadCategoriesDropDowns()
        {

            var Categories = _sessionService.GetAllCategoriesForDropDown();
            ViewBag.Categories = new SelectList(Categories, "Id", "Name");
        }

        #endregion
    }
}

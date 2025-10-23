using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class TrainerController : Controller
    {
        private readonly ITrainerService _trainerService;

        public TrainerController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }

        public ActionResult Index()
        {

            var trainers = _trainerService.GetAllTrainers();


            return View(trainers);
        }

        public ActionResult TrainerDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Of Trainer can Not be 0 or Negative Number";
                return RedirectToAction(nameof(Index));
            }

            var trainer = _trainerService.GetTrainerDetails(id);

            if (trainer == null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }


            return View(trainer);


        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateTrainer(CreateTrainerViewModel createTrainer)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvalid", "Check Data And Mising Fields");
                return View(nameof(Create), createTrainer);
            }

            bool Result = _trainerService.CreateTrainer(createTrainer);

            if (Result)
            {
                TempData["SuccessMessage"] = "Trainer Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "Trainer Created Failed , check Phone and Email";
                return RedirectToAction(nameof(Index));
            }

        }



        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Of Trainer can Not be 0 or Negative Number";
                return RedirectToAction(nameof(Index));
            }

            var TrainerToUpdate = _trainerService.GetTrainerToUpdate(id);

            if (TrainerToUpdate is null)
            {
                TempData["ErrorMessage"] = "Trainer not found";
                return RedirectToAction(nameof(Index));
            }

            return View(TrainerToUpdate);

        }

        [HttpPost]
        public ActionResult Edit([FromRoute] int id, TrianerToUpdateViewModel trianer)
        {
            if (!ModelState.IsValid)
                return View(trianer);

            bool Result = _trainerService.UpdateTrainer(id, trianer);


            if (Result)
            {
                TempData["SuccessMessage"] = "Trainer Updated Successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "Trainer Updated Failed , check Phone and Email";
                return RedirectToAction(nameof(Index));
            }
        }

        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Of Trainer can Not be 0 or Negative Number";
                return RedirectToAction(nameof(Index));
            }

            var trainer = _trainerService.GetTrainerDetails(id);

            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.trainerId = id;
            return View();
        }


        [HttpPost]
        public ActionResult DeleteConfirmed(int id)
        {
            var Result = _trainerService.RemoveTrainer(id);

            if (Result)
                TempData["SuccessMessage"] = "Trainer Deleted Successfully";


            else
                TempData["ErrorMessage"] = "Trainer Deleted Failed";

            return RedirectToAction(nameof(Index));

        }


    }
}

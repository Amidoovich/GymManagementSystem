using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.PlanViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class PlanController : Controller
    {
        private readonly IPlanService _planService;

        public PlanController(IPlanService planService)
        {
            _planService = planService;
        }


        public ActionResult Index()
        {
            
            var Plans = _planService.GetAllPlans();
            
            return View(Plans);
        }


        public ActionResult Details(int id)
        {
            if(id <= 0)
            {
                TempData["ErrorMessage"] = "id must be valid";
                return RedirectToAction(nameof(Index));
            }

            var plan = _planService.GetPlanDetails(id);

            if(plan is null)
            {
                TempData["ErrorMessage"] = "plan not found";
                return RedirectToAction(nameof(Index));
            }

            return View(plan);

        }

        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "id must be valid";
                return RedirectToAction(nameof(Index));
            }

            var plan = _planService.GetPlanToUpdate(id);

            if(plan is null)
            {
                TempData["ErrorMessage"] = "plan not found";
                return RedirectToAction(nameof(Index));
            }

            return View(plan);

        }


        [HttpPost]
        public ActionResult Edit([FromRoute]int id,UpdatePlanViewModel updatePlan)
        {
            if (!ModelState.IsValid)
                return View(updatePlan);

            var Result = _planService.UpdatePlan(id, updatePlan);

            if(Result)
            {
                TempData["SuccessMessage"] = "Plan Updated Successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "Plan Failed to Update";
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpPost]
        public ActionResult Activate(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "id must be valid";
                return RedirectToAction(nameof(Index));
            }

            var Result = _planService.ToggleStatus(id);


            if(Result)
                TempData["SuccessMessage"] = "Plan Updated Successfully";
            else
                TempData["ErrorMessage"] = "Plan Failed to Update";


            return RedirectToAction(nameof(Index));
        }
    }
}

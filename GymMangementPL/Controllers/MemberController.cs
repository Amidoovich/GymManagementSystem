using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GymManagementPL.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;

        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        #region Get All Members
        public ActionResult Index()
        {

            var Members = _memberService.GetAllMembers();
            return View(Members);
        }
        #endregion


        #region Get Member Data 

        public ActionResult MemberDetails(int id)
        {

            if (id <= 0)
            {

                TempData["ErrorMessage"] = "Id Of Member can Not be 0 or Negative Number";
                return RedirectToAction(nameof(Index));
            }

            var Member = _memberService.GetMemberDetails(id);
            
            if (Member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }


            
            return View(Member);
        }

        public ActionResult HealthRecordDetails([FromRoute]int id)
        {
            if(id <= 0)
            {

                TempData["ErrorMessage"] = "Id Of Health Record can Not be 0 or Negative Number";
                return RedirectToAction(nameof(Index));
            }

            var HealthRecord = _memberService.GetMemberHealthRecordDetails(id);

            if (HealthRecord is null)
            {
                TempData["ErrorMessage"] = "Health Record Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(HealthRecord);
        }
        #endregion


        #region Add Member

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateMember(CreateMemberViewModel createMember)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvalid", "Check Data And Mising Fields");
                return View(nameof(Create),createMember);
            }

            bool Result = _memberService.CreateMember(createMember);

            if(Result)
            {
                TempData["SuccessMessage"] = "Member Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "Member Failed Create , check Phone and Email";
                return RedirectToAction(nameof(Index));
            }
        }


        #endregion

        #region Update Member


        public ActionResult Edit(int id)
        {

            if (id <= 0)
            {

                TempData["ErrorMessage"] = "Id Of Member can Not be 0 or Negative Number";
                return RedirectToAction(nameof(Index));
            }

            var Member = _memberService.GetMemberToUpdate(id);

            if (Member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(Member);
        }


        [HttpPost]
        public ActionResult Edit([FromRoute]int id,MemberToUpdateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);


            var Result = _memberService.UpdateMember(id, viewModel);


            if (Result)
            {
                TempData["SuccessMessage"] = "Member Updated Successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "Member Failed to Update , check Phone and Email";
                return RedirectToAction(nameof(Index));
            }

        }

        #endregion

        #region Delete Member

        public ActionResult Delete(int id)
        {
            if(id <= 0)
            {

                TempData["ErrorMessage"] = "Id Of Member can Not be 0 or Negative Number";
                return RedirectToAction(nameof(Index));
            }


            var Member = _memberService.GetMemberDetails(id);

            if (Member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.MemberId = id;
            return View();
        }

        [HttpPost]
        public ActionResult DeleteConfirm(int id)
        {
            var Result = _memberService.RemoveMember(id);

            if (Result)
                TempData["SuccessMessage"] = "Member Deleted Successfully";


            else
                TempData["ErrorMessage"] = "Member Failed to Delete ";



            return RedirectToAction(nameof(Index));

        }

        #endregion

    }
}

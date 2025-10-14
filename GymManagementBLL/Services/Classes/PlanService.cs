using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.PlanViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    internal class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlanService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public IEnumerable<PlanViewModel> GetAllPlans()
        {
            var Plans = _unitOfWork.GetRepository<Plan>().GetAll();

            if (Plans == null || !Plans.Any()) return [];



            return Plans.Select(P => new PlanViewModel()
            {
                Description = P.Description,
                DurationDays = P.DurationDays,
                Id = P.Id,
                IsActive = P.IsActive,
                Name = P.Name,
                Price = P.Price,
            });
        }

        public PlanViewModel? GetPlanDetails(int id)
        {
            var Plan = _unitOfWork.GetRepository<Plan>().GetById(id);
            if (Plan == null) return null;

            return new PlanViewModel()
            {
                Description = Plan.Description,
                DurationDays = Plan.DurationDays,
                Id = Plan.Id,
                IsActive = Plan.IsActive,
                Name = Plan.Name,
                Price = Plan.Price,
            };
        }

        public UpdatePlanViewModel? GetPlanToUpdate(int planId)
        {
            
            
            
            
            var Plan = _unitOfWork.GetRepository<Plan>().GetById(planId);

            if (Plan is null || HasActiveMemberShips(planId)) return null;

            return new UpdatePlanViewModel()
            {
                Description = Plan.Description,
                DurationDays = Plan.DurationDays,
                PlanName = Plan.Name,
                Price = Plan.Price
            };


                

        }

        public bool ToggleStatus(int planId)
        {


            var planRepo = _unitOfWork.GetRepository<Plan>();
            var plan = planRepo.GetById(planId);

            if (plan is null || HasActiveMemberShips(planId)) return false;

            
            plan.IsActive = !plan.IsActive;

            try
            {
                planRepo.Update(plan);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }




        }

        public bool UpdatePlan(int planId, UpdatePlanViewModel updatePlan)
        {

            try
            {
                var planRepo = _unitOfWork.GetRepository<Plan>();


                var Plan = planRepo.GetById(planId);


                if (Plan is null || HasActiveMemberShips(planId)) return false;

                (Plan.Description, Plan.Price, Plan.DurationDays, Plan.UpdatedAt) = (updatePlan.Description, updatePlan.Price, updatePlan.DurationDays, DateTime.Now);

                planRepo.Update(Plan);

                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }

        }


        #region Helper Methods


        private bool HasActiveMemberShips(int planId)
        {
            return _unitOfWork.GetRepository<MemberShip>().GetAll(MS => MS.PlanId == planId && MS.Status == "Active").Any();
        } 

        #endregion
    }
}

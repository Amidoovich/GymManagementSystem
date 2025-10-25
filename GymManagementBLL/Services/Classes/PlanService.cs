using AutoMapper;
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
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PlanService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public IEnumerable<PlanViewModel> GetAllPlans()
        {
            var Plans = _unitOfWork.GetRepository<Plan>().GetAll();

            if (Plans == null || !Plans.Any()) return [];

            var PlansMapped = _mapper.Map<IEnumerable<PlanViewModel>>(Plans);

            return PlansMapped;


        }

        public PlanViewModel? GetPlanDetails(int id)
        {
            var Plan = _unitOfWork.GetRepository<Plan>().GetById(id);
            if (Plan == null) return null;

            var PlanMapped = _mapper.Map<PlanViewModel>(Plan);

            return PlanMapped;
        }

        public UpdatePlanViewModel? GetPlanToUpdate(int planId)
        {
            
            
            
            
            var Plan = _unitOfWork.GetRepository<Plan>().GetById(planId);

            if (Plan is null || HasActiveMemberShips(planId)) return null;

          
            var PlanMappedToUpdate = _mapper.Map<UpdatePlanViewModel>(Plan);

            return PlanMappedToUpdate;    

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

                _mapper.Map(updatePlan,Plan);
             



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

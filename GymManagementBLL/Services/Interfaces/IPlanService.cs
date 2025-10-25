using GymManagementBLL.ViewModels.PlanViewModels;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface IPlanService
    {
        IEnumerable<PlanViewModel> GetAllPlans();

        PlanViewModel? GetPlanDetails(int id);

        UpdatePlanViewModel? GetPlanToUpdate(int planId);

        bool UpdatePlan(int planId, UpdatePlanViewModel updatePlan);

        bool ToggleStatus(int planId);

    }
}

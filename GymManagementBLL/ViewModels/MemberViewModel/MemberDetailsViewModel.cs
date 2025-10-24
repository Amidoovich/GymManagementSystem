using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.MemberViewModel
{
    public class MemberDetailsViewModel : MemberViewModel
    {
        public string PlanName { get; set; } = null!;
        public string DateOfBirth { get; set; } = null!;
        public string MemberShipStartDate { get; set; } = null!;
        public string MemberShipEndDate { get; set; } = null!;
        public int BuildingNumber { get; set; } 
        public string Address { get; set; } = null!;
    }
}

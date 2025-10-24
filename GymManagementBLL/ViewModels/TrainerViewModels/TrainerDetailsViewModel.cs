using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.TrainerViewModels
{
    public class TrainerDetailsViewModel : TrainerViewModel
    {
        public string Address { get; set; } = null!;
        public string DateOfBirth { get; set; } = null!;
    }
}

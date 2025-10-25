using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.SessionViewModels
{
    public class SessionToUpdateViewModel
    {
        [Required(ErrorMessage = "Trainer is Required")]
        [Display(Name = "Trainer")]
        public int TrainerId { get; set; }

        [Required(ErrorMessage = "Description is Required")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Description Must be between 10 and 500 char")]
        [Display(Name = "Description")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Start Date is Required")]
        [Display(Name = "Start Date & Time")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End Date is Required")]
        [Display(Name = "End Date & Time")]
        public DateTime EndDate { get; set; }

    }
}

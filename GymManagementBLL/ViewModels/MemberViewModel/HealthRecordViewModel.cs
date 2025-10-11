using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.MemberViewModel
{
    internal class HealthRecordViewModel
    {
        [Required(ErrorMessage = "Height Is Required")]
        [Range(0.1,300,ErrorMessage ="Hieght Must be Between 0.1 and 300 ")]
        public decimal Height { get; set; }
        [Required(ErrorMessage = "weight Is Required")]
        [Range(0.1, 500, ErrorMessage = "wieght Must be Between 0.1 and 500 ")]
        public decimal weight { get; set; }
        [Required(ErrorMessage = "BloodType Is Required")]
        [StringLength(3,ErrorMessage ="Blood Type Must be between 3 Chars or 1 Less")]
        public string BloodType { get; set; } = null!;

        public string? Note { get; set; }
    }
}

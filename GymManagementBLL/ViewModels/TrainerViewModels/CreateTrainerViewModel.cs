using GymManagementDAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.TrainerViewModels
{
    public class CreateTrainerViewModel
    {
        [Required(ErrorMessage = "Name Is Required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Name Must Be Between 2 and 50 Char")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name Can Contain Only Letters And Spaces")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Email Is Required")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Email Must Be Between 5 and 100 Char")]
        [EmailAddress(ErrorMessage = "Invalid Email Format")]
        
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Phone Is Required")]
        [Phone(ErrorMessage = "Invalid Phone Format")]
        
        // \d => digit numbers , {8} => 8 digit Numbers
        [RegularExpression(@"^(010||011||012||015)\d{8}$", ErrorMessage = "Phone Number Must be valid Egyption Phone Number")]
        public string Phone { get; set; } = null!;
        [Required(ErrorMessage = "Date Of Birth Is Required")]
       
        public DateOnly DateOfBirth { get; set; }
        [Required(ErrorMessage = "Gender Is Required")]
        public Gender Gender { get; set; }
        [Required(ErrorMessage = "BuildingNumber Is Required")]
        [Range(1, 1000, ErrorMessage = "BuildingNumber Must be between 1 and 1000")]
        public int BuildingNumber { get; set; }
        [Required(ErrorMessage = "Street Is Required")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Street Must Be Between 2 and 30 Char")]
        public string Street { get; set; } = null!;
        [Required(ErrorMessage = "City Is Required")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "City Must Be Between 2 and 30 Char")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "City Can Contain Only Letters And Spaces")]
        public string City { get; set; } = null!;
        [Required(ErrorMessage = "Specialties Is Required")]
        public Specialties Specialties { get; set; }
    }
}

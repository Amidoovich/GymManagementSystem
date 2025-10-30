﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.AccountViewModels
{
    public class AccountViewModel
    {
        [Required(ErrorMessage ="Email Is Required")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Email Is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        public bool RememberMe { get; set; }
    }
}

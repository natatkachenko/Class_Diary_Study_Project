﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TestProject.ViewModels
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Не указан Email!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Не указана роль!")]
        public string roleName { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Не указан пароль!")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароль введен неверно!")]
        public string ConfirmPassword { get; set; }
    }
}

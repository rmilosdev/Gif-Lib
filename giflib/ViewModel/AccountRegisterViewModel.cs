using giflib.Repository;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Xunit;

namespace giflib.ViewModel
{
    public class AccountRegisterViewModel
    {
        [Required, MinLength(8)]
        public string Username { get; set; }
        [Required, StringLength(maximumLength:100, MinimumLength = 6)]
        public string Password { get; set; }
        [Display(Name = "Confim Password"), System.Web.Mvc.Compare("Password",
            ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public SelectList RoleListItems { get; set; }

        public void Init(RoleRepository roleRepository)
        {
            RoleListItems
                =  new SelectList(roleRepository.GetList(), "Name", "Name");
            
        }
    }
}
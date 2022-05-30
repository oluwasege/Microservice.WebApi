using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using UserAccount.Core.Enum;

namespace UserAccount.Entities.Models
{
    //phone Number, email , password, state of residence, and LGA.
    public class ApplicationUser: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ResetPasswordToken { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public ApplicationUser()
        {
            Id = Guid.NewGuid().ToString();
            CreatedOnUtc = DateTime.UtcNow;
        }
        public string MiddleName { get; set; }
        public State State { get; set; }
        public string StateId { get; set; }
        public Gender Gender { get; set; }
        [NotMapped]
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }

        public DateTime CreatedOnUtc { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime? ModifiedOnUtc { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public bool Activated { get; set; }
        public bool IsDeleted { get; set; }
    }
}

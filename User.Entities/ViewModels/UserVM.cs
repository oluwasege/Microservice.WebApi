using System;
using System.Collections.Generic;
using System.Text;
using UserAccount.Entities.Models;

namespace UserAccount.Entities.ViewModels
{
    public class UserVM
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string State { get; set; }
        public string LGA { get; set; }

        public static implicit operator UserVM(ApplicationUser model)
        {
            return model == null
                ? null
                : new UserVM()
                {
                    Id = model.Id,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    State = model.State.Name,
                    LGA = model.State.LGA
                };
        }
    }
}

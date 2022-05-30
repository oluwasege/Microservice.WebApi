using System;
using System.Collections.Generic;
using System.Text;

namespace UserAccount.Entities.Models
{
    public class State
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string LGA { get; set; }
        public State()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}

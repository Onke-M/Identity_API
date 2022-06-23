using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment3_API.ViewModels
{
    public class UserViewModel
    {
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
        public string Password { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace apiCatedra3.src.Dtos.Auth
{
    public class RegsiterDto
    {
        [Required]
        [EmailAddress]
        public string Email {get; set;} = string.Empty;
        [Required]
        [MinLength(6)]
        public string Password {get; set;} = string.Empty;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiCatedra3.src.Dtos
{
    public class UserDto
    {
        public string Email {get; set;} = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
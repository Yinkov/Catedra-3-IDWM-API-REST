using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace apiCatedra3.src.models
{
    public class AppUser : IdentityUser
    {
        public List<Post> posts {get;set ;}= new List<Post>();
    }
}
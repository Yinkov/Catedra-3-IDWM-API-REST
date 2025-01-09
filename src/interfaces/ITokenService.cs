using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apiCatedra3.src.models;

namespace apiCatedra3.src.interfaces
{
    public interface ITokenService
    {
         string CreateToken(AppUser user);
    }
}
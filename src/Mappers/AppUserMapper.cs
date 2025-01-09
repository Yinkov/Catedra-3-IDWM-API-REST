using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apiCatedra3.src.Dtos;
using apiCatedra3.src.Dtos.Auth;
using apiCatedra3.src.interfaces;
using apiCatedra3.src.models;

namespace apiCatedra3.src.Mappers
{
    public static class AppUserMapper
    {

        public static UserDto ToDto(this AppUser user, ITokenService _tokenService)
        {
            return new UserDto
            {
                Email = user.Email ?? "error en el correo",
                Token = _tokenService.CreateToken(user)
            };
        }

        public static AppUser ToModel(this RegsiterDto registerDto)
        {
            return new AppUser
            {
                Email = registerDto.Email,
                UserName = registerDto.Email
            };
        }
    }
}
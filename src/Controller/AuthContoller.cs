using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apiCatedra3.src.Dtos.Auth;
using apiCatedra3.src.interfaces;
using apiCatedra3.src.Mappers;
using apiCatedra3.src.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apiCatedra3.src.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;
        
        public AuthController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager){
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegsiterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                
                // Verifica si el correo ya está registrado
                var usuarioExistente = await _userManager.FindByEmailAsync(registerDto.Email);
                if (usuarioExistente != null)
                {
                    return BadRequest("El correo ya está registrado.");
                }

               
                var appUser = registerDto.ToModel();


                var usuarioCreado = await _userManager.CreateAsync(appUser, registerDto.Password);

                if (usuarioCreado.Succeeded)
                {
                    
                    var UserDto = appUser.ToDto(_tokenService);
                    

                    return Ok(new 
                { 
                    Message = "Usuario creado correctamente.", 
                    User = UserDto 
                });
                        
                    
                }
                else
                {
                    return StatusCode(500, new { Errores = usuarioCreado.Errors.Select(e => e.Description) });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] RegsiterDto loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Busca al usuario por correo
                var appUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == loginDto.Email);

                // Verifica si el usuario no existe
                if (appUser == null)
                {
                    return Unauthorized("Correo o contraseña incorrectos.");
                }


                // Verifica la contraseña
                var result = await _signInManager.CheckPasswordSignInAsync(appUser, loginDto.Password, false);
                if (!result.Succeeded)
                {
                    return Unauthorized("Correo o contraseña incorrectos.");
                }

                var UserDto = appUser.ToDto(_tokenService);
                    

                return Ok(new 
                { 
                    Message = "Login exitoso.", 
                    User = UserDto 
                });
            }
            catch (Exception ex)
            {
                // Manejo de errores genéricos
                return StatusCode(500, ex.Message);
            }
        }

    }
}
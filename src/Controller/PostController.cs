using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using apiCatedra3.src.Dtos.Post;
using apiCatedra3.src.Helpers;
using apiCatedra3.src.interfaces;
using apiCatedra3.src.Mappers;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apiCatedra3.src.Controller
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class PostController : ControllerBase
    {

        public readonly IPostRepository _postRepository;
        private readonly Cloudinary _cloudinary;

        public PostController(Cloudinary cloudinary, IPostRepository postRepository){
            _cloudinary = cloudinary;
            _postRepository = postRepository;
        }

        [HttpPost]
        [Authorize] 
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> RegisterPost([FromForm] CreatePostDto createPostDto)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.Email);
                if (userId == null)
                {
                    return Unauthorized("Usuario no autenticado.");
                }
                // Validar modelo inicial
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }



                if(createPostDto.Image == null || createPostDto.Image.Length == 0){
                    return BadRequest("la imagen es requeridad.");
                }
                if(createPostDto.Image.ContentType != "image/jpeg" &&
                   createPostDto.Image.ContentType != "image/png"){
                    return BadRequest("la imagen debe ser en formato jpeg o png.");
                }
                if(createPostDto.Image.Length > 5 * 1024 * 1024){
                    return BadRequest("la imagen debe ser menor a 5mb.");
                }

                var uploadParams = new ImageUploadParams{
                    File = new FileDescription(createPostDto.Image.FileName, createPostDto.Image.OpenReadStream()),
                    Folder= "catedra3_IDWM"
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if(uploadResult.Error != null){
                    return BadRequest(uploadResult.Error.Message);
                }



                // Crear el producto y añadirlo a la base de datos
                var newPost = createPostDto.ToModel(uploadResult); 

                await _postRepository.AddAsync(newPost,userId);

                return Ok("Producto añadido exitosamente.");
                
            }
            catch(Exception ex){
                return BadRequest(ex.Message);
            }
            
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllPost([FromQuery] QueryObject queryObject)
        {
            // Validar parámetros de paginación
            if (queryObject.pageNumber < 1 || queryObject.pageSize < 1)
            {
                return BadRequest("Los parámetros de paginación deben ser mayores a 0.");
            }

            var (posts,totalPost) = await _postRepository.GetPaginatedPosts(queryObject);

            return Ok(new
            {
                Total = totalPost,
                PaginaActual = queryObject.pageNumber,
                TamañoPagina = queryObject.pageSize,
                Posts = posts
            });
        }

        
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apiCatedra3.src.Dtos.Post;
using apiCatedra3.src.models;
using CloudinaryDotNet.Actions;

namespace apiCatedra3.src.Mappers
{
    public static class PostMapper
    {
       /* public static CreatePostDto ToDto(this AppUser user, ITokenService _tokenService)
        {
            return new UserDto
            {
                Email = user.Email ?? "error en el correo",
                Token = _tokenService.CreateToken(user)
            };
        }*/

        public static Post ToModel(this CreatePostDto createPostDto, ImageUploadResult imageUpload)
        {
            return new Post
            {
                Title = createPostDto.Title,
                publicationDate = DateTime.Now,
                linkImage = imageUpload.SecureUrl.AbsoluteUri,
                
                
            };
        }
        
    }
}
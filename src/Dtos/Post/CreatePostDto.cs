using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace apiCatedra3.src.Dtos.Post
{
    public class CreatePostDto
    {
        [Required]
        [MinLength(5)]
        public string Title {get; set;} = string.Empty;
        [Required]
        public IFormFile Image {get; set;} = null!;
    }
}
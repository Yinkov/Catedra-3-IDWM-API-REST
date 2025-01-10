using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiCatedra3.src.Dtos.Post
{
    public class PostDto
    {
        public int Id { get; set; }
        public string Title {get; set;} = string.Empty;
        public DateTime PublicationDate {get; set;}
        public string LinkImage {get; set;} = string.Empty;
    }
}
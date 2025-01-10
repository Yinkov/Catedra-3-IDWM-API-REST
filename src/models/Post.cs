using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiCatedra3.src.models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title {get; set;} = string.Empty;
        public DateTime publicationDate {get; set;}
        public string linkImage {get; set;} = string.Empty;

        // Relación con AppUser
        public string UserId { get; set; } = string.Empty; 
        public AppUser? User { get; set; } 
         
    }
}
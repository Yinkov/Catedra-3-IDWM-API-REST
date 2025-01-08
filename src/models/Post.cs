using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiCatedra3.src.models
{
    public class Post
    {
        public int Id { get; set; }
        private string Title {get; set;} = string.Empty;
        private DateTime publicationDate {get; set;}
        private string linkImage {get; set;} = string.Empty;
    }
}
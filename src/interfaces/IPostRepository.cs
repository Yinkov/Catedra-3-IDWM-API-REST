using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apiCatedra3.src.Dtos.Post;
using apiCatedra3.src.Helpers;
using apiCatedra3.src.models;

namespace apiCatedra3.src.interfaces
{
    public interface IPostRepository
    {
        Task<Post> AddAsync(Post post, string userId);
        Task<(List<PostDto> posts, int total)> GetPaginatedPosts(QueryObject queryObject);
    }
}
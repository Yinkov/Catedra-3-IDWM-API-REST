using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apiCatedra3.src.Data;
using apiCatedra3.src.Dtos.Post;
using apiCatedra3.src.Helpers;
using apiCatedra3.src.interfaces;
using apiCatedra3.src.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace apiCatedra3.src.Repositories
{
    public class PostRepository : IPostRepository
    {
        public readonly AplicationDBContext _appDbContext;

        private readonly UserManager<AppUser> _userManager;

        public PostRepository(AplicationDBContext appDbContext,UserManager<AppUser> userManager){
            _appDbContext = appDbContext;
            _userManager = userManager;
        }
        public async Task<Post> AddAsync(Post post, string userId)
        {
            // Obtener al usuario por su ID
            var user = await _userManager.FindByEmailAsync(userId);

            if (user == null)
            {
                throw new Exception("Usuario no encontradoaaaaaa."); // Manejo de errores adecuado
            }

            // Asociar el Post al usuario
            post.UserId = user.Id;

            // Agregar el Post a la base de datos
            await _appDbContext.posts.AddAsync(post);

            // Guardar los cambios en la base de datos
            await _appDbContext.SaveChangesAsync();

            return post;
        }

        public async Task<(List<PostDto> posts, int total)> GetPaginatedPosts(QueryObject queryObject)
        {
            // Validar parámetros de paginación
            if (queryObject.pageNumber < 1 || queryObject.pageSize < 1)
            {
                throw new ArgumentException("Los parámetros de paginación deben ser mayores a 0.");
            }

            var query = _appDbContext.posts.AsQueryable();

            // Calcular paginación
            var skipNumber = (queryObject.pageNumber - 1) * queryObject.pageSize;
            var total = await query.CountAsync();

            var posts = await query
                .OrderByDescending(p => p.publicationDate)
                .Skip(skipNumber)
                .Take(queryObject.pageSize)
                .Select(p => new PostDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    LinkImage = p.linkImage,
                    PublicationDate = p.publicationDate
                })
                .ToListAsync();

            return (posts, total);
        }
    }
}
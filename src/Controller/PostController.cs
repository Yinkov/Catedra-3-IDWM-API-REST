using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace apiCatedra3.src.Controller
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class PostController : ControllerBase
    {

        [HttpPost]
        [Authorize] 
        public async Task<IActionResult> RegisterProduct([FromForm] CreateProductDto createProductDto, IFormFile imageFile)
        {
            // Validar modelo inicial
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validar si ya existe un producto con el mismo nombre y tipo
            var exists = await _context.Products.AnyAsync(p => p.Nombre == createProductDto.Nombre && p.Tipo == createProductDto.Tipo);
            if (exists) 
            {
                return BadRequest("Ya existe un producto con el mismo nombre y tipo.");
            }

            // Validar y subir la imagen si se proporciona
            string uploadedImageUrl = string.Empty; // Para guardar la URL de la imagen subida
            if (imageFile != null)
            {
                var allowedFormats = new[] { "image/jpeg", "image/png" };
                if (!allowedFormats.Contains(imageFile.ContentType))
                {
                    return BadRequest("Solo se permiten archivos .jpg o .png.");
                }

                if (imageFile.Length > 10 * 1024 * 1024) // 10 MB
                {
                    return BadRequest("El tamaño de la imagen no debe exceder los 10 MB.");
                }

                try
                {
                    // Subir la imagen a Cloudinary
                    Cloudinary cloudinary = new Cloudinary(_cloudinaryUrl);
                    using var stream = imageFile.OpenReadStream();
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(imageFile.FileName, stream),
                        UseFilename = true,
                        Overwrite = true,
                        Folder = "taller-backend"
                    };

                    var uploadResult = cloudinary.Upload(uploadParams);
                    uploadedImageUrl = uploadResult.SecureUrl?.ToString();

                    if (string.IsNullOrEmpty(uploadedImageUrl))
                    {
                        return BadRequest("Error al subir la imagen a Cloudinary.");
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Error al subir la imagen: {ex.Message}");
                }
            }

            else
            {
                return BadRequest("Por favor, suba una imagen.");
            }

            // Crear el producto y añadirlo a la base de datos
            var newProduct = new Product
            {
                Nombre = createProductDto.Nombre,
                Tipo = createProductDto.Tipo,
                Precio = createProductDto.Precio,
                Stock = createProductDto.Stock,
                LinkImagen = uploadedImageUrl // Guardar la URL de la imagen subida
            };

            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();

            return Ok("Producto añadido exitosamente.");
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] QueryObject queryObject)
        {
            // Validar parámetros de paginación
            if (queryObject.numeroPagina < 1 || queryObject.tamañoPagina < 1)
            {
                return BadRequest("Los parámetros de paginación deben ser mayores a 0.");
            }

            var query = _context.Products.AsQueryable();

            // Verificar el rol del usuario
            var isAuthenticated = User.Identity.IsAuthenticated; // Verificar si el usuario está autenticado
            var isAdmin = User.IsInRole("Admin"); // Verificar si el usuario es Admin

            // Si no es Admin, solo mostrar productos con stock > 0
            if (!isAdmin)
            {
                query = query.Where(p => p.Stock > 0);
            }

            // Filtrar por nombre o tipo si 'SortBy' está presente
            if (!string.IsNullOrWhiteSpace(queryObject.SortBy))
            {
                query = query.Where(p => p.Nombre.Contains(queryObject.SortBy) || p.Tipo.Contains(queryObject.SortBy));
            }

            // Calcular la paginación
            var skipNumber = (queryObject.numeroPagina - 1) * queryObject.tamañoPagina;

            var totalProducts = await query.CountAsync();

            // Obtener los productos con paginación
            var productsQuery = query
                .OrderBy(p => p.Nombre) // Ordenar por nombre
                .Skip(skipNumber)
                .Take(queryObject.tamañoPagina);

            // Si el usuario es Admin o está autenticado, incluir el Id
            var products = await productsQuery
                .Select(p => new
                {
                    Id = (isAuthenticated || isAdmin) ? p.Id : (int?)null, // Mostrar Id solo si es Admin o autenticado
                    p.Nombre,
                    p.Tipo,
                    p.Precio,
                    p.Stock,
                    p.LinkImagen
                })
                .ToListAsync();

            return Ok(new
            {
                Total = totalProducts,
                PaginaActual = queryObject.numeroPagina,
                TamañoPagina = queryObject.tamañoPagina,
                Productos = products
            });
        }

        
    }
}
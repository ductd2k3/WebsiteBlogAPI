using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebsiteBlog.Models;
using WebsiteBlog.Repository;

namespace WebsiteBlog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryController(ICategoryRepository categoryRepository) 
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(new Response { Success = "true", Data = _categoryRepository.GetAll() });
        }
    }
}

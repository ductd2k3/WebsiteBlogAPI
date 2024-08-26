using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using WebsiteBlog.Models;
using WebsiteBlog.Repository;
using WebsiteBlog.Service;

namespace WebsiteBlog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogRepository _blogRepository;
        public BlogController(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        [HttpGet("numberOfBlogs")]
        public IActionResult Get(int? start, int? end)
        {
            int _start = start ?? 0;
            int _end = end ?? 3;
            var blogs = _blogRepository.Paging(_start, _end);
            return Ok(new Response {Success = "true",Data = blogs });
        }
        [HttpPost("Create")]
        [Authorize]
        public async Task<IActionResult> CreateBlog([FromBody] DTOBlog DTOBlog)
        {
            var processedContent = await FormatContent(DTOBlog.Content);
            DTOBlog.Content = processedContent;
            string img = await FormatContent(DTOBlog.Image);
            Blog blog = new Blog 
            { 
                Title = DTOBlog.Title,
                UserId = DTOBlog.UserId,
                Content = DTOBlog.Content,
                CategoryId = DTOBlog.CatrogryId,
                BlogImage = img,
                CreateAt = DateTime.Now    
            };
            _blogRepository.Add(blog);
            return Ok(new Response { Success = "true", Data = DTOBlog});
        }
        private async Task<string> FormatContent(string content)
        {
            var regex = new Regex(@"data:image/(?<type>[a-zA-Z]+);base64,(?<data>[a-zA-Z0-9+/=]+)");

            var matches = regex.Matches(content);
            foreach (Match match in matches)
            {
                try
                {
                    var base64Data = match.Groups["data"].Value;
                    var imageType = match.Groups["type"].Value;
                    var imageData = Convert.FromBase64String(base64Data);

                    var fileName = $"{Guid.NewGuid()}.{imageType}";
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Images", fileName);

                    await System.IO.File.WriteAllBytesAsync(filePath, imageData);

                    var scheme = HttpContext.Request.Scheme;
                    var host = HttpContext.Request.Host;

                    var imageUrl = $"{scheme}://{host}/uploads/{fileName}";

                    content = content.Replace(match.Value, imageUrl);
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }


            return content;
        }
        [HttpGet("Get")]
        public IActionResult GetBlogById(int id)
        {
            var blog = _blogRepository.GetById(id);
            if(blog == null)
            {
                return Ok(new Response { Success = " false"});
            }
            return Ok(new Response { Success = "true", Data = blog});
        }

        

    }
}

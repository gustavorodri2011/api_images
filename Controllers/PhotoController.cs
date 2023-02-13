using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api_images.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post()
        {
            var file = Request.Form.Files[0];
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "photos", file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return Ok();
        }
    }
}

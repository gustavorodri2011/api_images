using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api_images.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IWebHostEnvironment env;

        public ImageController(IWebHostEnvironment env)
        {
            this.env = env;
        }

        [HttpGet("{ntramite}")]
        public IActionResult Get(string ntramite)
        {
            try
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "Images", ntramite + ".jpg");
                var image = System.IO.File.OpenRead(path);
                return File(image, "image/jpeg");
            }
            catch (Exception)
            {
                return NotFound($"No existe foto para el tramite: {ntramite}.");
            }
        }

        [HttpPost]
        public IActionResult UploadImage([FromForm] Photo photo)
        {
            try
            {
                // Obtén el nombre del archivo
                var fileName = Path.GetFileName(photo.FormFile.FileName);

                string extension = Path.GetExtension(fileName);

                // Obtén la ruta de destino
                string folderPath = Path.Combine(env.WebRootPath, "images");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                var filePath = Path.Combine(folderPath, photo.Tramite + extension);

                // Guarda el archivo en la ruta especificada
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    photo.FormFile.CopyTo(stream);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{ntramite}")]
        public IActionResult Delete(string ntramite)
        {
            string folderPath = Path.Combine(env.WebRootPath, "images");

            DirectoryInfo directory = new DirectoryInfo(folderPath);
            FileInfo[] files = directory.GetFiles();

            // Eliminar archivo
            foreach (FileInfo file in files)
            {
                if (file.Name == ntramite + ".jpg")
                {
                    file.Delete();
                }
            }
            return NoContent();
        }

        [HttpDelete]
        public IActionResult Delete()
        {
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Images");

            DirectoryInfo directory = new DirectoryInfo(folderPath);
            FileInfo[] files = directory.GetFiles();

            // Eliminar cada archivo
            foreach (FileInfo file in files)
            {
                file.Delete();
            }
            return NoContent();
        }
    }
}

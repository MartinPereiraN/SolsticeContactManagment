using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Solstice.CodingChallenge.API.Dtos.Responses;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Solstice.CodingChallenge.API.Controllers
{
    [Route("api/[controller]")]
    public class FilesController : Controller
    {
        [HttpPost]
        [ProducesResponseType(typeof(FileResponseDto), 200)]
        public async Task<IActionResult> UploadFile([FromForm]IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Content("file not selected");

            string fileName = file.FileName;
            string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            fileName = fileName.Replace(Path.GetExtension(fileName), timeStamp + Path.GetExtension(fileName));

            var path = Path.Combine(
                        Directory.GetCurrentDirectory(), "FileUploads",
                       fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new FileResponseDto() { FileName = fileName });
        }

        [HttpGet("{fileName}")]
        public async Task<IActionResult> GetFile([FromRoute] string fileName)
        {
            var path = Path.Combine(
                        Directory.GetCurrentDirectory(), "FileUploads",
                       fileName);
            var image = System.IO.File.OpenRead(path);
            return File(image, "application/octet-stream");
        }
    }
}

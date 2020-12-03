using AndroidKotlinServer.Photo.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AndroidKotlinServer.Photo.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> SavePhoto(IFormFile photo, CancellationToken cancellationToken)
        {
            //cancellation token; eğer büyük bir dosya gelmişse o işlemi yarıda kesilirse metodu durdurur.
            if (photo == null && photo.Length <= 0) return BadRequest("Photo is empty");
            var randomFileName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", randomFileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await photo.CopyToAsync(stream, cancellationToken);
            }

            var returnPath = "photos/" + randomFileName;

            return Ok(new
            {
                Url = returnPath
            });
        }

        [HttpDelete]
        public IActionResult DeletePhoto(PhotoDeleteDto photoDeleteDto)
        {
            if (photoDeleteDto.PhotoUrl == null) return BadRequest("Wrong photo url");
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", photoDeleteDto.PhotoUrl);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
                return NoContent();
            }
            return BadRequest();
        }

    }
}

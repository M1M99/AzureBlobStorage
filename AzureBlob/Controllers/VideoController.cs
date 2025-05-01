using AzureBlob.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AzureBlob.Controllers
{
    public class VideoController : Controller
    {
        private readonly IBlobStorageService _blobStorageService;
        private readonly string _videoContainer = "videos";

        public VideoController(IBlobStorageService blobStorageService)
        {
            _blobStorageService = blobStorageService;
        }

        public async Task<IActionResult> Index()
        {
            var video = await _blobStorageService.ListFilesAsync(_videoContainer);
            return View(video);
        }

        public async Task<IActionResult> DownloadVideo(string fileName)
        {
            var stream = await _blobStorageService.DownloadAsync(fileName,_videoContainer);
            return File(stream, "video/mp4", fileName);
        }
        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile formFile)
        {
            if (formFile == null || formFile.Length == 0) return BadRequest("No file");

            using var stream = formFile.OpenReadStream();
            var fileName = Guid.NewGuid() + Path.GetExtension(formFile.FileName);
            await _blobStorageService.UploadAsync(stream, fileName,_videoContainer);
            return RedirectToAction("Index");
        }
    }
}

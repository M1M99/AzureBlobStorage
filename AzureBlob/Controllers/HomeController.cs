using AzureBlob.Models;
using AzureBlob.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AzureBlob.Controllers
{
    public class HomeController : Controller
    {
        //in class but changes somethings
        private readonly ILogger<HomeController> _logger;
        private readonly IBlobStorageService _blobStorageService;
        private readonly string _imgContainer = "images";


        public HomeController(ILogger<HomeController> logger,IBlobStorageService blobStorageService)
        {
            _logger = logger;
            _blobStorageService = blobStorageService;
        }

        public async Task<IActionResult> Index()
        {
            var imgUrls = await _blobStorageService.ListFilesAsync(_imgContainer);
            return View(imgUrls);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File is empty");
            }

            using var stream = file.OpenReadStream();
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var imgurl = await _blobStorageService.UploadAsync(stream, fileName, _imgContainer);
            return RedirectToAction("Index");
        }

        [HttpGet("download/{fileName}")]
        public async Task<IActionResult> DownloadImage(string fileName)
        {
            var stream = await _blobStorageService.DownloadAsync(fileName, _imgContainer);
            return File(stream, "image/jpeg", fileName);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

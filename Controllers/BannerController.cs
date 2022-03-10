using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGeneration;

namespace Banner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BannerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BannerController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("CreateBanner")]
        public async Task<int> CreateBanner([FromForm]BannerDto model)
        {
            var path = $"wwwroot/Images/Product/{model.Queue}";
            var fullPath = Path.GetFullPath(path);
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);    
            }

            var fileExtension = Path.GetExtension(model.Image.FileName);
            var fileName = $"{Guid.NewGuid().ToString()}{fileExtension}";
            var finalFileName = Path.Combine(fullPath, fileName);
            var imagePath = Path.Combine(path, fileName);
            await using (var fileStream = System.IO.File.Create(finalFileName))
            {
                await model.Image.CopyToAsync(fileStream);
            }
            var banner = new Banner
            {
                ImagePath = imagePath,
                Queue = model.Queue
            }; 
            await _context.AddAsync(banner);
            await _context.SaveChangesAsync();
            return 233426859;
        }

        [HttpGet("GetBanner")]
        public async Task<List<Banner>> GetBanner()
        {
            return await _context.Banners.ToListAsync();
        }

        [HttpDelete("DeleteBannerPhoto")]
        public async Task DeleteBannerImg(int id)
        {
            var banner = await _context.Banners.FirstOrDefaultAsync(b => b.Id == id);
            if (System.IO.File.Exists(banner.ImagePath))
                System.IO.File.Delete(banner.ImagePath);
            _context.Banners.Remove(banner);
            await _context.SaveChangesAsync();
        }
    }
}
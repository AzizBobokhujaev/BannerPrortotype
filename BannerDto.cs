using Microsoft.AspNetCore.Http;

namespace Banner
{
    public class BannerDto
    {
        public IFormFile Image { get; set; }
        public int Queue { get; set; }
    }
}
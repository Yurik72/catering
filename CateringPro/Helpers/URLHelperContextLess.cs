using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Helpers
{
    public class URLConfiguration
    {
        public string BaseUrl { get; set; }
    }
    public class URLHelperContextLess
    {
        IConfiguration _configuration;
        URLConfiguration settings;
        public URLHelperContextLess(IConfiguration configuration)
        {
            _configuration = configuration;
            settings = configuration.GetSection("URLConfiguration").Get<URLConfiguration>();
            
        }
        public string BuildFullUrl(string urlend)
        {
            string url = settings.BaseUrl;
            if (!urlend.StartsWith("/"))
                url += "/";
            return url + urlend;
        }
        public string BuildPictureUrl(int? pictureId,int width=50,int height = 50)
        {
            
            string url = BuildFullUrl("Pictures/GetPicture")+"?";
            url += "id=" + (pictureId.HasValue?pictureId.ToString():"-1");
            url += "&";
            url += "width=" + width.ToString();
            url += "&";
            url += "height=" + height.ToString();
            return url;
        }
    }
}

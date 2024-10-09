namespace OfficePortal.Services
{
    public class UserAgentService
    {
        public bool IsMobileDevice(HttpRequest request)
        {
            var userAgent = request.Headers["User-Agent"].ToString().ToLower();
            return userAgent.Contains("iphone") || userAgent.Contains("android") || userAgent.Contains("ipad");
        }

        public bool IsDesktopDevice(HttpRequest request)
        {
            var userAgent = request.Headers["User-Agent"].ToString().ToLower();
            // Assuming anything that is not mobile is desktop
            return !IsMobileDevice(request);
        }
    }



}

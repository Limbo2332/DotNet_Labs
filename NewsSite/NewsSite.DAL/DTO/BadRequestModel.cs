using System.Net;

namespace NewsSite.DAL.DTO
{
    public class BadRequestModel
    {
        public string? Message { get; set; }

        public List<string> Errors { get; set; } = null!;

        public HttpStatusCode HttpStatusCode { get; set; }
    }
}

using System;

namespace ExampleApp.WebApi.Responses.Advertisement
{
    public class AdResponseModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Content { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
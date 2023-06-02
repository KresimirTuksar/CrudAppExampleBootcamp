using ExampleApp.Model.Common;
using System;

namespace ExampleApp.Model
{
    public class AdModel : IAdModel
    {
        public Guid? Id { get; set; }
        public Guid? UserId { get; set; }
        public string Content { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}

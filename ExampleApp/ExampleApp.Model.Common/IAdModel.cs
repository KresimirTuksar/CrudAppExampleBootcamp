using System;

namespace ExampleApp.Model.Common
{
    public interface IAdModel
    {
        Guid? Id { get; set; }
        Guid? UserId { get; set; }
        string Content { get; set; }
        DateTime? CreatedAt { get; set; }
        DateTime? UpdatedAt { get; set; }
    }
}

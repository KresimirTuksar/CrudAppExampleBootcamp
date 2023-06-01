using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

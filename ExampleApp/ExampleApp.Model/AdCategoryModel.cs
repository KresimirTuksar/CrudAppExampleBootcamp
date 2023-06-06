using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleApp.Model
{
    public class AdCategoryModel
    {
        public Guid Id { get; set; }

        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string Content{ get; set; }
        public string Category { get; set; }

        public string User { get 
            {
                return UserFirstName + " " + UserLastName;
            } 
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace News
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Login { get; set; }
        public string Password { get; set; }
    }
}

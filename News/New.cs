using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News
{
    public class New
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Text { get; set; }
    }
}

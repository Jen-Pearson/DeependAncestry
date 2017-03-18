using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Business.Models
{
    public class Data
    {
        public IList<Place> Places { get; set; }
        public IEnumerable<Person> People { get; set; }

    }
}

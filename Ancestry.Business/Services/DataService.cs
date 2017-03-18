using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ancestry.Business.Models;

namespace Ancestry.Business
{
    public class DataService
    {
        public Data ReadFile()
        {
            var rawRecords = File.ReadAllText(@"c:\temp\ancestry\data_small.json");
            return JsonConvert.DeserializeObject<Data>(rawRecords);
        }

        public Data ReadFile(string filePath)
        {
            var rawRecords = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<Data>(rawRecords);
        }

    }
}

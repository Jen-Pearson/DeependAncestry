using System.IO;
using Newtonsoft.Json;
using Ancestry.Business.Models;

namespace Ancestry.Business
{
    public class DataService
    {
        public Data ReadFile(string filePath)
        {
            var rawRecords = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<Data>(rawRecords);
        }

    }
}

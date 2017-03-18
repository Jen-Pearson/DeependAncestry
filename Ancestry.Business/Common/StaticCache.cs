using System.ComponentModel;
using Ancestry.Business.Models;

namespace Ancestry.Business.Common
{
    [System.ComponentModel.DataObject]
    public class StaticCache
    {
            private static Data records = null;
            public static void LoadStaticCache(string filePath)
            {
                var service = new DataService();
                records = service.ReadFile(filePath);
            }

            [DataObjectMethod(DataObjectMethodType.Select, true)]
            public static Data GetData()
            {
                return records;
            }
    }
}

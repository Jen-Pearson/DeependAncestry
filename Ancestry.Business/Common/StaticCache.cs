using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Ancestry.Business.Models;

namespace Ancestry.Business.Common
{
    [System.ComponentModel.DataObject]
    public class StaticCache
    {
        private static Data records = null;
        private static IDictionary<int, string> places = null;
        private static IDictionary<int, Person> people = null;

        public static void LoadStaticCache(string filePath)
        {
            var service = new DataService();
            records = service.ReadFile(filePath);

            places = records.Places.ToDictionary(p => p.Id, p => p.Name);
            people = records.People.ToDictionary(p => p.Id, p => p);
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static Data GetData()
        {
            return records;
        }

        public static string GetPlaceById(int id)
        {
            return places.ContainsKey(id) ? places[id] : null;
        }

        public static Person GetPersonById(int id)
        {
            return people.ContainsKey(id) ? people[id] : null;
        }
    }
}

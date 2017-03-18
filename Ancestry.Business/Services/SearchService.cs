using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ancestry.Business.Models;

namespace Ancestry.Business.Services
{
    public class SearchService
    {
        public List<Person> FindPeople(int? gender, string name)
        {
            DataService service = new DataService();
            var data = service.ReadFile();

            if (data == null)
                return null;
            
            IDictionary<int, string> birthPlaces = data.Places.ToDictionary(p => p.Id, p=> p.Name);
            
            // figure out insensitive case
            var query = data.People.Where(p => p.Name.IndexOf(name, StringComparison.CurrentCultureIgnoreCase) >=0);

            // get gender enum by int
            if (gender.HasValue)
                query = query.Where(p => p.Gender.Equals((Enums.Gender)gender.Value == Enums.Gender.Male ? "M" : "F"));

            var results = query.ToList();
            foreach (var person in results)
            {
                if (birthPlaces.ContainsKey(person.Place_Id))
                    person.BirthPlace = birthPlaces[person.Place_Id];
            }
            return results;
            
        }
    }
}

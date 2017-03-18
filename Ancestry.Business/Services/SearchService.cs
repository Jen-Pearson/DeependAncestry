using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ancestry.Business.Common;
using Ancestry.Business.Models;

namespace Ancestry.Business.Services
{
    public class SearchService
    {
        public List<Person> FindPeople(int? gender, string name)
        {
            var data = StaticCache.GetData();
            if (data == null)
                return null;
            
            var query = data.People.Where(p => p.Name.IndexOf(name, StringComparison.CurrentCultureIgnoreCase) >=0);

            // get gender enum by int
            if (gender.HasValue)
                query = query.Where(p => p.Gender.Equals((Enums.Gender)gender.Value == Enums.Gender.Male ? "M" : "F"));

            var results = query.ToList();
            foreach (var person in results)
            {
                person.BirthPlace = StaticCache.GetPlaceById(person.Place_Id);
            }

            return results;
            
        }

        public List<Person> FindPeopleAndRelations(int? gender, string name, bool includeAncestors, int maxRecordsToDisplay)
        {
            var data = StaticCache.GetData();
            if (data == null)
                return null;

            var query = data.People.Where(p => p.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));

            var personOfInterest = query.FirstOrDefault();

            if (personOfInterest == null)
                return null;

            List<Person> results = null;
            switch (includeAncestors)
            {
                case true:
                    results = GetAncestors(personOfInterest, maxRecordsToDisplay, gender);
                    break;
                case false:
                    results = GetDescendants(personOfInterest, maxRecordsToDisplay, gender);
                    break;
            }

            if (results == null)
                return null;
            
            foreach (var person in results)
            {                
                    person.BirthPlace = StaticCache.GetPlaceById(person.Place_Id);
            }

            return results;

        }

        private List<Person> GetAncestors(Person person, int maxRecordsToDisplay, int? genderToDisplay)
        {
            return FindAllParents(person, new List<Person>(), maxRecordsToDisplay, genderToDisplay);
        }

        private List<Person> FindAllParents(Person person, List<Person> ancestorList, int maxRecordsToDisplay, int? gender)
        {
            if (!person.Mother_Id.HasValue && !person.Father_Id.HasValue)
                return ancestorList;

            if (ancestorList.Count >= maxRecordsToDisplay)
                return ancestorList;

            var includeFemales = true;
            var includeMales = true;

            if (gender.HasValue)
            {
                if ((Enums.Gender) gender.Value == Enums.Gender.Female)
                    includeMales = false;
                else
                    includeFemales = false;
            }

            if (includeFemales && person.Mother_Id.HasValue)
            {
                var mother = StaticCache.GetPersonById(person.Mother_Id.Value);
                if (mother != null)
                {
                    ancestorList.Add(mother);
                    FindAllParents(mother, ancestorList, maxRecordsToDisplay, gender);
                }
            }

            if (includeMales && person.Father_Id.HasValue && DoesResultListHaveCapacity(ancestorList, maxRecordsToDisplay))
            {
                var father = StaticCache.GetPersonById(person.Father_Id.Value);
                if (father != null)
                {
                    ancestorList.Add(father);
                    FindAllParents(father, ancestorList, maxRecordsToDisplay, gender);
                }
            }

            return ancestorList;
        }

        private static bool DoesResultListHaveCapacity(List<Person> ancestorList, int maxRecordsToDisplay)
        {
            return ancestorList.Count < maxRecordsToDisplay;
        }

        private List<Person> GetDescendants(Person person, int maxRecordsToDisplay, int? genderToDisplay)
        {
            return FindAllChildren(person, new List<Person>(), maxRecordsToDisplay, genderToDisplay);
        }

        private List<Person> FindAllChildren(Person person, List<Person> descendentList, int maxRecordsToDisplay, int? gender)
        {
            var query = StaticCache.GetData().People;
            List<Person> data;

            if (gender.HasValue)
            {
                query = query.Where(p => p.Gender.Equals((Enums.Gender) gender.Value == Enums.Gender.Male ? "M" : "F"));

                if ((Enums.Gender) gender.Value == Enums.Gender.Female)
                    query = query.Where(p => p.Mother_Id == person.Id);
                else
                    query = query.Where(p => p.Father_Id == person.Id);

                data = query.Select(p => p).ToList();
            }
            else
                data = query.Where(p => p.Mother_Id == person.Id || p.Father_Id == person.Id).Select(p => p).ToList();

            foreach (var child in data)
            {
                if (DoesResultListHaveCapacity(descendentList, maxRecordsToDisplay))
                {
                    descendentList.Add(child);
                    FindAllChildren(child, descendentList, maxRecordsToDisplay, gender);
                }
                else
                    return descendentList;
            }

            return descendentList;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
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

            var query = data.People.Where(p => p.Name.IndexOf(name, StringComparison.CurrentCultureIgnoreCase) >= 0);

            if (gender.HasValue)
                query = query.Where(p => p.Gender.Equals((Enums.Gender)gender.Value == Enums.Gender.Male ? "M" : "F"));

            var results = query.ToList();
            foreach (var person in results)
            {
                person.BirthPlace = StaticCache.GetPlaceById(person.Place_Id);
            }

            return results;
        }

        public List<Person> FindPeopleAndRelations(int? gender, string name, bool includeAncestors,
            int maxRecordsToDisplay)
        {
            var data = StaticCache.GetData();
            if (data == null)
                return null;

            var query = data.People.Where(p => p.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));

            var personOfInterest = query.FirstOrDefault();

            if (personOfInterest == null)
                return null;

            var results = BuildUpList(personOfInterest, maxRecordsToDisplay, gender, includeAncestors);

            if (results == null)
                return null;

            foreach (var person in results)
            {
                person.BirthPlace = StaticCache.GetPlaceById(person.Place_Id);
            }

            return results;

        }

        private static bool DoesResultListHaveCapacity(List<Person> ancestorList, int maxRecordsToDisplay, int? gender)
        {
            var relevantItemCount = ancestorList.Count;

            if (gender.HasValue)
                relevantItemCount =
                    ancestorList.Count(
                        p => p.Gender.Equals((Enums.Gender)gender.Value == Enums.Gender.Male ? "M" : "F"));

            return relevantItemCount < maxRecordsToDisplay;
        }

        private List<Person> BuildUpList(Person person, int maxRecordsToDisplay, int? gender, bool findAncestors)
        {
            var relationsList = new List<Person>();
            var listToQuery = new List<Person>() {person};

            while (DoesResultListHaveCapacity(relationsList, maxRecordsToDisplay, gender) && listToQuery.Count > 0)
            {
                var directRelationList = GetDirectRelations(peopleInTier: listToQuery, findParents: findAncestors);

                foreach (var relation in directRelationList)
                {
                    if (DoesResultListHaveCapacity(relationsList, maxRecordsToDisplay, gender))
                        relationsList.Add(relation);
                    else
                        break;
                }

                listToQuery = directRelationList;
            }

            return gender.HasValue
                ? relationsList.Where(p => p.Gender.Equals((Enums.Gender)gender.Value == Enums.Gender.Male ? "M" : "F"))
                    .ToList()
                : relationsList;
        }

        private List<Person> GetDirectRelations(List<Person> peopleInTier, bool findParents)
        {
            List<Person> relationsList = new List<Person>();
            foreach (var person in peopleInTier)
            {
                var directRelations = findParents ? GetDirectParents(person) : GetDirectChildren(person);
                relationsList.AddRange(directRelations);
            }

            return relationsList;
        }

        private List<Person> GetDirectChildren(Person person)
        {
            var query = StaticCache.GetData().People;
            return query.Where(p => p.Mother_Id == person.Id || p.Father_Id == person.Id).Select(p => p).ToList();
        }

        private List<Person> GetDirectParents(Person person)
        {
            var listOfParents = new List<Person>();

            if (!person.Mother_Id.HasValue && !person.Father_Id.HasValue)
                return listOfParents;

            if (person.Mother_Id.HasValue)
            {
                var mother = StaticCache.GetPersonById(person.Mother_Id.Value);
                if (mother != null)
                {
                    listOfParents.Add(mother);
                }
            }

            if (person.Father_Id.HasValue)
            {
                var father = StaticCache.GetPersonById(person.Father_Id.Value);
                if (father != null)
                {
                    listOfParents.Add(father);
                }
            }

            return listOfParents;
        }
    }
}

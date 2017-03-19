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

            List<Person> results = null;
            switch (includeAncestors)
            {
                case true:
                    results = BuildUpAncestorList(personOfInterest, maxRecordsToDisplay, gender);
                    break;
                case false:
                    results = BuildUpDescendantList(personOfInterest, maxRecordsToDisplay, gender);
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

        private static bool DoesResultListHaveCapacity(List<Person> ancestorList, int maxRecordsToDisplay, int? gender)
        {
            var relevantItemCount = ancestorList.Count;

            if (gender.HasValue)
                relevantItemCount =
                    ancestorList.Count(
                        p => p.Gender.Equals((Enums.Gender)gender.Value == Enums.Gender.Male ? "M" : "F"));

            return relevantItemCount < maxRecordsToDisplay;
        }

        

        #region GetAncestors

        private List<Person> BuildUpAncestorList(Person person, int maxRecordsToDisplay, int? gender)
        {
            List<Person> ancestorList = new List<Person>();
            List<Person> listToQuery = new List<Person>() { person };

            while (DoesResultListHaveCapacity(ancestorList, maxRecordsToDisplay, gender) && listToQuery.Count > 0)
            {
                List<Person> parentList = GetDirectParents(listToQuery);

                foreach (var parent in parentList)
                {
                    if (DoesResultListHaveCapacity(ancestorList, maxRecordsToDisplay, gender))
                        ancestorList.Add(parent);
                    else
                        break;
                }

                listToQuery = parentList;
            }

            return gender.HasValue
                ? ancestorList.Where(p => p.Gender.Equals((Enums.Gender)gender.Value == Enums.Gender.Male ? "M" : "F"))
                    .ToList()
                : ancestorList;
        }

        private List<Person> GetDirectParents(List<Person> peopleInTier)
        {
            List<Person> parentList = new List<Person>();
            foreach (var person in peopleInTier)
            {
                var directParents = GetDirectParents(person);
                parentList.AddRange(directParents);
            }

            return parentList;
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


        #endregion

        #region GetDescendants

        private List<Person> BuildUpDescendantList(Person person, int maxRecordsToDisplay, int? gender)
        {
            List<Person> descendantList = new List<Person>();
            List<Person> listToQuery = new List<Person>() { person };

            while (DoesResultListHaveCapacity(descendantList, maxRecordsToDisplay, gender) && listToQuery.Count > 0)
            {
                List<Person> childList = GetDirectChildren(listToQuery);

                foreach (var child in childList)
                {
                    if (DoesResultListHaveCapacity(descendantList, maxRecordsToDisplay, gender))
                        descendantList.Add(child);
                    else
                        break;
                }

                listToQuery = childList;
            }

            return gender.HasValue
                ? descendantList.Where(p => p.Gender.Equals((Enums.Gender)gender.Value == Enums.Gender.Male ? "M" : "F"))
                    .ToList()
                : descendantList;
        }

        private List<Person> GetDirectChildren(List<Person> peopleInTier)
        {
            List<Person> childrenList = new List<Person>();
            foreach (var person in peopleInTier)
            {
                var directChildren = GetDirectChildren(person);
                childrenList.AddRange(directChildren);
            }

            return childrenList;
        }

        private List<Person> GetDirectChildren(Person person)
        {
            var query = StaticCache.GetData().People;
            return query.Where(p => p.Mother_Id == person.Id || p.Father_Id == person.Id).Select(p => p).ToList();
        }

        #endregion
    }
}

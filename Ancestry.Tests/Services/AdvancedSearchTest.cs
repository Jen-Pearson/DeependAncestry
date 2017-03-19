using System.Collections.Generic;
using Ancestry.Business.Common;
using Ancestry.Business.Models;
using Ancestry.Business.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ancestry.Tests.Services
{
    [TestClass]
    public class AdvancedSearchTest
    {
        [TestInitialize]
        public void SetupData()
        {
            var sampleData = new Data();

            var samplePlace = new Place
            {
                Id = 1,
                Name = "Testville"
            };

            sampleData.Places = new List<Place> { samplePlace };
            List<Person> samplePeople = new List<Person>();
            var samplePerson = new Person
            {
                Id = 1,
                Gender = "F",
                Level = 1,
                Name = "Mollie Smith",
                Place_Id = 1
            };
            samplePeople.Add(samplePerson);

            var sampleChild = new Person
            {
                Id=2,
                Gender = "M",
                Level =2,
                Name ="Billy Kid",
                Place_Id = 1,
                Mother_Id = 1
            };

            samplePeople.Add(sampleChild);
            sampleData.People = samplePeople;

            StaticCache.LoadStaticData(sampleData);
        }

        [TestMethod]
        public void AdvancedSearch_PartialName_ReturnsNoResults()
        {
            bool isAncestorSearch = true;
            var service = new SearchService();
            var results = service.FindPeopleAndRelations(null, "Mollie", isAncestorSearch, 10);

            Assert.IsNull(results);
        }

        [TestMethod]
        public void AdvancedSearch_AncestorSearchWithNoAncestors_ReturnsNoResults()
        {
            bool isAncestorSearch = true;
            var service = new SearchService();
            var results = service.FindPeopleAndRelations(null, "Mollie Smith", isAncestorSearch, 10);

            Assert.AreEqual(results.Count, 0);
        }

        [TestMethod]
        public void AdvancedSearch_AncestorSearchWithAncestor_ReturnsOneResult()
        {
            bool isAncestorSearch = true;
            var service = new SearchService();
            var results = service.FindPeopleAndRelations(null, "Billy Kid", isAncestorSearch, 10);

            Assert.AreEqual(results.Count, 1);
            Assert.AreEqual(results[0].Name,"Mollie Smith");
        }

        [TestMethod]
        public void AdvancedSearch_DescendantSearchWithDescendant_ReturnsOneResult()
        {
            bool isAncestorSearch = false;
            var service = new SearchService();
            var results = service.FindPeopleAndRelations(null, "Mollie Smith", isAncestorSearch, 10);

            Assert.AreEqual(results.Count, 1);
            Assert.AreEqual(results[0].Name, "Billy Kid");
        }

        [TestMethod]
        public void AdvancedSearch_DescendantSearchWithNoDescendant_ReturnsOneResult()
        {
            bool isAncestorSearch = false;
            var service = new SearchService();
            var results = service.FindPeopleAndRelations(null, "Billy Kid", isAncestorSearch, 10);

            Assert.AreEqual(results.Count, 0);
        }
    }
}

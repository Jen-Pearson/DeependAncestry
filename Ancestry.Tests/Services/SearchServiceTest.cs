using System.Collections.Generic;
using Ancestry.Business.Common;
using Ancestry.Business.Models;
using Ancestry.Business.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ancestry.Tests.Services
{
    [TestClass]
    public class SearchServiceTest
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

            var samplePerson = new Person
            {
                Id = 1,
                Gender = "F",
                Level = 1,
                Name = "Mollie Smith",
                Place_Id = 1
            };
            sampleData.People = new List<Person> {samplePerson};

            StaticCache.LoadStaticData(sampleData);
        }

        [TestMethod]
        public void BasicSearch_NameSearch_ResultFound()
        {
            var service = new SearchService();
            var results = service.FindPeople(null, "Mollie");

            Assert.IsNotNull(results);
        }

        [TestMethod]
        public void BasicSearch_NameSearch_PlaceNameMapped()
        {
            var service = new SearchService();
            var results = service.FindPeople(null, "Mollie");

            Assert.AreEqual(results[0].BirthPlace, "Testville");
        }

        [TestMethod]
        public void BasicSearch_SearchWithGender_NoResultFound()
        {
            var service = new SearchService();
            var gender = 0; // test male
            var results = service.FindPeople(gender, "Mollie");

            Assert.IsNotNull(results);
            Assert.AreEqual(results.Count, 0);
        }

        [TestMethod]
        public void BasicSearch_SearchWithCorrectGender_ResultFound()
        {
            var service = new SearchService();
            var gender = 1; // test female
            var results = service.FindPeople(gender, "Mollie");

            Assert.IsNotNull(results);
            Assert.AreEqual(results.Count, 1);
        }

        [TestMethod]
        public void BasicSearch_PartialNameLowerCase_ResultFound()
        {
            var service = new SearchService();
            var results = service.FindPeople(null, "mollie");

            Assert.IsNotNull(results);
            Assert.AreEqual(results.Count, 1);
        }

       
    }
}

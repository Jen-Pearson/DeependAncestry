using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ancestry.Business.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ancestry.Tests.Services
{
    [TestClass]
    public class SearchServiceTest
    {

        [TestMethod]
        public void TestNameSearch()
        {
            var service = new SearchService();
            var results = service.FindPeople(null, "Mollie");

            Assert.IsNotNull(results);
        }

        [TestMethod]
        public void TestSearchNotFound()
        {
            var service = new SearchService();
            var results = service.FindPeople(0, "Mollie");

            Assert.IsNotNull(results);
            Assert.AreEqual(results.Count, 0);
        }

        [TestMethod]
        public void TestSearchNameCaseInsensitive()
        {
            var service = new SearchService();
            var results = service.FindPeople(null, "mollie");

            Assert.IsNotNull(results);
            Assert.AreEqual(results.Count, 1);
        }

        [TestMethod]
        public void TestSearchManyResults()
        {
            var service = new SearchService();
            var results = service.FindPeople(null, "e");

            Assert.IsNotNull(results);
            Assert.AreNotEqual(results.Count, 0);
        }
    }
}

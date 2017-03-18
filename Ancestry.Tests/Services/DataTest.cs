using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ancestry.Business;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ancestry.Tests.Services
{
    [TestClass]
    public class DataTest
    {
        [TestMethod]
        public void TestDataLoad()
        {
            var service = new DataService();
            var data = service.ReadFile();
            Assert.IsNotNull(data);
        }
    }
}

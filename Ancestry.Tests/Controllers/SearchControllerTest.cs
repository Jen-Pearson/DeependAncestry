using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Ancestry.Business;
using Ancestry.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ancestry.Tests.Controllers
{
    [TestClass]
    public class SearchControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            SearchController controller = new SearchController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}

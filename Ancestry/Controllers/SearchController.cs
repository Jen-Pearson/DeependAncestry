using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ancestry.Business;
using Ancestry.Business.Services;
using Ancestry.Models;

namespace Ancestry.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        public ActionResult Index()
        {
            var model = new SearchViewModel();
            model.GenderList = InitialiseGenders(null);
            return View(model);
        }

        private static List<SelectListItem> InitialiseGenders(int[] selectedGenders)
        {

            if (selectedGenders == null)
                selectedGenders = new int[] {};

            var genderList = new List<SelectListItem>
            {
                new SelectListItem() {Text = "Male", Value = "0", Selected = selectedGenders.Contains(0) },
                new SelectListItem() {Text = "Female", Value = "1", Selected = selectedGenders.Contains(1)}
            };

            return genderList;
        }

        [HttpPost]
        public ActionResult Index(SearchViewModel model, int[] selectedGenders)
        {
            model.GenderList = InitialiseGenders(selectedGenders);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            int? genderToSearch = null;
            if (selectedGenders != null && selectedGenders.Length == 1)
                genderToSearch = selectedGenders[0];

            var searchService = new SearchService();
            var results = searchService.FindPeople(genderToSearch, model.Name);
            var displayResults = new List<Result>();

            foreach (var result in results)
            {
                displayResults.Add(new Result(result.Id, result.Name, result.Gender, result.BirthPlace));
            }

            model.Results = displayResults;
            return View(model);
        }

        public ActionResult Advanced()
        {
            return View();
        }
    }
}
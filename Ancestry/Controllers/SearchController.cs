using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ancestry.Business;
using Ancestry.Business.Models;
using Ancestry.Business.Services;
using Ancestry.Models;
using PagedList;

namespace Ancestry.Controllers
{
    public class SearchController : Controller
    {
        [HttpGet]
        public ActionResult Index(SearchViewModel model)
        {
            if (DetermineIfInitialPageLoad(model))
            {
                ModelState.Clear();
                model.GenderList = InitialiseGenders(null);
                return View(model);
            }

            var genders = model.SelectedGenders== null ? new List<int>() : model.SelectedGenders.Split(',').ToList().ConvertAll(c => Convert.ToInt32(c));
            return PerformSearch(model,genders.ToArray() , model.Page);
        }

        private static bool DetermineIfInitialPageLoad(SearchViewModel model)
        {
            return model.Name == null;
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
        public ActionResult Index(SearchViewModel model, int[] selectedGenders, int? page)
        {
            return PerformSearch(model, selectedGenders, page);
        }

        private ActionResult PerformSearch(SearchViewModel model, int[] selectedGenders, int? page)
        {

            model.GenderList = InitialiseGenders(selectedGenders);
            ModelState.Remove("SelectedGenders");
            if (!ModelState.IsValid)
                return View(model);

            var genderToSearch = DetermineGenderToSearch(selectedGenders);

            var pageNumber = page ?? 1;

            var searchService = new SearchService();
            var results = searchService.FindPeople(genderToSearch, model.Name);

            model.Results = FormatResultsForDisplay(results, pageNumber, 10);
            return View(model);
        }

        private static int? DetermineGenderToSearch(int[] selectedGenders)
        {
            int? genderToSearch = null;

            if (selectedGenders != null && selectedGenders.Count() == 1)
                genderToSearch = Convert.ToInt32(selectedGenders[0]);
            return genderToSearch;
        }

        private static IPagedList<Result> FormatResultsForDisplay(List<Person> results, int pageNumber, int pageSize)
        {
            var displayResults = new List<Result>();

            foreach (var result in results)
            {
                displayResults.Add(new Result(result.Id, result.Name, result.Gender, result.BirthPlace));
            }

            var onePageOfResults = displayResults.ToPagedList<Result>(pageNumber, pageSize);
            return onePageOfResults;
        }

        public ActionResult Advanced()
        {
            var model = new SearchViewModel {GenderList = InitialiseGenders(null)};
            return View(model);
        }

        [HttpPost]
        public ActionResult Advanced(SearchViewModel model, int[] selectedGenders)
        {
            model.GenderList = InitialiseGenders(selectedGenders);
            ModelState.Remove("SelectedGenders");
            if (!ModelState.IsValid)
                return View(model);

            var genderToSearch = DetermineGenderToSearch(selectedGenders);
            var searchService = new SearchService();
            var results = searchService.FindPeople(genderToSearch, model.Name);

            model.Results = FormatResultsForDisplay(results, 1, 10); // todo: clean this up for max of 10 records, no paging
            
            return View(model);
        }

       
    }
}
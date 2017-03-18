using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ancestry.Business;
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

            var genders = model.SelectedGenders.Split(',').ToList().ConvertAll(c => Convert.ToInt32(c));
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
            {
                var error = ModelState.Values
                    .SelectMany(v => v.Errors);
                    
                
                return View(model);
            }

            int? genderToSearch = null;
            
            if (selectedGenders != null && selectedGenders.Count() == 1)
                genderToSearch = Convert.ToInt32(selectedGenders[0]);

            var pageNumber = page ?? 1;

            var searchService = new SearchService();
            var results = searchService.FindPeople(genderToSearch, model.Name);
            var displayResults = new List<Result>();

            foreach (var result in results)
            {
                displayResults.Add(new Result(result.Id, result.Name, result.Gender, result.BirthPlace));
            }

            var onePageOfResults = displayResults.ToPagedList<Result>(pageNumber, 10);
            model.Results = onePageOfResults;
            return View(model);
        }

        public ActionResult Advanced()
        {
            return View();
        }
    }
}
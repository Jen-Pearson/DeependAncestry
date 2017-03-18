using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace Ancestry.Models
{
    public class SearchViewModel
    {
        [Required(ErrorMessage = "Please enter a name to search on")]
        public string Name { get; set; }

        public enum Gender
        {
            Male = 0,
            Female = 1
        }

        public IList<SelectListItem> GenderList { get; set; }

        public string SelectedGenders { get; set; }

        public IPagedList<Result> Results { get; set; }

        public int? Page { get; set; }

        public bool UseAncestorDirection { get; set; }
    }

    public class Result
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string Birthplace { get; set; }

        public Result(int id, string fullName, string gender, string birthPlace)
        {
            Id = id;
            FullName = fullName;
            switch (gender.ToUpper())
            {
                case "M":
                    Gender = "Male";
                    break;
                case "F":
                    Gender = "Female";
                    break;
            }

            Birthplace = birthPlace;
        }

    }

}
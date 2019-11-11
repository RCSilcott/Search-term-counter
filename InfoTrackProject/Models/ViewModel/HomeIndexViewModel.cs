using InfoTrackProject.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InfoTrackProject.Models.ViewModel
{
	public class HomeIndexViewModel
	{

		[Display(Name = "Search Engine")]
		public SelectList SearchEngines { get; set; }
		public string SearchResults { get; set; }

		[Display(Name = "Number of searches on the page")]
		public int NumOfSearchesOnPage { get; set; }

		[Display(Name = "Search Term")]
		public string SearchTerm { get; set; }

		[Display(Name = "Search Query")]
		public string SearchQuery { get; set; }

		public SearchEngine  SearchEngine { get; set; }
	}
}
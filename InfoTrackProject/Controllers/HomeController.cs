using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Xml.XPath;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using InfoTrackProject.Models.Enums;
using InfoTrackProject.Models.ViewModel;
using InfoTrackProject.Models;

namespace InfoTrackProject.Controllers
{
	public class HomeController : Controller
	{
		public Array searchEngines { get; }

		public HomeController()
		{
			searchEngines = Enum.GetValues(typeof(SearchEngine));
		}		

		public async Task<ActionResult> Index(string searchTerm = null, int numOfSearches = 100, string searchQuery = "", SearchEngine searchEngine = SearchEngine.Google)
		{
			try
			{
			var result = await SearchTermTracker.ReturnResult(searchEngine, numOfSearches, searchTerm, searchQuery);		

			var viewModel = new HomeIndexViewModel()
			{
				SearchEngines = new SelectList(searchEngines),
				NumOfSearchesOnPage = numOfSearches,
				SearchTerm = searchTerm,
				SearchQuery = searchQuery,
				SearchResults = result.TrimEnd(','),
				 SearchEngine = searchEngine
			};
			
			return View(viewModel);
			}
			catch
			{
				return View("Error");
			}
		}		

		
	}
}
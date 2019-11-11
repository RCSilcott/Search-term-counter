using HtmlAgilityPack;
using InfoTrackProject.Models.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace InfoTrackProject.Models
{
	public static class SearchTermTracker
	{
		public async static Task<string> ReturnResult(SearchEngine searchEngine, int numOfSearches, string searchTerm, string searchQuery)
		{
			if(searchEngine == SearchEngine.Bing)
				return await searchBing(numOfSearches, searchTerm, searchQuery) ;

			if (searchEngine == SearchEngine.Google)
				return await searchGoogle(numOfSearches, searchTerm, searchQuery);

			if (searchEngine == SearchEngine.Yahoo)
				return await searchYahoo(numOfSearches, searchTerm, searchQuery);

			return "";
		}

		async static Task<string> searchBing(int numOfSearches, string searchTerm, string searchQuery)
		{		

			var urlString = $"https://www.bing.com/search?q={searchQuery}&count={numOfSearches}";

			var htmlDoc = await CreateHtmlDoc(urlString);
			
			var searchResultsList = htmlDoc.DocumentNode.Descendants().Where(x => (x.Name == "li" && x.OuterHtml.Contains("b_algo")) || (x.Name == "li" && x.InnerHtml.Contains("b_restorableLink")));


			if (NoOfTimesSearchTermIsFound(searchResultsList, searchTerm) == 0)
				return "";

			return ShowWhereSearchTermIsFound(searchResultsList, searchTerm) + Environment.NewLine + "Search term was found " + NoOfTimesSearchTermIsFound(searchResultsList, searchTerm) + " times";
		}

		async static Task<string> searchGoogle(int numOfSearches, string searchTerm, string searchQuery)
		{

			var urlSearchQuery = searchQuery.Trim().Replace(' ','+'); //might need this if url doesn't edit the searchquery spaces

			var searchResultClass = "class=\"ZINbbc xpd O9g5cc uUPGi\"";

			var urlString = $"https://www.google.co.uk/search?num={numOfSearches}&q={searchQuery}";

			var htmlDoc = await CreateHtmlDoc(urlString);

			var searchResultsList = htmlDoc.DocumentNode.Descendants().Where(x=>x.Name=="div" && x.InnerHtml.Contains(searchResultClass));

			if (NoOfTimesSearchTermIsFound(searchResultsList, searchTerm) == 0)
				return "";

			return ShowWhereSearchTermIsFound(searchResultsList, searchTerm) + Environment.NewLine + "Search term was found " + NoOfTimesSearchTermIsFound(searchResultsList, searchTerm) + " times";
		}	

		async static Task<string> searchYahoo(int numOfSearches, string searchTerm, string searchQuery)
		{
			var urlSearchQuery = searchQuery.Trim().Replace(' ', '+');
			
			var urlString = $"https://uk.search.yahoo.com/search?p={urlSearchQuery}&n={numOfSearches}";

			var htmlDoc = await CreateHtmlDoc(urlString);
			
			var searchResultsList = htmlDoc.DocumentNode.Descendants().Where(x => x.Name == "li" && (x.ParentNode.InnerHtml.Contains("dd algo") || x.InnerHtml.Contains("dd ads")));

			if (NoOfTimesSearchTermIsFound(searchResultsList, searchTerm) == 0)
				return "";

			return ShowWhereSearchTermIsFound(searchResultsList, searchTerm) + Environment.NewLine + "Search term was found " + NoOfTimesSearchTermIsFound(searchResultsList, searchTerm) + " times";
		}

		async static Task<HtmlDocument> CreateHtmlDoc(string urlString)
		{
			HttpClient http = new HttpClient();
			var response = await http.GetByteArrayAsync(urlString);
			String source = Encoding.GetEncoding("utf-8").GetString(response, 0, response.Length - 1);
			source = WebUtility.HtmlDecode(source);
			HtmlDocument resultat = new HtmlDocument();
			resultat.LoadHtml(source);

			return resultat;			
		}

		static string ShowWhereSearchTermIsFound(IEnumerable<HtmlNode> searchResultsList, string searchTerm )
		{
			var myString = "";

			for (var node = 1; node < searchResultsList.Count(); node++)
			{
				if (searchResultsList.ElementAt(node).InnerText.Contains(searchTerm))
				{
					myString += node + ",";
				}
			}

			return myString;
		}

		static int NoOfTimesSearchTermIsFound(IEnumerable<HtmlNode> searchResultsList, string searchTerm)
		{
			int count = 0;

			for (var node = 1; node < searchResultsList.Count(); node++)
			{
				if (searchResultsList.ElementAt(node).InnerText.Contains(searchTerm))
				{
					count++;
				}
			}

			return count;
		}

	}
	
}
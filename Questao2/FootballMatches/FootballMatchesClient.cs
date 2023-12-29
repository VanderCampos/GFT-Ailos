using System.Net.Http.Json;

namespace Questao2
{
	public class FootballMatchesClient
	{
		private const string ADDRESS = "https://jsonmock.hackerrank.com/api/football_matches";

		public FootballMatchesClient()
		{
		}

		private void FetchAll(string url, ref FootballMatchesResponse teamMatches)
		{
			int page = 1;
			int totalPages;

			using (HttpClient client = new())
				do
				{
					var matches = client.GetFromJsonAsync<FootballMatchesResponse>(url + $"&page={page}").Result;
					totalPages = matches.total_pages;
					teamMatches.data.AddRange(matches.data);
					page++;
				}
				while (page <= totalPages);
		}

		public FootballMatchesResponse FetchAll(string team, int year)
		{
			FootballMatchesResponse teamMatches = new FootballMatchesResponse()
			{
				page = 1,
				total_pages = 1
			};

			var url = ADDRESS + $"?year={year}";

			//Casa
			FetchAll(url + $"&team1={team}", ref teamMatches);
			//Visitante
			FetchAll(url + $"&team2={team}", ref teamMatches);

			teamMatches.total = teamMatches.data.Count();
			teamMatches.per_page = teamMatches.total;

			return teamMatches;
		}


	}
}

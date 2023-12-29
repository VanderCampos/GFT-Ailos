using Newtonsoft.Json;
using Questao2;

public class Program
{
	public static void Main()
	{
		string teamName = "Paris Saint-Germain";
		int year = 2013;
		int totalGoals = getTotalScoredGoals(teamName, year);

		Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

		teamName = "Chelsea";
		year = 2014;
		totalGoals = getTotalScoredGoals(teamName, year);

		Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

		// Output expected:
		// Team Paris Saint - Germain scored 109 goals in 2013
		// Team Chelsea scored 92 goals in 2014
	}

	public static int getTotalScoredGoals(string team, int year)
	{
		try
		{
			var footballMatchesClient = new FootballMatchesClient()
				.FetchAll(team, year);

			var totalGoalsHome = footballMatchesClient.data
				.Where(g => g.Team1 == team)
				.Sum(t => t.Team1goals);

			var totalGoalsAway = footballMatchesClient.data
				.Where(g => g.Team2 == team)
				.Sum(t => t.Team2goals);

			return totalGoalsHome + totalGoalsAway;

		}
		catch (Exception ex)
		{
			Console.WriteLine($"-------------------------");
			Console.WriteLine($"Team:{team} - Year{year}");
			Console.WriteLine($"Erro:{ex.Message}");
			return -1;
		}
	}

}
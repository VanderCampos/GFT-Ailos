namespace Questao2
{
	public class FootballMatchesResponse
	{
		public int page { get; set; }
		public int per_page { get; set; }
		public int total { get; set; }
		public int total_pages { get; set; }
		public List<FootballMatchesData> data { get; set; }

		public FootballMatchesResponse()
		{
			data = new List<FootballMatchesData>();
		}

	}
}

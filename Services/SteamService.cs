namespace steam_compare_backend.Services
{
	public class SteamService
	{
		public string? SteamKey = Environment.GetEnvironmentVariable( smSteamKey );

		private static readonly string smSteamKey = "STEAM_KEY";
	}
}
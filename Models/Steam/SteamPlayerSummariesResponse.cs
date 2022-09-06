namespace steam_compare_backend.Models.Steam
{
	public class SteamPlayerSummariesResponse
	{
		public SteamPlayersWrapper Response { get; set; }
	}

	public class SteamPlayersWrapper
	{
		public SteamPlayer[] Players { get; set; }
	}

	public class SteamPlayer
	{
		public string SteamId { get; set; }
		public string PersonaName { get; set; }
		public string ProfileUrl { get; set; }
		public string Avatar { get; set; }
		public string AvatarMedium { get; set; }
		public string AvatarFull { get; set; }
		public PersonaState PersonaState { get; set; }
		public SteamGame[]? Games { get; set; }
	}

	public enum PersonaState
	{
		OFFLINE,
		ONLINE,
		BUSY,
		AWAY,
		SNOOZE,
		LOOKING_TO_TRADE,
		LOOKING_TO_PLAY
	}
}
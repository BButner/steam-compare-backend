namespace steam_compare_backend.Api
{
	public static class SteamApiEndpoints
	{
		public static readonly string ISteamUserGetFriendsList =
			"http://api.steampowered.com/ISteamUser/GetFriendList/v0001";

		public static readonly string ISteamUserGetPlayerSummaries =
			"http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002";

		public static readonly string IPlayerServiceGetOwnedGames =
			"http://api.steampowered.com/IPlayerService/GetOwnedGames/v0001";

		public static readonly string ISteamUserGetVanityUrl =
			"http://api.steampowered.com/ISteamUser/ResolveVanityURL/v0001";
	}
}
using System.Text.Json.Serialization;

namespace steam_compare_backend.Models.Steam
{
	public class SteamPlayerGamesResponse
	{
		public SteamGames Response { get; set; }
	}

	public class SteamGames
	{
		[JsonPropertyName( "game_count" )]
		public int GameCount { get; set; }

		public SteamGame[] Games { get; set; }
	}

	public class SteamGame
	{
		[JsonPropertyName( "appid" )]
		public long AppId { get; set; }

		public string Name { get; set; }

		[JsonPropertyName( "playtime_forever" )]
		public int PlaytimeForever { get; set; }

		[JsonPropertyName( "playtime_2weeks" )]
		public int Playtime2Weeks { get; set; }

		[JsonPropertyName( "img_icon_url" )]
		public string ImageIconUrl { get; set; }

		[JsonPropertyName( "img_logo_url" )]
		public string ImageLogoUrl { get; set; }
	}
}
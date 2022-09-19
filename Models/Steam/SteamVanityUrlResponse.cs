namespace steam_compare_backend.Models.Steam
{
	public class SteamVanityUrlResponseWrapper
	{
		public SteamVanityUrlResponse Response { get; set; }
	}

	public class SteamVanityUrlResponse
	{
		public string SteamId { get; set; }
		public SteamVanityUrlResponseResult Success { get; set; }
	}

	public enum SteamVanityUrlResponseResult
	{
		Success = 1,
		NoMatch = 42,
		Invalid = 43
	}
}
namespace steam_compare_backend.Models.Steam
{
	public class SteamUserFriendsListResponse
	{
		public SteamFriendsWrapper FriendsList { get; set; }
	}

	public class SteamFriendsWrapper
	{
		public SteamFriend[] Friends { get; set; }
	}

	public class SteamFriend
	{
		public string SteamId { get; set; }

		public string Relationship { get; set; }

		public long FriendSince { get; set; }
	}
}
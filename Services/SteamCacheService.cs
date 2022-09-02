using Microsoft.Extensions.Caching.Memory;

using steam_compare_backend.Models.Steam;

namespace steam_compare_backend.Services
{
	public class SteamCacheService
	{
		public SteamCacheService( IMemoryCache memoryCache ) =>
			_memoryCache = memoryCache;

		public SteamUserFriendsListResponse? TryGetSteamFriendsFromCacheBySteamId( string steamId )
		{
			_memoryCache.TryGetValue( steamId, out SteamUserFriendsListResponse? steamUserFriendsListResponse );

			return steamUserFriendsListResponse;
		}

		public void SetSteamFriendsToCacheBySteamId( string steamId,
			SteamUserFriendsListResponse steamUserFriendsListResponse )
		{
			var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration( TimeSpan.FromHours( 1 ) );

			_memoryCache.Set( steamId, steamUserFriendsListResponse, cacheEntryOptions );
		}

		private IMemoryCache _memoryCache;
	}
}
using Microsoft.Extensions.Caching.Memory;

using steam_compare_backend.Models.Steam;

namespace steam_compare_backend.Services
{
	public class SteamCacheService
	{
		public SteamCacheService( IMemoryCache memoryCache ) =>
			_memoryCache = memoryCache;

		public SteamPlayer? TryGetSteamPlayerFromCache( string steamId )
		{
			_memoryCache.TryGetValue( steamId, out SteamPlayer steamPlayer );

			return steamPlayer;
		}

		public SteamPlayer[] TryGetSteamPlayersFromCache( string[] steamIds )
		{
			var steamPlayers = new List<SteamPlayer>();

			foreach( var steamId in steamIds )
			{
				var player = TryGetSteamPlayerFromCache( steamId );
				if( player is not null ) steamPlayers.Add( player );
			}

			return steamPlayers.ToArray();
		}

		public void SetSteamPlayerToCache( SteamPlayer steamPlayer )
		{
			var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration( TimeSpan.FromHours( 1 ) );

			_memoryCache.Set( steamPlayer.SteamId, steamPlayer, cacheEntryOptions );
		}

		public void SetSteamPlayersToCache( SteamPlayer[] players )
		{
			foreach( var steamPlayer in players )
			{
				var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration( TimeSpan.FromHours( 1 ) );

				_memoryCache.Set( steamPlayer.SteamId, steamPlayer, cacheEntryOptions );
			}
		}

		public void SetSteamGamesToCache( string steamId, SteamGame[] games )
		{
			var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration( TimeSpan.FromHours( 1 ) );

			_memoryCache.Set( steamId + "_games", games, cacheEntryOptions );
		}

		public SteamGame[]? TryGetSteamGamesFromCache( string steamId )
		{
			_memoryCache.TryGetValue( steamId + "_games", out SteamGame[] games );

			return games;
		}

		public void SetSteamVanityUrlToCache( string vanityUrl, string steamId )
		{
			var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration( TimeSpan.FromHours( 1 ) );
			_memoryCache.Set( vanityUrl.ToLower(), steamId, cacheEntryOptions );
		}

		public string? TryGetSteamIdFromVanityUrlCache( string vanityUrl )
		{
			_memoryCache.TryGetValue( vanityUrl.ToLower(), out string steamId );

			return steamId;
		}

		private IMemoryCache _memoryCache;
	}
}
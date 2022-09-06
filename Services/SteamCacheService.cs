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

		private IMemoryCache _memoryCache;
	}
}
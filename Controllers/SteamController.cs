using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

using steam_compare_backend.Api;
using steam_compare_backend.Models.Steam;
using steam_compare_backend.Services;

namespace steam_compare_backend.Controllers
{
	[ApiController]
	[Route( "[controller]" )]
	public class SteamController
	{
		public SteamController( SteamService service,
			IHttpClientFactory httpClientFactory,
			SteamCacheService steamCacheService )
		{
			_steamService = service;
			_steamCacheService = steamCacheService;
			_httpClientFactory = httpClientFactory;
		}

		[HttpGet( "/user/{steamId}" )]
		public async Task<IActionResult> GetUserBySteamId( [FromRoute] string steamId,
			[FromQuery] bool loadGames = true )
		{
			var user = ( await SteamApi.GetPlayerSummaries( _httpClientFactory, _steamService, new[] { steamId } ) )
				.FirstOrDefault();

			if( user is null )
			{
				return new NotFoundResult();
			}

			return new OkObjectResult( user );
		}

		[HttpGet( "/user/{steamId}/games" )]
		public async Task<IActionResult> GetGamesBySteamId( [FromRoute] string steamId )
		{
			var gamesFromCache = _steamCacheService.TryGetSteamGamesFromCache( steamId );

			if( gamesFromCache is not null )
			{
				return new OkObjectResult( gamesFromCache );
			}

			try
			{
				var games = await SteamApi.GetOwnedGames( _httpClientFactory, _steamService, steamId );

				if( games is not null )
				{
					_steamCacheService.SetSteamGamesToCache( steamId, games );
				}
				else
				{
					_steamCacheService.SetSteamGamesToCache( steamId, Array.Empty<SteamGame>() );
				}

				return new OkObjectResult( games );
			}
			catch( Exception e ) { }

			return new NotFoundResult();
		}

		// TODO: Handle errors (401, 500, etc)
		[HttpGet( "/user/{steamId}/friends" )]
		public async Task<IActionResult> GetFriendsListBySteamId( [FromRoute] string steamId )
		{
			var friends = await SteamApi.GetSteamFriends( _httpClientFactory, _steamService, steamId );

			if( friends is null )
			{
				return new NotFoundResult();
			}

			var friendSteamIds = friends.FriendsList.Friends.Select( friend => friend.SteamId ).ToList();

			var summariesFromCache =
				_steamCacheService.TryGetSteamPlayersFromCache( friendSteamIds.ToArray() ).ToList();

			var idsNotInCache = friendSteamIds
				.Except( summariesFromCache.Select( summary => summary.SteamId ).ToArray() ).ToArray();

			var summaries = await SteamApi.GetPlayerSummaries( _httpClientFactory, _steamService, idsNotInCache );

			if( summaries is not null )
			{
				_steamCacheService.SetSteamPlayersToCache( summaries );
			}

			var allSummaries = summariesFromCache.Concat( summaries ?? Array.Empty<SteamPlayer>() ).ToList();

			foreach( var steamPlayer in allSummaries )
			{
				var gamesFromCache = _steamCacheService.TryGetSteamGamesFromCache( steamPlayer.SteamId );

				if( gamesFromCache is not null )
				{
					steamPlayer.Games = gamesFromCache;
					continue;
				}

				try
				{
					var games = await SteamApi.GetOwnedGames( _httpClientFactory, _steamService, steamPlayer.SteamId );
					if( games is not null )
					{
						_steamCacheService.SetSteamGamesToCache( steamPlayer.SteamId, games );
					}
					else
					{
						_steamCacheService.SetSteamGamesToCache( steamPlayer.SteamId, Array.Empty<SteamGame>() );
					}

					steamPlayer.Games = games;
				}
				catch( Exception e ) { }
			}

			return new OkObjectResult( allSummaries );
		}

		private SteamService _steamService;
		private IHttpClientFactory _httpClientFactory;
		private SteamCacheService _steamCacheService;
	}
}
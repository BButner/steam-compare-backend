using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

using steam_compare_backend.Api;
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
			_SteamCacheService = steamCacheService;
			_httpClientFactory = httpClientFactory;
		}

		[HttpGet( "/user/{steamId}" )]
		public async Task<IActionResult> GetUserBySteamId( [FromRoute] string steamId )
		{
			var user = await SteamApi.GetPlayerSummaries( _httpClientFactory, _steamService, new[] { steamId } );

			// todo handle cache, have to parse each steam id and verify against it, then only retrieve what we need, to build the response
			// this is singular, but we can share the same cache functionality for the bulk call

			if( user is null )
			{
				return new NotFoundResult();
			}

			return new OkObjectResult( user.First() );
		}

		[HttpGet( "/player/summaries" )]
		public async Task<IActionResult> GetPlayerSummaries( [FromQuery, Required] string[] steamIds )
		{
			var summaries = await SteamApi.GetPlayerSummaries( _httpClientFactory, _steamService, steamIds );

			return new OkObjectResult( summaries );
		}

		// TODO: Handle errors (401, 500, etc)
		[HttpGet( "/user/{steamId}/friends" )]
		public async Task<IActionResult> GetFriendsListBySteamId( [FromRoute] string steamId )
		{
			var friendsCache = _SteamCacheService.TryGetSteamFriendsFromCacheBySteamId( steamId );

			if( friendsCache is not null )
			{
				Console.WriteLine( "Returning from Cache!" );
				return new OkObjectResult( friendsCache );
			}

			var friends = await SteamApi.GetSteamFriends( _httpClientFactory, _steamService, steamId );

			if( friends is not null ) _SteamCacheService.SetSteamFriendsToCacheBySteamId( steamId, friends );

			return new OkObjectResult( friends );
		}

		private SteamService _steamService;
		private IHttpClientFactory _httpClientFactory;
		private SteamCacheService _SteamCacheService;
	}
}
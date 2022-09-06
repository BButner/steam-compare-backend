using Microsoft.AspNetCore.WebUtilities;

using steam_compare_backend.Models.Steam;
using steam_compare_backend.Services;

namespace steam_compare_backend.Api
{
	public static class SteamApi
	{
		public static async Task<SteamUserFriendsListResponse?> GetSteamFriends(
			IHttpClientFactory factory,
			SteamService steamService,
			string steamId
		)
		{
			var client = factory.CreateClient();

			var dict = new Dictionary<string, string?> { { "key", steamService.SteamKey }, { "steamid", steamId } };

			var requestMessage = new HttpRequestMessage(
				HttpMethod.Get,
				SteamApiEndpoints.ISteamUserGetFriendsList );

			requestMessage.RequestUri =
				new Uri( QueryHelpers.AddQueryString( requestMessage.RequestUri.ToString(), dict ) );

			var response = await client.SendAsync( requestMessage );

			if( response.IsSuccessStatusCode )
			{
				try
				{
					var friends = await response.Content.ReadFromJsonAsync<SteamUserFriendsListResponse>();

					return friends;
					// var test = await response.Content.ReadAsStringAsync();
				}
				catch( Exception ex )
				{
					Console.WriteLine( ex.Message );
				}
			}

			return null;
		}

		public static async Task<SteamPlayer[]?> GetPlayerSummaries(
			IHttpClientFactory factory,
			SteamService steamService,
			string[] steamIds
		)
		{
			var client = factory.CreateClient();

			var dict = new Dictionary<string, string?>
			{
				{ "key", steamService.SteamKey }, { "steamids", string.Join( ',', steamIds ) }
			};

			var requestMessage = new HttpRequestMessage(
				HttpMethod.Get,
				SteamApiEndpoints.ISteamUserGetPlayerSummaries );

			requestMessage.RequestUri =
				new Uri( QueryHelpers.AddQueryString( requestMessage.RequestUri.ToString(), dict ) );

			var response = await client.SendAsync( requestMessage );

			if( response.IsSuccessStatusCode )
			{
				try
				{
					var players = await response.Content.ReadFromJsonAsync<SteamPlayerSummariesResponse>();

					return players.Response.Players;
				}
				catch( Exception ex )
				{
					Console.WriteLine( ex.Message );
				}
			}
			else
			{
				Console.WriteLine( response.ReasonPhrase );
			}

			return null;
		}

		public static async Task<SteamGame[]?> GetOwnedGames(
			IHttpClientFactory factory,
			SteamService steamService,
			string steamId
		)
		{
			var client = factory.CreateClient();

			var dict = new Dictionary<string, string?>
			{
				{ "key", steamService.SteamKey }, { "steamid", steamId }, { "include_appinfo", "true" }
			};

			var requestMessage = new HttpRequestMessage(
				HttpMethod.Get,
				SteamApiEndpoints.IPlayerServiceGetOwnedGames );

			requestMessage.RequestUri =
				new Uri( QueryHelpers.AddQueryString( requestMessage.RequestUri.ToString(), dict ) );

			var response = await client.SendAsync( requestMessage );

			if( response.IsSuccessStatusCode )
			{
				try
				{
					var games = await response.Content.ReadFromJsonAsync<SteamPlayerGamesResponse>();

					return games?.Response.Games;
					// var test = await response.Content.ReadAsStringAsync();
				}
				catch( Exception ex )
				{
					Console.WriteLine( ex.Message );
				}
			}

			return null;
		}
	}
}
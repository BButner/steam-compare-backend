using Microsoft.AspNetCore.HostFiltering;

using steam_compare_backend.Services;

var builder = WebApplication.CreateBuilder( args );

if( builder.Environment.IsProduction() )
{
	builder.Services.AddCors( options =>
	{
		options.AddDefaultPolicy(
			policy =>
			{
				policy.WithOrigins( "https://steamcompare.games",
					"https://www.steamcompare.games" );
			} );
	} );
}

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<SteamService>();
builder.Services.AddSingleton<SteamCacheService>();
builder.Services.AddHttpClient();

var hosts = builder.Configuration["AllowedHosts"]?
	.Split( new[] { ';' }, StringSplitOptions.RemoveEmptyEntries );

if( hosts?.Length > 0 )
{
	builder.Services.Configure<HostFilteringOptions>(
		options => options.AllowedHosts = hosts
	);
}

if( builder.Environment.IsProduction() )
{
	builder.Services.AddLettuceEncrypt();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if( app.Environment.IsDevelopment() )
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

if( app.Environment.IsProduction() )
{
	app.UseCors();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
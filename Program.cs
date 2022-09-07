using steam_compare_backend.Services;

var builder = WebApplication.CreateBuilder( args );
var origins = "_myAllowSpecificOrigins";

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<SteamService>();
builder.Services.AddSingleton<SteamCacheService>();
builder.Services.AddHttpClient();

if( builder.Environment.IsProduction() )
{
	builder.Services.AddLettuceEncrypt();

	builder.Services.AddCors( options =>
	{
		options.AddPolicy( name: origins,
			policy =>
			{
				policy.WithOrigins( "https://steamcompare.games",
					"https://www.steamcompare.games" );
			} );
	} );
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
	app.UseCors( origins );
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
using BezoekersRegistratieSysteemBL.Managers;
using BezoekersRegistratieSysteemDL;
using BezoekersRegistratieSysteemREST.Repo;

const string ENV_SQL_CONNECTION = "SQL_CONNECTION_STRING";

var builder = WebApplication.CreateBuilder(args);

// Het instellen van omgevingsvariabelen voor de connection string
builder.Host.ConfigureAppConfiguration((hostingContext, configuration) => {
	// Zorg dat je deze user secret instelt voordat je de applicatie start
	// zie https://dotnetcoretutorials.com/2022/04/28/using-user-secrets-configuration-in-net/ voor
	// meer informatie
	configuration.AddUserSecrets("SQL_CONNECTIONSTRING");
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string? connectionstring = Environment.GetEnvironmentVariable(ENV_SQL_CONNECTION);

if (connectionstring is null) {
	Console.WriteLine($"Omgevingsvariabele {ENV_SQL_CONNECTION} is niet ingesteld");
	return;
}

// Alle managers als singleton toevoegen
// dit omdat de API interract met de managers
// WAARCHUWING: DE REPOS ZIJN TIJDELIJK, MOETEN VERVANGEN WORDEN DOOR DB
var manager = new BedrijfRepoADO(connectionstring);
BedrijfManager bedrijfManager = new(manager);
AfspraakManager afspraakManager = new(new AfspraakRepoADO(connectionstring));
WerknemerManager werknemerManager = new(new WerknemerRepoADO(connectionstring), manager);

builder.Services.AddSingleton(bedrijfManager);
builder.Services.AddSingleton(afspraakManager);
builder.Services.AddSingleton(werknemerManager);

// Wij hebben liever lowercase URLs voor onze Aapie
builder.Services.AddRouting(options => options.LowercaseUrls = true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();

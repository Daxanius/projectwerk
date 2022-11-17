using BezoekersRegistratieSysteemBL.Managers;
using BezoekersRegistratieSysteemDL.ADOMS;

const string ENV_SQL_CONNECTION = "BRS_CONNECTION_STRING";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Pak de connectionstring van de omgevingsvariablen of de user secrets
string? connectionstring = builder.Configuration[ENV_SQL_CONNECTION] ?? Environment.GetEnvironmentVariable(ENV_SQL_CONNECTION);

if (connectionstring is null) {
	Console.WriteLine($"{ENV_SQL_CONNECTION} is niet ingesteld");
	return;
}

// Weer een Microsoft quirk...
connectionstring = connectionstring.Replace("\\\\", "\\");

// Alle managers als singleton toevoegen
// dit omdat de API interract met de managers
// WAARCHUWING: DE REPOS ZIJN TIJDELIJK, MOETEN VERVANGEN WORDEN DOOR DB
BedrijfManager bedrijfManager = new(new BedrijfRepoADOMS(connectionstring));
AfspraakManager afspraakManager = new(new AfspraakRepoADOMS(connectionstring));
WerknemerManager werknemerManager = new(new WerknemerRepoADOMS(connectionstring));

builder.Services.AddSingleton(bedrijfManager);
builder.Services.AddSingleton(afspraakManager);
builder.Services.AddSingleton(werknemerManager);

// Wij hebben liever lowercase URLs voor onze Aapie
builder.Services.AddRouting(options => options.LowercaseUrls = true);

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment()) {
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseAuthorization();
app.UseHttpLogging();
app.MapControllers();

app.Run();
using BezoekersRegistratieSysteemBL.Managers;
using BezoekersRegistratieSysteemDL.ADO;

const string ENV_SQL_CONNECTION = "BRS_CONNECTION_STRING";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Pak de connectionstring van de omgevingsvariablen of de user secrets
string? connectionstring = Environment.GetEnvironmentVariable(ENV_SQL_CONNECTION) ?? builder.Configuration[ENV_SQL_CONNECTION];

if (connectionstring is null) {
	Console.WriteLine($"{ENV_SQL_CONNECTION} is niet ingesteld");
	return;
}

// Alle managers als singleton toevoegen
// dit omdat de API interract met de managers
// WAARCHUWING: DE REPOS ZIJN TIJDELIJK, MOETEN VERVANGEN WORDEN DOOR DB
BedrijfManager bedrijfManager = new(new BedrijfRepoADO(connectionstring));
AfspraakManager afspraakManager = new(new AfspraakRepoADO(connectionstring));
WerknemerManager werknemerManager = new(new WerknemerRepoADO(connectionstring));

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
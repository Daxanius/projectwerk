using BezoekersRegistratieSysteemBL.Interfaces;
using BezoekersRegistratieSysteemBL.Managers;
using BezoekersRegistratieSysteemDL.ADOMS;
using BezoekersRegistratieSysteemDL.ADOMySQL;

const string ENV_DB = "BRS_DATABASE";
const string ENV_SQL_CONNECTION = "BRS_CONNECTION_STRING";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Database technologie om te gebruiken
string database = (builder.Configuration[ENV_DB] ?? "mysql").Trim().ToLower();

// Pak de connectionstring van de omgevingsvariablen of de user secrets
string? connectionstring = builder.Configuration[ENV_SQL_CONNECTION] ?? Environment.GetEnvironmentVariable(ENV_SQL_CONNECTION);

if (connectionstring is null) {
	Console.WriteLine($"{ENV_SQL_CONNECTION} is niet ingesteld");
	return;
}

// Weer een Microsoft quirk...
connectionstring = connectionstring.Replace("\\\\", "\\");

// Lege repos declareren
IAfspraakRepository afspraakRepo;
IBedrijfRepository bedrijfRepo;
IWerknemerRepository werknemerRepo;
IParkingContractRepository parkingContractRepo;
IParkeerplaatsRepository parkeerplaatsRepo;

// Dit zorgt ervoor dat we een database technologie kunnen kiezen
// bij het opstarten van onze service.
switch (database) {
	case "azure":
	case "express":
	case "msserver":
	case "mssql": {
			afspraakRepo = new AfspraakRepoADOMS(connectionstring);
			bedrijfRepo = new BedrijfRepoADOMS(connectionstring);
			werknemerRepo = new WerknemerRepoADOMS(connectionstring);
			parkingContractRepo = new ParkingContractADOMS(connectionstring);
			parkeerplaatsRepo = new ParkeerPlaatsADOMS(connectionstring);
			break;
		}
	case "mysql": {
			afspraakRepo = new AfspraakRepoADOMySQL(connectionstring);
			bedrijfRepo = new BedrijfRepoADOMySQL(connectionstring);
			werknemerRepo = new WerknemerRepoADOMySQL(connectionstring);
			parkingContractRepo = new ParkingContractADOMySQL(connectionstring);
			parkeerplaatsRepo = new ParkeerPlaatsADOMySQL(connectionstring);;
			break;
		}
	default: {
			Console.WriteLine($"Implementatie niet gevonden voor: \"{database}\"");
			Console.WriteLine($"U kunt een implementatie selecteren door \"{ENV_DB}\" te specifieren in uw appsettings");
			return;
		}
}

// Alle managers als singleton toevoegen
// dit omdat de API interract met de managers
BedrijfManager bedrijfManager = new(bedrijfRepo, afspraakRepo);
AfspraakManager afspraakManager = new(afspraakRepo);
WerknemerManager werknemerManager = new(werknemerRepo, afspraakRepo);
ParkingContractManager parkingContractManager = new(parkingContractRepo);
ParkeerplaatsManager parkeerplaatsManager = new(parkeerplaatsRepo, parkingContractRepo);

builder.Services.AddSingleton(bedrijfManager);
builder.Services.AddSingleton(afspraakManager);
builder.Services.AddSingleton(werknemerManager);
builder.Services.AddSingleton(parkingContractManager);
builder.Services.AddSingleton(parkeerplaatsManager);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.UseHttpLogging();
app.MapControllers();

app.Run();
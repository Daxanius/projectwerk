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

// Dit zorgt ervoor dat we een database technologie kunnen kiezen
// bij het opstarten van onze service.
switch (database) {
	case "azure":
	case "express":
	case "msserver":
	case "mssql": {
			// Alle managers als singleton toevoegen
			// dit omdat de API interract met de managers
			IAfspraakRepository afspraakRepo = new AfspraakRepoADO(connectionstring);
			IParkingContractRepository parkingContractRepo = new ParkingContractADO(connectionstring);

			BedrijfManager bedrijfManager = new(new BedrijfRepoADO(connectionstring), afspraakRepo);
			AfspraakManager afspraakManager = new(afspraakRepo);
			WerknemerManager werknemerManager = new(new WerknemerRepoADO(connectionstring), afspraakRepo);
			ParkingContractManager parkingContractManager = new(parkingContractRepo);
			ParkeerplaatsManager parkeerplaatsManager = new(new ParkeerPlaatsADO(connectionstring), parkingContractRepo);

			builder.Services.AddSingleton(bedrijfManager);
			builder.Services.AddSingleton(afspraakManager);
			builder.Services.AddSingleton(werknemerManager);
			builder.Services.AddSingleton(parkingContractManager);
			builder.Services.AddSingleton(parkeerplaatsManager);

			break;
		}
	case "mysql": {
			// Alle managers als singleton toevoegen
			// dit omdat de API interract met de managers
			IAfspraakRepository afspraakRepo = new AfspraakRepoMySQL(connectionstring);
			IParkingContractRepository parkingContractRepo = new ParkingContractMySQL(connectionstring);

			BedrijfManager bedrijfManager = new(new BedrijfRepoMySQL(connectionstring), afspraakRepo);
			AfspraakManager afspraakManager = new(afspraakRepo);
			WerknemerManager werknemerManager = new(new WerknemerRepoMySQL(connectionstring), afspraakRepo);
			ParkingContractManager parkingContractManager = new(parkingContractRepo);
			ParkeerplaatsManager parkeerplaatsManager = new(new ParkeerPlaatsMySQL(connectionstring), parkingContractRepo);

			builder.Services.AddSingleton(bedrijfManager);
			builder.Services.AddSingleton(afspraakManager);
			builder.Services.AddSingleton(werknemerManager);
			builder.Services.AddSingleton(parkingContractManager);
			builder.Services.AddSingleton(parkeerplaatsManager);

			break;
		}
	default: {
			Console.WriteLine($"Implementatie niet gevonden voor: \"{database}\"");
			Console.WriteLine($"U kunt een implementatie selecteren door \"{ENV_DB}\" te specifieren in uw appsettings");
			Environment.Exit(1);
			break;
		}
}

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.UseHttpLogging();
app.MapControllers();

app.Run();
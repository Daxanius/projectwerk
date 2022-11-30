using BezoekerRegistratieSysteemDLPicker;
using BezoekersRegistratieSysteemBL.Interfaces;
using BezoekersRegistratieSysteemBL.Managers;
using BezoekersRegistratieSysteemDL.ADOMS;
using BezoekersRegistratieSysteemDL.ADOMySQL;

const string ENV_DB = "BRS_DATABASE";
//const string ENV_SQL_CONNECTION = "BRS_CONNECTION_STRING_ADO";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Database technologie om te gebruiken (ADO/MYSQL)
string database = (builder.Configuration[ENV_DB] ?? "mysql").Trim().ToUpper();
// Houd rekening met DB type nu (ADO/MYSQL)
string ENV_SQL_CONNECTION = $"BRS_CONNECTION_STRING_{database}";
// Pak de connectionstring van de omgevingsvariablen of de user secrets
string? connectionstring = builder.Configuration[ENV_SQL_CONNECTION] ?? Environment.GetEnvironmentVariable(ENV_SQL_CONNECTION);

if (connectionstring is null) {
	Console.WriteLine($"{ENV_SQL_CONNECTION} is niet ingesteld");
	return;
}

// Weer een Microsoft quirk...
connectionstring = connectionstring.Replace("\\\\", "\\");
BezoekersRegistratieBeheerRepo repos = null;
try {
	if (!Enum.TryParse<RepoType>(database.ToUpper(), out RepoType repoType)) {
		throw new Exception("Database type not found.");
	}
	repos = DLPickerFactory.GeefRepositories(connectionstring, repoType);
} catch (Exception ex) {
	Console.WriteLine($"{ex.Message}");
    Environment.Exit(1);
	return;
}

BedrijfManager bedrijfManager = new(repos.bedrijfRepository, repos.afspraakrepository);
AfspraakManager afspraakManager = new(repos.afspraakrepository);
WerknemerManager werknemerManager = new(repos.werknemerRepository, repos.afspraakrepository);
ParkingContractManager parkingContractManager = new(repos.parkingContractRepository);
ParkeerplaatsManager parkeerplaatsManager = new(repos.parkeerplaatsRepository, repos.parkingContractRepository);


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
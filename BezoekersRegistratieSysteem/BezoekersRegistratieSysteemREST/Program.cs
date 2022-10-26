using BezoekersRegistratieSysteemBL.Managers;
using BezoekersRegistratieSysteemREST.Repo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Alle managers als singleton toevoegen
// dit omdat de API interract met de managers
// WAARCHUWING: DE REPOS ZIJN TIJDELIJK, MOETEN VERVANGEN WORDEN DOOR DB
var manager = new BedrijfRepo();
BedrijfManager bedrijfManager = new(manager);
AfspraakManager afspraakManager = new(new AfspraakRepo());
BezoekerManager bezoekerManager = new(new BezoekerRepo());
WerknemerManager werknemerManager = new(new WerknemerRepo(), manager);

builder.Services.AddSingleton(bedrijfManager);
builder.Services.AddSingleton(afspraakManager);
builder.Services.AddSingleton(bezoekerManager);
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

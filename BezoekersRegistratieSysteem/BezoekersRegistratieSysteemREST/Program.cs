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
builder.Services.AddSingleton<BedrijfsManager>(new BedrijfsManager(new BedrijfRepo()));
builder.Services.AddSingleton<AfspraakManager>(new AfspraakManager(new AfspraakRepo()));
builder.Services.AddSingleton<BezoekerManager>(new BezoekerManager(new BezoekerRepo()));
builder.Services.AddSingleton<WerknemerManager>(new WerknemerManager(new WerknemerRepo()));

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

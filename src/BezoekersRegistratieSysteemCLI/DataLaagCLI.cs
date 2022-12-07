using BezoekerRegistratieSysteemDLPicker;
using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Managers;
using Newtonsoft.Json;

namespace BezoekersRegistratieSysteemCLI {
	public class DataLaagCLI {
		private const string databaseType = "MYSQL";
		private const string sqlServerHost = $"BRS_CONNECTION_STRING_{databaseType}";
		static void Main() {
			//We zitten momenteel aan VoegAfspraakToe()
			if (sqlServerHost == "" || databaseType == "") {
				Console.WriteLine("sqlServerHost en database moeten ingevult zijn.");
				return;
			}
			string connectionString = Environment.GetEnvironmentVariable(sqlServerHost);
			connectionString = connectionString.Replace("\\\\", "\\");

			BezoekersRegistratieBeheerRepo repos = null;
			if (!Enum.TryParse<RepoType>(databaseType.ToUpper(), out RepoType repoType)) {
				Console.WriteLine("Databse type niet geldig.");
				return;
			}
			repos = DLPickerFactory.GeefRepositories(connectionString, repoType);


			AfspraakManager afspraakRepo = new AfspraakManager(repos.afspraakrepository);
			BedrijfManager bedrijfRepo = new BedrijfManager(repos.bedrijfRepository, repos.afspraakrepository);
			WerknemerManager werknemerRepo = new WerknemerManager(repos.werknemerRepository, repos.afspraakrepository);
			ParkeerplaatsManager parkeerRepo = new ParkeerplaatsManager(repos.parkeerplaatsRepository, repos.parkingContractRepository);
			ParkingContractManager parkingRepo = new ParkingContractManager(repos.parkingContractRepository);

			Bedrijf bestaandBedrijf = new Bedrijf(1, "allphi", "BE0838576480", true, "093961130", "info@allphi.be", "Guldensporenpark 24 9820 merelbeke");
			Werknemer bestaandWerknemer = new Werknemer(38, "MArcella", "Lawrence");
			bestaandWerknemer.VoegBedrijfEnFunctieToeAanWerknemer(bestaandBedrijf, "MarcellaLawrence@allphi.be", "Logistiek");
			object result;


            //try {
            //    var aresult = parkeerRepo.GeefWeekoverzichtParkingVoorBedrijf(bestaandBedrijf);
            //    Print(aresult.GeefXwaardeGeparkeerdenTotaalPerWeek(), "VoegAfspraakToe");
            //} catch (Exception ex) {
            //    Error(ex);
            //    return;
            //}


            //try {
            //    var aresult = parkeerRepo.GeefUuroverzichtParkingVoorBedrijf(bestaandBedrijf);
            //    Print(aresult.GeefXwaardeCheckInsPerUur(), "VoegAfspraakToe");
            //    Print(aresult.GeefXwaardeGeparkeerdenTotaalPerUur(), "VoegAfspraakToe");
            //} catch (Exception ex) {
            //    Error(ex);
            //    return;
            //}




            #region AfspraakRepo


            #region VoegAfspraakToe()
            //try {
            //	result = afspraakRepo.VoegAfspraakToe(new Afspraak(DateTime.Now, bestaandBedrijf, new Bezoeker("Dool", "Mans", "DoolMans@gmail.com", "Dool BVBA"), bestaandWerknemer));
            //	Print(result, "VoegAfspraakToe");
            //} catch (Exception ex) {
            //	Error(ex);
            //	return;
            //}
            #endregion

            #region GeefHuidigeAfsprakenPerBedrijf(long bedrijfId)
            //try
            //{
            //	long bedrijfId = 1;
            //	result = afspraakRepo.GeefHuidigeAfsprakenPerBedrijf(bedrijfId);
            //	Print(result, "GeefHuidigeAfsprakenPerBedrijf(bedrijfId)");
            //} catch (Exception ex) {
            //Error(ex);
            //return;
            //}
            #endregion

            #region VoegAfspraakToe(Afspraak afspraak)
            //try
            //{
            //Afspraak afspraak = new(DateTime.Now, new Bezoeker("Stan", "Persoons", "stan2@gmail.com", "Hogent"), new Werknemer(1, "Jan", "Cornelis"));
            //	result = afspraakRepo.VoegAfspraakToe(afspraak);
            //	Print(result, "VoegAfspraakToe(Afspraak afspraak)");
            //} catch (Exception ex) {
            //Error(ex);
            //return;
            //}
            #endregion

            #region GeefHuidigeAfsprakenPerWerknemer(long werknemerId)
            //try
            //{
            //	long werknemerId = 1;
            //	result = afspraakRepo.GeefHuidigeAfsprakenPerWerknemer(werknemerId);
            //	Print(result, "GeefHuidigeAfsprakenPerWerknemer(long werknemerId)");
            //} catch (Exception ex) {
            //Error(ex);
            //return;
            //}
            //#endregion

            #endregion

            #region BestaatAfspraak(Afspraak afspraak)
            //try
            //{
            //	Afspraak afspraak = new(DateTime.Parse("2022-10-28T17:11:38.25"), new Bezoeker("Bart", "De Pauw", "Diddler@Outlook.com", "Diddler NV"), new Werknemer(1, "Jan", "Cornelis"));
            //	result = afspraakRepo.BestaatAfspraak(afspraak);
            //	Print($"BestaatAfspraak: {result}", "BestaatAfspraak(Afspraak afspraak)");
            //} catch (Exception ex) {
            //Error(ex);
            //return;
            //}
            #endregion

            #region BestaatAfspraak(long afspraakid)
            //try {
            //	long afspraakId = 1;
            //	result = afspraakRepo.BestaatAfspraak(afspraakId);
            //	Print($"BestaatAfspraak: {result}", "BestaatAfspraak(long afspraakid)");
            //} catch (Exception ex) {
            //Error(ex);
            //return;
            //}
            #endregion

            #region BeeindigAfspraakBezoeker(long afspraakId)
            //try {
            //	long afspraakId = 1;
            //	Print(afspraakRepo.GeefHuidigeAfspraken(), "afspraakRepo.GeefHuidigeAfspraken()");
            //	afspraakRepo.BeeindigAfspraakBezoeker(afspraakId);
            //	Print("Afspraak met id 1 verwijderd", "BeeindigAfspraakBezoeker(long afspraakid)");
            //	Print(afspraakRepo.GeefHuidigeAfspraken(), "afspraakRepo.GeefHuidigeAfspraken()");
            //} catch (Exception ex) {
            //Error(ex);
            //return;
            //}
            #endregion

            #region BeeindigAfspraakSysteem(long afspraakId)
            //try {
            //	long afspraakId = 1;
            //	Print(afspraakRepo.GeefHuidigeAfspraken(), "afspraakRepo.GeefHuidigeAfspraken()");
            //	afspraakRepo.BeeindigAfspraakBezoeker(afspraakId);
            //	Print("Afspraak met id 1 beeindigd", "BeeindigAfspraakSysteem(long afspraakid)");
            //	Print(afspraakRepo.GeefHuidigeAfspraken(), "afspraakRepo.GeefHuidigeAfspraken()");
            //} catch (Exception ex) {
            //Error(ex);
            //return;
            //}
            #endregion

            #region VerwijderAfspraak(long afspraakId)
            //try {
            //	long afspraakId = 1;
            //	Print(afspraakRepo.GeefHuidigeAfspraken(), "afspraakRepo.GeefHuidigeAfspraken()");
            //	afspraakRepo.VerwijderAfspraak(afspraakId);
            //	Print("Afspraak met id 1 verwijderd", "VerwijderAfspraak(long afspraakid)");
            //	Print(afspraakRepo.GeefHuidigeAfspraken(), "afspraakRepo.GeefHuidigeAfspraken()");
            //} catch (Exception ex) {
            //Error(ex);
            //return;
            //}
            #endregion

            #region BewerkAfspraak(Afspraak afspraak)
            //try {
            //	Afspraak afspraak = afspraakRepo.GeefHuidigeAfspraken().First();
            //	Print(afspraakRepo.GeefHuidigeAfspraken(), "afspraakRepo.GeefHuidigeAfspraken()");
            //	afspraak.ZetStarttijd(DateTime.Now);
            //	afspraakRepo.BewerkAfspraak(afspraak);
            //	Print($"Afspraak met id {afspraak.Id} is veranderd", "BewerkAfspraak(Afspraak afspraak)");
            //	Print(afspraakRepo.GeefHuidigeAfspraken(), "afspraakRepo.GeefHuidigeAfspraken()");
            //} catch (Exception ex) {
            //Error(ex);
            //return;
            //}
            #endregion

            #region GeefAfspraak(long afspraakId)
            //try {
            //	Afspraak afspraak = afspraakRepo.GeefAfspraak(1);
            //	Print(afspraak, "GeefAfspraak(long afspraakId)");
            //} catch (Exception ex) {
            //	Error(ex);
            //	return;
            //}
            #endregion

            #region GeefAfsprakenPerDag(DateTime datum)
            //try {
            //	result = afspraakRepo.GeefAfsprakenPerDag(DateTime.Now);
            //	Print(result, "GeefAfsprakenPerDag(DateTime datum)");
            //} catch (Exception ex) {
            //	Error(ex);
            //	return;
            //}
            #endregion

            #region GeefAlleAfsprakenPerWerknemer(long werknemerId)
            //try {
            //	result = afspraakRepo.GeefAlleAfsprakenPerWerknemer(1);
            //	Print(result, "GeefAlleAfsprakenPerWerknemer(long werknemerId)");
            //} catch (Exception ex) {
            //	Error(ex);
            //	return;
            //}
            #endregion

            #region GeefAfsprakenPerWerknemerOpDag(long werknemerId, DateTime datum)
            //try {
            //	DateTime date = DateTime.Now;
            //	Afspraak afspraak = new(date, new Bezoeker("Stans", "Persoon", "stan4@gmail.com", "Hogend"), new Werknemer(1, "Jan", "Cornelis"));
            //	afspraakRepo.VoegAfspraakToe(afspraak);
            //	result = afspraakRepo.GeefAfsprakenPerWerknemerOpDag(1, DateTime.Parse(date.ToString("d")));
            //	Print(result, "GeefAfsprakenPerWerknemerOpDag(long werknemerId, DateTime datum)");
            //} catch (Exception ex) {
            //	Error(ex);
            //	return;
            //}
            #endregion

            #endregion

            #region BedrijfRepo

            #region BestaatBedrijf(bedrijf)
            //try {
            //	Bedrijf bedrijf = new Bedrijf("Hogent", "BE0676747521", "0476687215", "stan@gmail.com", "kerkstraat 164");
            //	bedrijf = bedrijfRepo.VoegBedrijfToe(bedrijf);
            //	result = bedrijfRepo.BestaatBedrijf(bedrijf);
            //	Print($"Bestaat bedrijf {bedrijf.Naam}:  {result}", "BestaatBedrijf(bedrijf)");
            //} catch (Exception ex) {
            //	Error(ex);
            //	return;
            //}
            #endregion

            #region BestaatBedrijf(bedrijfId)
            //try {
            //	Bedrijf bedrijf = bedrijfRepo.Geefbedrijven().First();
            //	result = bedrijfRepo.BestaatBedrijf(bedrijf.Id);
            //	Print($"Bestaat bedrijf met id {bedrijf.Id}:  {result}", "BestaatBedrijf(bedrijfId)");
            //} catch (Exception ex) {
            //	Error(ex);
            //	return;
            //}
            #endregion

            #region BestaatBedrijf(bedrijfsnaam)
            //try {
            //	Bedrijf bedrijf = bedrijfRepo.Geefbedrijven().First();
            //	result = bedrijfRepo.BestaatBedrijf(bedrijf.Naam);
            //	Print($"Bestaat bedrijf met naam {bedrijf.Naam}:  {result}", "BestaatBedrijf(bedrijfsnaam)");
            //} catch (Exception ex) {
            //	Error(ex);
            //	return;
            //}
            #endregion

            #region BewerkBedrijf(bedrijf)
            //try {
            //	Bedrijf bedrijf = bedrijfRepo.Geefbedrijven().First();
            //	bedrijf.ZetEmail("stan123@gmail.com");
            //	bedrijf.ZetAdres("JoKerkBin 1");
            //	bedrijf.ZetNaam("HooGent");
            //	bedrijf.ZetTelefoonNummer("04123456789");
            //	bedrijfRepo.BewerkBedrijf(bedrijf);
            //	result = bedrijfRepo.GeefBedrijf(bedrijf.Id);
            //	Print(result, "BewerkBedrijf(bedrijf)");
            //} catch (Exception ex) {
            //	Error(ex);
            //	return;
            //}
            #endregion

            #region GeefBedrijf(id)
            //try {
            //	result = bedrijfRepo.GeefBedrijf(1);
            //	Print(result, "GeefBedrijf(id)");
            //} catch (Exception ex) {
            //	Error(ex);
            //	return;
            //}
            #endregion

            #region GeefBedrijf(bedrijfsnaam)
            //try {
            //	result = bedrijfRepo.GeefBedrijf("Hogent");
            //	Print(result, "GeefBedrijf(bedrijfsnaam)");
            //} catch (Exception ex) {
            //	Error(ex);
            //	return;
            //}
            #endregion

            #region Geefbedrijven()
            //try {
            //	result = bedrijfRepo.Geefbedrijven();
            //	Print(result, "Geefbedrijven()");
            //} catch (Exception ex) {
            //	Error(ex);
            //	return;
            //}
            #endregion

            #region VerwijderBedrijf(bedrijfId)
            //try {
            //	Bedrijf bedrijf = bedrijfRepo.Geefbedrijven().First();
            //	bedrijfRepo.VerwijderBedrijf(bedrijf.Id);
            //	Print($"Bedrijf met id: {bedrijf.Id} heeft nu status: Verwijderd", "VerwijderBedrijf(bedrijfId)");
            //} catch (Exception ex) {
            //	Error(ex);
            //	return;
            //}
            #endregion

            #region VoegBedrijfToe(bedrijfId)
            //try {
            //	Bedrijf bedrijf = new Bedrijf("Hogent", "BE0676747521", "0476687215", "stan@gmail.com", "kerkstraat 164");
            //	result = bedrijfRepo.VoegBedrijfToe(bedrijf);
            //	Print(result, "VoegBedrijfToe(bedrijfId)");
            //} catch (Exception ex) {
            //	Error(ex);
            //	return;
            //}
            #endregion

            #endregion

            #region WerknemerRepo

            #region BestaatWerknemer(Werknemer werknemer)
            //try {
            //	Werknemer werknemer = werknemerRepo.GeefWerknemer(1);
            //	Print($"Bestaat werknemer: {werknemerRepo.BestaatWerknemer(werknemer)}", "BestaatWerknemer(Werknemer werknemer)");
            //} catch (Exception ex) {
            //	Error(ex);
            //	return;
            //}
            #endregion

            #region BestaatWerknemer(long id)
            //try {
            //	Werknemer werknemer = werknemerRepo.GeefWerknemer(1);
            //	Print($"Bestaat werknemer met id = {werknemer.Id}: {werknemerRepo.BestaatWerknemer(werknemer.Id)}", "BestaatWerknemer(long id)");
            //} catch (Exception ex) {
            //	Error(ex);
            //	return;
            //}
            #endregion

            #region GeefWerknemer(long werknemerId)
            //try {
            //	long werknemerId = 1;
            //  result = werknemerRepo.GeefWerknemer(werknemerId);
            //	Print(result, "GeefWerknemer(long werknemerId)");
            //} catch (Exception ex) {
            //	Error(ex);
            //	return;
            //}
            #endregion

            #region GeefWerknemersOpNaam(string voornaam, string achternaam)
            //try {
            //	string voornaam = "Joris";
            //	string achternaam = "Conelis";
            //	result = werknemerRepo.GeefWerknemersOpNaam(voornaam, achternaam);
            //	Print(result, "GeefWerknemersOpNaam(string voornaam, string achternaam)");
            //} catch (Exception ex) {
            //	Error(ex);
            //	return;
            //}
            #endregion

            #region GeefWerknemersPerBedrijf(long bedrijfId)
            //try {
            //	long bedrijfId = 2;
            //	result = werknemerRepo.GeefWerknemersPerBedrijf(bedrijfId);
            //	Print(result, "GeefWerknemersPerBedrijf(long bedrijfId)");
            //} catch (Exception ex) {
            //	Error(ex);
            //	return;
            //}
            #endregion

            #region VerwijderWerknemer(Werknemer werknemer, Bedrijf bedrijf)
            //try {
            //	Werknemer werknemer = new Werknemer("Stan", "Persoons");
            //	Bedrijf bedrijf = new Bedrijf("Hogent", "BE0676747521", "0476687215", "stan@gmail.com", "kerkstraat 164");

            //	bedrijfRepo.VoegBedrijfToe(bedrijf);

            //	werknemer.VoegBedrijfEnFunctieToeAanWerknemer(bedrijf, "stan@gmail.com", "CFO");
            //	werknemerRepo.VoegWerknemerToe(werknemer);

            //	werknemerRepo.VerwijderWerknemer(werknemer, bedrijf);

            //	Print($"Werknemer met id = {werknemer.Id} is verwijderd", "VerwijderWerknemer(Werknemer werknemer, Bedrijf bedrijf)");
            //} catch (Exception ex) {
            //	Error(ex);
            //	return;
            //}
            #endregion

            #region VerwijderWerknemerFunctie(Werknemer werknemer, Bedrijf bedrijf, string functie)
            //try {
            //	Werknemer werknemer = new Werknemer("Stan1", "Persoons1");
            //	Bedrijf bedrijf = new Bedrijf("Hogent", "BE0676747521", "0476687215", "stan@gmail.com", "kerkstraat 164");

            //	bedrijfRepo.VoegBedrijfToe(bedrijf);

            //	werknemer.VoegBedrijfEnFunctieToeAanWerknemer(bedrijf, "stan@gmail.com", "CFO");
            //	werknemer.VoegBedrijfEnFunctieToeAanWerknemer(bedrijf, "stan@gmail.com", "CEO");
            //	werknemerRepo.VoegWerknemerToe(werknemer);

            //	werknemerRepo.VerwijderWerknemerFunctie(werknemer, bedrijf, "CEO");

            //	Print($"Werknemer met functie = CEO is verwijderd", "VerwijderWerknemerFunctie(Werknemer werknemer, Bedrijf bedrijf, string functie)");
            //} catch (Exception ex) {
            //	Error(ex);
            //	return;
            //}
            #endregion

            #region VoegWerknemerToe(Werknemer werknemer)
            //try {
            //	string voornaam = "Weude";
            //	string achternaam = "Balder";
            //	Werknemer werknemer = new Werknemer(voornaam, achternaam);
            //	result = werknemerRepo.VoegWerknemerToe(werknemer);
            //	Print(result, "VoegWerknemerToe(Werknemer werknemer)");
            //} catch (Exception ex) {
            //	Error(ex);
            //	return;
            //}
            #endregion

            #region WijzigWerknemer(Werknemer werknemer, Bedrijf bedrijf)
            //try {
            //	string voornaam = "Weude";
            //	string achternaam = "Balder";
            //	Werknemer werknemer = new Werknemer(voornaam, achternaam);
            //	Bedrijf bedrijf = new Bedrijf("Hogent", "BE0676747521", "0476687215", "stan@gmail.com", "kerkstraat 164");

            //	bedrijfRepo.VoegBedrijfToe(bedrijf);
            //	werknemer.VoegBedrijfEnFunctieToeAanWerknemer(bedrijf, "stan@gmail.com", "CFO");
            //	werknemerRepo.VoegWerknemerToe(werknemer);

            //	Print(werknemerRepo.GeefWerknemer(werknemer.Id), "WijzigWerknemer(Werknemer werknemer, Bedrijf bedrijf)");

            //	werknemer.ZetVoornaam("Pardon");
            //	werknemer.ZetAchternaam("Kenan");

            //	werknemerRepo.WijzigWerknemer(werknemer, bedrijf);
            //	Print(werknemerRepo.GeefWerknemer(werknemer.Id), "WijzigWerknemer(Werknemer werknemer, Bedrijf bedrijf)");
            //} catch (Exception ex) {
            //	Error(ex);
            //	return;
            //}
            #endregion

            #endregion


            static void Print(object input, string functie) {
				string json = JsonConvert.SerializeObject(input, Formatting.Indented);
				Console.ForegroundColor = ConsoleColor.DarkCyan;
				Console.BackgroundColor = ConsoleColor.Black;
				Console.WriteLine($"<--- {functie} --->");
				Console.ResetColor();
				Console.BackgroundColor = ConsoleColor.Black;
				Console.ForegroundColor = ConsoleColor.DarkYellow;
				Console.WriteLine($"\n{json}\n");
				Console.ResetColor();
				Console.ForegroundColor = ConsoleColor.DarkCyan;
				Console.BackgroundColor = ConsoleColor.Black;
				Console.WriteLine($"<--- Einde - {functie} --->\n\n\n");
				Console.ResetColor();
			}

			static void Error(Exception ex) {
				Console.Clear();
				Console.ForegroundColor = ConsoleColor.DarkRed;
				Console.BackgroundColor = ConsoleColor.Black;
				Console.WriteLine($"\n\n\n<### Error: {ex.Message} ###>\n\n\n");
				Console.ResetColor();
				Console.ReadKey(false);
			}
		}
	}
}
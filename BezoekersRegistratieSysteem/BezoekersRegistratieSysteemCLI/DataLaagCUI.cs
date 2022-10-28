using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemDL;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace BezoekersRegistratieSysteemCLI
{
	public class DataLaagCUI
	{
		private const string sqlServerHost = "";
		private const string database = "";
		private const string connectionString = $"Data Source={sqlServerHost};Initial Catalog={database};Integrated Security=True";
		static void Main()
		{
			if (sqlServerHost == "" || database == "")
			{
				Console.WriteLine("sqlServerHost en database moeten ingevult zijn.");
				return;
			}

			object result;

			#region AfspraakRepo

			AfspraakRepoADO afspraakRepo = new(connectionString);

			#region GeefHuidigeAfspraken()
			//try
			//{
			//	result = afspraakRepo.GeefHuidigeAfspraken();
			//	Print(result, "GeefHuidigeAfspraken");
			//} catch (Exception ex)
			//{
			//	Console.WriteLine($"{ex.Message}");
			//	Console.ReadKey(false);
			//	return;
			//}
			#endregion

			#region GeefHuidigeAfsprakenPerBedrijf(long bedrijfId)
			//try
			//{
			//	long bedrijfId = 1;
			//	result = afspraakRepo.GeefHuidigeAfsprakenPerBedrijf(bedrijfId);
			//	Print(result, "GeefHuidigeAfsprakenPerBedrijf(bedrijfId)");
			//} catch (Exception ex)
			//{
			//	Console.WriteLine($"{ex.Message}");
			//	Console.ReadKey(false);
			//	return;
			//}
			#endregion

			#region VoegAfspraakToe(Afspraak afspraak)
			//try
			//{
			//Afspraak afspraak = new(DateTime.Now, new Bezoeker("Stan", "Persoons", "stan2@gmail.com", "Hogent"), new Werknemer(1, "Jan", "Cornelis"));
			//	result = afspraakRepo.VoegAfspraakToe(afspraak);
			//	Print(result, "VoegAfspraakToe(Afspraak afspraak)");
			//} catch (Exception ex)
			//{
			//	Console.WriteLine($"{ex.Message}");
			//	Console.ReadKey(false);
			//	return;
			//}
			#endregion

			#region GeefHuidigeAfsprakenPerWerknemer(long werknemerId)
			//try
			//{
			//	long werknemerId = 1;
			//	result = afspraakRepo.GeefHuidigeAfsprakenPerWerknemer(werknemerId);
			//	Print(result, "GeefHuidigeAfsprakenPerWerknemer(long werknemerId)");
			//} catch (Exception ex)
			//{
			//	Console.WriteLine($"{ex.Message}");
			//	Console.ReadKey(false);
			//	return;
			//}
			//#endregion

			#endregion

			#region BestaatAfspraak(Afspraak afspraak)
			//try
			//{
			//	Afspraak afspraak = new(DateTime.Parse("2022-10-28T17:11:38.25"), new Bezoeker("Bart", "De Pauw", "Diddler@Outlook.com", "Diddler NV"), new Werknemer(1, "Jan", "Cornelis"));
			//	result = afspraakRepo.BestaatAfspraak(afspraak);
			//	Print($"BestaatAfspraak: {result}", "BestaatAfspraak(Afspraak afspraak)");
			//} catch (Exception ex)
			//{
			//	Console.WriteLine($"{ex.Message}");
			//	Console.ReadKey(false);
			//	return;
			//}
			#endregion

			#region BestaatAfspraak(long afspraakid)
			//try
			//{
			//	long afspraakId = 1;
			//	result = afspraakRepo.BestaatAfspraak(afspraakId);
			//	Print($"BestaatAfspraak: {result}", "BestaatAfspraak(long afspraakid)");
			//} catch (Exception ex)
			//{
			//	Console.WriteLine($"{ex.Message}");
			//	Console.ReadKey(false);
			//	return;
			//}
			#endregion

			#endregion

			#region BedrijfRepo



			#endregion

			#region WerknemerRepo



			#endregion

			void Print(object input, string functie)
			{
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
		}

		//public static void BeeindigAfspraakBezoeker(long afspraakId)
		//{

		//}

		//public static void BeeindigAfspraakSysteem(long afspraakId)
		//{

		//}

		//public static void VerwijderAfspraak(long afspraakId)
		//{

		//}

		//public static void BewerkAfspraak(Afspraak afspraak)
		//{

		//}

		//public static Afspraak GeefAfspraak(long afspraakId)
		//{

		//}

		//public static IReadOnlyList<Afspraak> GeefAfsprakenPerDag(DateTime datum)
		//{

		//}

		//public static IReadOnlyList<Afspraak> GeefAlleAfsprakenPerWerknemer(long werknemerId)
		//{

		//}

		//public static IReadOnlyList<Afspraak> GeefAfsprakenPerWerknemerOpDag(long werknemerId, DateTime datum)
		//{

		//}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace BezoekersRegistratieSysteemUI.Nutsvoorzieningen {
	public static class ValideerInput {
		private static readonly Regex RegexBtw = new("^[A-Za-z]{2,4}(?=.{2,12}$)[-_\\s0-9]*(?:[a-zA-Z][-_\\s0-9]*){0,2}$");
		private static readonly Regex RegexEmail = new(@"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$", RegexOptions.IgnoreCase);
		private static readonly Regex RegexTelefoonnummer = new(@"^\+?[0-9]{1,15}$");

		#region Checkers
		public static bool IsEmailGeldig(this string? input) {
			if (input is null) return false;
			return RegexEmail.IsMatch(input);
		}

		public static bool IsNietLeeg(this string? input) {
			return !(string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input));
		}

		public static bool IsLeeg(this string? input) {
			return !input.IsNietLeeg();
		}

		public static void IsLeeg(Dictionary<string, string?> arr) {
			foreach (KeyValuePair<string, string?> item in arr) {
				if (item.Value.IsLeeg()) {
					MessageBox.Show($"{item.Key} mag niet leeg zijn", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}
			}
		}

		public static bool IsTelefoonNummerGeldig(this string input) {
			return RegexTelefoonnummer.IsMatch(input);
		}

		public static bool IsBtwNummerGeldig(this string input) {
			return RegexBtw.IsMatch(input);
		}

		#endregion

		#region Setters

		public static string ZetLeeg(this string input) {
			return string.Empty;
		}

		public static object? ZetNull(this object input) {
			return null;
		}

		public static string ToCamelCase(this string input) {
			if (input.Length > 1) {
				List<string> woorden = input.Split(" ").ToList();
				input = string.Join(" ", woorden.Select(i => i[0].ToString().ToUpper() + i[1..]));
			}
			return input;
		}

		#endregion
	}
}

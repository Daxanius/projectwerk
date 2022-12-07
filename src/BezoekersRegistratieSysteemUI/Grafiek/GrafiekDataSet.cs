using System.Collections.Generic;
using System.Windows.Media;

namespace BezoekersRegistratieSysteemUI.Grafiek {
	/// <summary>
	/// Een dataset is simpelweg een lijn of set data dat op de
	/// grafiek getekend kan worden.
	/// </summary>
	public class GrafiekDataset {
		/// <summary>
		/// De data die grafisch weergeven moet worden
		/// </summary>
		public List<double> Data { get; set; } = new();

		/// <summary>
		/// De kleur van de dataset, wordt
		/// in de legende zoal de grafiek weergeven
		/// </summary>
		public Brush Stroke { get; set; } = Brushes.DarkBlue;

		/// <summary>
		/// De tekendikte van de dataset
		/// </summary>
		public double StrokeThickness { get; set; } = 2;

		/// <summary>
		/// De label van de dataset, wordt 
		/// in de legende weergeven
		/// </summary>
		public string? Label { get; set; }

		/// <summary>
		/// Maakt een pen op
		/// </summary>
		/// <returns></returns>
		public Pen GeefPen() {
			return new(Stroke, StrokeThickness);
		}
	}
}
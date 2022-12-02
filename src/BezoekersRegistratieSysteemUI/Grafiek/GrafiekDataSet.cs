using System.Collections.Generic;
using System.Windows.Media;

namespace BezoekersRegistratieSysteemUI.Grafiek {
	/// <summary>
	/// Een dataset is simpelweg een lijn of set data dat op de
	/// grafiek getekend kan worden.
	/// </summary>
	public class GrafiekDataset {
		public List<double> Data { get; set; } = new();
		public Brush Stroke { get; set; } = Brushes.DarkBlue;
		public double StrokeThickness { get; set; } = 2;
		public string? Label { get; set; }

		public Pen GeefPen() {
			return new(Stroke, StrokeThickness);
		}
	}
}
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BezoekersRegistratieSysteemUI.Grafiek {
	public class GrafiekControl : Control {
		/// <summary>
		/// De waarden per kolom, steunt meerdere lijnen
		/// </summary>
		public List<GrafiekDataset> Datasets { get; set; } = new();

		/// <summary>
		/// Kolom labels, wordt onderaan de grafiek
		/// weergeven per data waarde (kolom).
		/// </summary>
		public List<string> KolomLabels { get; set; } = new();

		/// <summary>
		/// Het grafiektype
		/// </summary>
		public GrafiekType GrafiekType { get; set; } = GrafiekType.Lijn;

		/// <summary>
		/// De brush waarmee de achtergrond wordt getekend
		/// </summary>
		public Brush Stroke { get; set; } = Brushes.LightGray;

		/// <summary>
		/// De dikheid van de achtergrond brush
		/// </summary>
		public double StrokeThickness { get; set; } = 1;

		/// <summary>
		/// De grootte van een punt
		/// </summary>
		public int StrokeDot { get; set; } = 10;

		/// <summary>
		/// De tussenruimte tussen punten
		/// </summary>
		public int StrokeDotPadding { get; set; } = 5;

		/// <summary>
		/// Het lettertype van de tekst
		/// </summary>
		public string Font { get; set; } = "Arial";

		/// <summary>
		/// Met welke waarden de grafiek te incrementeren
		/// </summary>
		public double WaardeIncrement { get; set; } = 10;

		/// <summary>
		/// Tekst padding
		/// </summary>
		public double TextPadding { get; set; } = 10;

		/// <summary>
		/// Tekst schaal spul
		/// </summary>
		public double PixelsPerDip { get; set; } = 10;

		/// <summary>
		/// Ruimte tussen de bar elementen
		/// </summary>
		public double BarMargin { get; set; } = 2;

		/// <summary>
		/// Ruimte tussen de legende elementen
		/// </summary>
		public double LegendeMargin { get; set; } = 2;

		private double _hoogsteWaarde = 0;
		private int _langsteSet = 0;

		/// <summary>
		/// Geeft de absolute X positie van een kolom
		/// op de grafiek
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		private double GeefDataPositieX(int index) {
			// Correcties en offsets kunnen voorkomen bij
			// verschillende grafiektypes
			int correctie = 0;
			double offset = 0;

			switch (GrafiekType) {
				case GrafiekType.Lijn:
					correctie = -1;
					break;
				case GrafiekType.Bar:
					offset = Width / (_langsteSet * BarMargin);
					break;
				default:
					break;
			}

			return (index * (Width / (_langsteSet + correctie))) + offset;
		}

		/// <summary>
		/// Geeft de absolute Y positie van een stuk data
		/// op de grafiek
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		private double GeefDataPositieY(double data) {
			return Height - (data / _hoogsteWaarde * Height * 0.9);
		}

		/// <summary>
		/// Geeft de absolute hoogte van een stuk
		/// data op de grafiek
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		private double GeefDataHoogte(double data) {
			return data / _hoogsteWaarde * Height * 0.9;
		}

		/// <summary>
		/// Tekent een achtergrond gebaseerd op de grootste dataset
		/// </summary>
		/// <param name="drawingContext"></param>
		private void TekenAchtergrond(DrawingContext drawingContext) {
			for (int i = 0; i < _langsteSet; i++) {
				// X positie berekenen
				double x = GeefDataPositieX(i);

				// Verticale dotted lijn
				for (int ii = 0; ii < Height; ii += (StrokeDot + StrokeDotPadding)) {
					drawingContext.DrawLine(new(Stroke, StrokeThickness), new(x, ii), new(x, ii + StrokeDot));
				}

				// Horizontale dotted lijn
				//for (int ii = 0; ii < Width; ii += (StrokeDot + StrokeDotPadding)) {
				//	drawingContext.DrawLine(new(Stroke, StrokeThickness), new(ii, x), new(ii + StrokeDot, x));
				//}
			}
		}

		/// <summary>
		/// Tekent een dataset op de grafiek als lijn
		/// </summary>
		/// <param name="drawingContext"></param>
		private void TekenDatasetsLijn(DrawingContext drawingContext) {
			// We gaan door alle datasets gaan en ze individueel tekenen
			foreach (var dataset in Datasets) {
				for (int i = 0; i < dataset.Data.Count -1; i++) {
					Point punt = new(GeefDataPositieX(i),
						GeefDataPositieY(dataset.Data[i]));
					Point puntNext = new(GeefDataPositieX(i +1),
						GeefDataPositieY(dataset.Data[i +1]));

					drawingContext.DrawLine(dataset.GeefPen(), punt, puntNext);
				}
			}
		}

		/// <summary>
		/// Tekent een dataset op de grafiek als bargrafiek
		/// </summary>
		/// <param name="drawingContext"></param>
		private void TekenDatasetsBar(DrawingContext drawingContext) {
			// We gaan door alle datasets gaan en ze individueel tekenen
			foreach (var dataset in Datasets) {
				for (int i = 0; i < dataset.Data.Count; i++) {
					Size grootte = new(Width / (_langsteSet * BarMargin), GeefDataHoogte(dataset.Data[i]));
					Point punt = new(GeefDataPositieX(i) - (grootte.Width / 2), Height - grootte.Height);
					drawingContext.DrawRectangle(dataset.Stroke, dataset.GeefPen(), new(punt, grootte));
				}
			}
		}

		/// <summary>
		/// Tekent de kolom labels
		/// </summary>
		/// <param name="drawingContext"></param>
		private void TekenKolomLabels(DrawingContext drawingContext) {
			// Tekent de kolom labels
			for (int i = 0; i < KolomLabels.Count && i < _langsteSet; i++) {
				double x = GeefDataPositieX(i);
				drawingContext.DrawText(new(
						KolomLabels[i],
						CultureInfo.CurrentCulture,
						FlowDirection.LeftToRight,
						new(Font),
						FontSize,
						Foreground,
						PixelsPerDip),
					new(x - (KolomLabels[i].Length / 2 * FontSize) + PixelsPerDip / 2, Height + TextPadding));
			}
		}

		/// <summary>
		/// Tekent de data referenties
		/// </summary>
		/// <param name="drawingContext"></param>
		private void TekenDataReferentie(DrawingContext drawingContext) {
			// Tekent de nummers met een increment
			double capaciteit = _hoogsteWaarde / WaardeIncrement;
			int topInc = (int)Math.Floor(capaciteit);

			for (int i = 0; i <= topInc; i++) {
				double waarde = i * WaardeIncrement;

				drawingContext.DrawText(new(
						waarde.ToString(),
						CultureInfo.CurrentCulture,
						FlowDirection.RightToLeft,
						new(Font),
						FontSize,
						Foreground,
						PixelsPerDip),
					new(-TextPadding, GeefDataPositieY(waarde)));
			}

			// Als de hoogste waarde groter is dan de hoeveel keer
			// increment in waarde kan, geef aan welke waarde de hoogste
			// waarde heeft
			if (capaciteit > topInc) {
				drawingContext.DrawText(new(
					_hoogsteWaarde.ToString(),
					CultureInfo.CurrentCulture,
					FlowDirection.RightToLeft,
					new(Font),
					FontSize,
					Foreground,
					PixelsPerDip),
				new(-TextPadding, GeefDataPositieY(_hoogsteWaarde)));
			}
		}

		private void TekenLegende(DrawingContext drawingContext) {
			// Tekent de legende
			double labelCount = 0;
			for (int i = 0; i < Datasets.Count; i++) {
				var dataset = Datasets[i];
				if (dataset.Label is null) continue;

				var textHeight = labelCount + (Height / 2);
				Rect rect = new(new(Width + TextPadding + 10, textHeight + 2), new Size(10, 10));
				drawingContext.DrawRectangle(dataset.Stroke, dataset.GeefPen(), rect);
				drawingContext.DrawText(new(
				dataset.Label,
					CultureInfo.CurrentCulture,
					FlowDirection.LeftToRight,
					new(Font),
					FontSize,
					Foreground,
					PixelsPerDip),
				new(rect.Location.X + rect.Width + 5, textHeight));

				labelCount += LegendeMargin * 10;
			}
		}

		/// <summary>
		/// Tekent de textuele informatie
		/// </summary>
		/// <param name="drawingContext"></param>
		private void TekenInfo(DrawingContext drawingContext) {
			TekenKolomLabels(drawingContext);
			TekenDataReferentie(drawingContext);
			TekenLegende(drawingContext);
		}

		protected override void OnRender(DrawingContext drawingContext) {
			// Haalt de grootste sets op
			_langsteSet = Datasets.Max(s => s.Data.Count as int?) ?? 0;
			_hoogsteWaarde = Datasets.Max(x => x.Data.Max() as double?) ?? 0;

			TekenAchtergrond(drawingContext);

			switch (GrafiekType) {
				case GrafiekType.Bar:
					TekenDatasetsBar(drawingContext);
					break;
				default:
					TekenDatasetsLijn(drawingContext);
					break;
			}

			TekenInfo(drawingContext);
			base.OnRender(drawingContext);
		}

		static GrafiekControl() {
			DefaultStyleKeyProperty.OverrideMetadata(typeof(GrafiekControl), new FrameworkPropertyMetadata(typeof(GrafiekControl)));
		}
	}
}

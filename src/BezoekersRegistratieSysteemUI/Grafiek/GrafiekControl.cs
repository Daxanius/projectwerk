using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BezoekersRegistratieSysteemUI.Grafiek
{
    public class GrafiekControl : Control
    {
        /// <summary>
        /// De waarden per kolom, steunt meerdere lijnen
        /// </summary>
        public Dictionary<string, GrafiekDataset> Lijnen { get; set; } = new();

		/// <summary>
		/// De kolomnamen voor extra duidelijkheid
		/// </summary>
        public List<string> Kolommen { get; set; } = new();

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
        public int WaardeIncrement { get; set; } = 10;
        public double TextPadding { get; set; } = 10;
        public double PixelsPerDip { get; set; } = 10;
		public double BarMargin { get; set; } = 2;
		public double LegendeMargin { get; set; } = 2;

		/// <summary>
		/// Tekent een achtergrond gebaseerd op de grootste dataset
		/// </summary>
		/// <param name="drawingContext"></param>
		private void TekenAchtergrond(DrawingContext drawingContext) {
            // Haalt de grootste set op
            int langsteSet = Lijnen.Values.Max(s => s.Data.Count);

			// Voor een bar hebben we geen offset nodig
			int offset = 0;
			switch (GrafiekType) {
				case GrafiekType.Lijn:
					offset = -1;
					break;
				default:
					break;
			}

			for (int i = 0; i < langsteSet; i++) {
                // X positie berekenen
                double x = i * (Width / (langsteSet + offset));

				// Verticale dotted lijn
				for (int ii = 0; ii < Height; ii += (StrokeDot + StrokeDotPadding)) {
					drawingContext.DrawLine(new(Stroke, StrokeThickness), new(x, ii), new(x, ii + StrokeDot));
				}

                // Horizontale dotted lijn
				//for (int ii = 0; ii < Width; ii += (MileStrokeDot + MileStrokeDotPadding))
				//{
				//    drawingContext.DrawLine(new(MileStroke, MileStrokeThickness), new(ii, height), new(ii + MileStrokeDot, height));
				//}
			}
		}

        /// <summary>
        /// Tekent een dataset op de grafiek als lijn
        /// </summary>
        /// <param name="drawingContext"></param>
        private void TekenDatasetsLijn(DrawingContext drawingContext) {
            // Haalt de hoogste waarde op
            double maxWaarde = Lijnen.Values.Max(x => x.Data.Max());
			int langsteSet = Lijnen.Values.Max(s => s.Data.Count);

			// We gaan door alle datasets gaan en ze individueel tekenen
			foreach (var key in Lijnen.Keys) {
                for (int i = 0; i < Lijnen[key].Data.Count -1; i++) {
                    Point punt = new(i * (Width / (langsteSet - 1)), 
                        Height - (Lijnen[key].Data[i] / maxWaarde * Height * 0.9));
                    Point puntNext = new((i + 1) * (Width / (langsteSet - 1)), 
                        Height - (Lijnen[key].Data[i + 1] / maxWaarde * Height * 0.9));

                    drawingContext.DrawLine(Lijnen[key].GeefPen(), punt, puntNext);
				}
			}
        }

		/// <summary>
		/// Tekent een dataset op de grafiek als bargrafiek
		/// </summary>
		/// <param name="drawingContext"></param>
		private void TekenDatasetsBar(DrawingContext drawingContext) {
			// Haalt de hoogste waarde op
			double maxWaarde = Lijnen.Values.Max(x => x.Data.Max());
			int langsteSet = Lijnen.Values.Max(s => s.Data.Count);

			// We gaan door alle datasets gaan en ze individueel tekenen
			foreach (var key in Lijnen.Keys) {
				for (int i = 0; i < Lijnen[key].Data.Count; i++) {
					Point punt = new(i * (Width / langsteSet), Height - (Lijnen[key].Data[i] / maxWaarde * Height * 0.9));
					Rect rect = new(punt, new Size(Width / (langsteSet * BarMargin), Lijnen[key].Data[i] / maxWaarde * Height * 0.9));
					drawingContext.DrawRectangle(Lijnen[key].Stroke, Lijnen[key].GeefPen(), rect);
				}
			}
		}

		/// <summary>
		/// Tekent de textuele informatie
		/// </summary>
		/// <param name="drawingContext"></param>
		private void TekenInfo(DrawingContext drawingContext) {
			// Haalt de hoogste waarden op
			double maxWaarde = Lijnen.Values.Max(x => x.Data.Max());
			int langsteSet = Lijnen.Values.Max(s => s.Data.Count);

			// Voor een bar hebben we geen offset nodig
			int offset = 0;
			switch (GrafiekType) {
				case GrafiekType.Lijn:
					offset = -1;
					break;
				default:
					break;
			}

			// Tekent de onderste legende
			for (int i = 0; i < Kolommen.Count && i < langsteSet; i++) {
				var width = i * (Width / (langsteSet + offset));
				drawingContext.DrawText(new(
						Kolommen[i],
						CultureInfo.CurrentCulture,
						FlowDirection.LeftToRight,
						new(Font),
						FontSize,
						Foreground,
						PixelsPerDip),
					new(width - ((Kolommen[i].Length / 2) * FontSize) + PixelsPerDip / 2, Height + TextPadding));
			}

			// Tekent de nummers met een increment
			for (int i = 0; i <= maxWaarde; i += WaardeIncrement) {
				var textHeight = Height - (i / maxWaarde * Height * 0.9);

				drawingContext.DrawText(new(
						i.ToString(),
						CultureInfo.CurrentCulture,
						FlowDirection.RightToLeft,
						new(Font),
						FontSize,
						Foreground,
						PixelsPerDip),
					new(-TextPadding, textHeight));
			}

			// Tekent de legende
			double count = 0;
			foreach (var key in Lijnen.Keys) {
				var textHeight = count + (Height / 2);

				Rect rect = new(new(Width + TextPadding, textHeight + 2), new Size(10, 10));
				drawingContext.DrawRectangle(Lijnen[key].Stroke, Lijnen[key].GeefPen(), rect);
				drawingContext.DrawText(new(
					key,
					CultureInfo.CurrentCulture,
					FlowDirection.LeftToRight,
					new(Font),
					FontSize,
					Foreground,
					PixelsPerDip),
				new(Width + TextPadding + rect.Width + 5, textHeight));

				count += LegendeMargin * 10;
			}
		}

        protected override void OnRender(DrawingContext drawingContext) {
            base.OnRender(drawingContext);
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
		}

        public GrafiekControl() {
            // Een eerste dataset definieren
            #region DummyData
   //         GrafiekDataset ds = new() {
			//	Data = new() {
			//		23,
			//		34,
			//		65,
			//		65,
			//		100,
			//		34,
			//		54
			//	}
			//};

			//GrafiekDataset ds1 = new() {
			//	Data = new() {
			//		65,
			//		12,
			//		98,
			//		34,
			//		45,
			//		100,
			//		76
			//	},
			//	Stroke = Brushes.OrangeRed
			//};

			//// Kolommen definieren
			//Kolommen = new() {
   //             "Ma",
   //             "Di",
   //             "Wo",
   //             "Do",
   //             "Vr",
   //             "Za",
   //             "Zo"
   //         };

   //         // De dataset toevoegen
			//Lijnen.Add("Gemiddeld", ds);
			//Lijnen.Add("Totaal ofz", ds1);



			//GrafiekType = GrafiekType.Bar;
            #endregion
        }

        static GrafiekControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GrafiekControl), new FrameworkPropertyMetadata(typeof(GrafiekControl)));
        }
    }
}

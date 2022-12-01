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
        public Dictionary<string, double> Waarden { get; set; } = new();
        public Brush Stroke { get; set; } = Brushes.DarkBlue;
        public double StrokeThickness { get; set; } = 2;
        public Brush MileStroke { get; set; } = Brushes.LightGray;
        public double MileStrokeThickness { get; set; } = 1;
        public int MileStrokeDot { get; set; } = 10;
        public int MileStrokeDotPadding { get; set; } = 5;
        public int ValueIncrement { get; set; } = 10;
        public String Font { get; set; } = "Arial";
        public double TextPadding { get; set; } = 10;
        public double PixelsPerDip { get; set; } = 10;

        protected override void OnRender(DrawingContext drawingContext) {
            base.OnRender(drawingContext);
            Pen pen = new(Stroke, StrokeThickness);
            var max = Waarden.Values.Max();

			List<Point> punten = new();
            foreach (var key in Waarden.Keys) {
                var width = punten.Count * (Width / (Waarden.Keys.Count-1));
				punten.Add(new(width, Height - (Waarden[key] / max * Height * 0.9)));
				drawingContext.DrawText(new(
                        key, 
                        CultureInfo.CurrentCulture, 
                        FlowDirection.LeftToRight, 
                        new(Font), FontSize, 
                        Foreground, 
                        PixelsPerDip), 
                    new(width, Height + TextPadding));
			}

			for (int i = 0; i < punten.Count; i++) {
				for (int ii = 0; ii < Height; ii += (MileStrokeDot + MileStrokeDotPadding)) {
					drawingContext.DrawLine(new(MileStroke, MileStrokeThickness), new(punten[i].X, ii), new(punten[i].X, ii + MileStrokeDot));
				}

                if (i >= punten.Count - 1) {
                    continue;
                }

				drawingContext.DrawLine(pen, punten[i], punten[i+1]);
			}

			for (int i = 0; i <= max; i += ValueIncrement) {
                var textHeight = Height - (i / max * Height * 0.9);

				//for (int ii = 0; ii < Width; ii += (MileStrokeDot + MileStrokeDotPadding)) {
				//	drawingContext.DrawLine(new(MileStroke, MileStrokeThickness), new(ii, height), new(ii + MileStrokeDot, height));
				//}

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
		}

        public GrafiekControl() {
            Waarden.Add("Ma", 30);
            Waarden.Add("Di", 100);
            Waarden.Add("Wo", 34);
            Waarden.Add("Do", 67);
            Waarden.Add("Vr", 54);
            Waarden.Add("Za", 3);
            Waarden.Add("Zo", 0);
        }

        static GrafiekControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GrafiekControl), new FrameworkPropertyMetadata(typeof(GrafiekControl)));
        }
    }
}

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

                //for (int ii = 0; ii < Width; ii += (MileStrokeDot + MileStrokeDotPadding))
                //{
                //    drawingContext.DrawLine(new(MileStroke, MileStrokeThickness), new(ii, height), new(ii + MileStrokeDot, height));
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
            Waarden.Add("1u", 30);
            Waarden.Add("2u", 100);
            Waarden.Add("3u", 34);
            Waarden.Add("4u", 67);
            Waarden.Add("5u", 54);
            Waarden.Add("6u", 3);
            Waarden.Add("7u", 0);
            Waarden.Add("8u", 30);
            Waarden.Add("9u", 100);
            Waarden.Add("10u", 34);
            Waarden.Add("11u", 67);
            Waarden.Add("12u", 54);
            Waarden.Add("13u", 3);
            Waarden.Add("14u", 0);
            Waarden.Add("15u", 30);
            Waarden.Add("16u", 100);
            Waarden.Add("17u", 34);
            Waarden.Add("18u", 67);
            Waarden.Add("19u", 54);
            Waarden.Add("20u", 3);
            Waarden.Add("21u", 0);
            Waarden.Add("22u", 0);
            Waarden.Add("23u", 0);
            Waarden.Add("24u", 0);
        }

        static GrafiekControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GrafiekControl), new FrameworkPropertyMetadata(typeof(GrafiekControl)));
        }
    }
}

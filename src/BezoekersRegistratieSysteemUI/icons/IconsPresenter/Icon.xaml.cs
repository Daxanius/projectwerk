using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BezoekersRegistratieSysteemUI.icons.IconsPresenter {
	/// <summary>
	/// Interaction logic for Icon.xaml
	/// </summary>
	public partial class Icon : UserControl {
		public Icon() {
			this.DataContext = this;
			InitializeComponent();
		}

		public int IconSize {
			get { return (int)GetValue(IconSizeProperty); }
			set { SetValue(IconSizeProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Property1.  
		// This enables animation, styling, binding, etc...
		public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register(
				  nameof(IconSize),
				  typeof(int),
				  typeof(Icon),
				  new PropertyMetadata(40)
			  );

		public int CanvasWidth {
			get { return (int)GetValue(CanvasWidthPropperty); }
			set { SetValue(CanvasWidthPropperty, value); }
		}

		// Using a DependencyProperty as the backing store for Property1.  
		// This enables animation, styling, binding, etc...
		public static readonly DependencyProperty CanvasWidthPropperty = DependencyProperty.Register(
				  nameof(CanvasWidth),
				  typeof(int),
				  typeof(Icon),
				  new PropertyMetadata(50)
			  );

		public Thickness IconPadding {
			get { return (Thickness)GetValue(IconPaddingPropperty); }
			set { SetValue(IconPaddingPropperty, value); }
		}

		// Using a DependencyProperty as the backing store for Property1.  
		// This enables animation, styling, binding, etc...
		public static readonly DependencyProperty IconPaddingPropperty = DependencyProperty.Register(
				  nameof(IconPadding),
				  typeof(Thickness),
				  typeof(Icon),
				  new PropertyMetadata(new Thickness(0))
			  );

		public int CanvasHeight {
			get { return (int)GetValue(CanvasHeightPropperty); }
			set { SetValue(CanvasHeightPropperty, value); }
		}

		// Using a DependencyProperty as the backing store for Property1.  
		// This enables animation, styling, binding, etc...
		public static readonly DependencyProperty CanvasHeightPropperty = DependencyProperty.Register(
				  nameof(CanvasHeight),
				  typeof(int),
				  typeof(Icon),
				  new PropertyMetadata(50)
			  );

		public int CircleSize {
			get { return (int)GetValue(CircleSizeProppperty); }
			set { SetValue(CircleSizeProppperty, value); }
		}

		// Using a DependencyProperty as the backing store for Property1.  
		// This enables animation, styling, binding, etc...
		public static readonly DependencyProperty CircleSizeProppperty = DependencyProperty.Register(
				  nameof(CircleSize),
				  typeof(int),
				  typeof(Icon),
				  new PropertyMetadata(40)
			  );

		public Brush CircleBackground {
			get { return (Brush)GetValue(CircleBackgroundProppperty); }
			set { SetValue(CircleBackgroundProppperty, value); }
		}

		// Using a DependencyProperty as the backing store for Property1.  
		// This enables animation, styling, binding, etc...
		public static readonly DependencyProperty CircleBackgroundProppperty = DependencyProperty.Register(
				  nameof(CircleBackground),
				  typeof(Brush),
				  typeof(Icon),
				  new PropertyMetadata(Brushes.White)
			  );

		public string IconSource {
			get => (string)GetValue(IconSourcePropperty);
			set => SetValue(IconSourcePropperty, value);
		}

		// Using a DependencyProperty as the backing store for Property1.  
		// This enables animation, styling, binding, etc...
		public static readonly DependencyProperty IconSourcePropperty = DependencyProperty.Register(
				  nameof(IconSource),
				  typeof(string),
				  typeof(Icon),
				  new PropertyMetadata("../DefaultLeeg.xaml")
			  );
	}
}

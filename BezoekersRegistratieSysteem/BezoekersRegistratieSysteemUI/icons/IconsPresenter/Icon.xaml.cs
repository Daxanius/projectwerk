using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
		
		public int IconOffsetLeft {
			get { return (int)GetValue(IconOffsetLeftPropperty); }
			set { SetValue(IconOffsetLeftPropperty, value); }
		}

		// Using a DependencyProperty as the backing store for Property1.  
		// This enables animation, styling, binding, etc...
		public static readonly DependencyProperty IconOffsetLeftPropperty = DependencyProperty.Register(
				  nameof(IconOffsetLeft),
				  typeof(int),
				  typeof(Icon),
				  new PropertyMetadata(8)
			  );

		public int IconOffsetTop {
			get { return (int)GetValue(IconOffsetTopPropperty); }
			set { SetValue(IconOffsetTopPropperty, value); }
		}

		// Using a DependencyProperty as the backing store for Property1.  
		// This enables animation, styling, binding, etc...
		public static readonly DependencyProperty IconOffsetTopPropperty = DependencyProperty.Register(
				  nameof(IconOffsetTop),
				  typeof(int),
				  typeof(Icon),
				  new PropertyMetadata(8)
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

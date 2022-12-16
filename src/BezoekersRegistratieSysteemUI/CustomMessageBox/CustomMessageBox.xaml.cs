using System.Windows;
using System.Windows.Controls;

namespace BezoekersRegistratieSysteemUI.MessageBoxes {
	public partial class CustomMessageBox : Window, ICustomMessageBox {
		private static ECustomMessageBoxResult result = ECustomMessageBoxResult.Cancel;

		public ECustomMessageBoxResult Show(string message, string title, ECustomMessageBoxIcon icon) {
			InitializeComponent();

			this.Title = "Opgelet !";
			TitleLabel.Content = title;
			TextTextBox.Text = message;

			switch (icon) {
				case ECustomMessageBoxIcon.Warning:
					Icon.IconSource = "../WarningIcon.xaml";
					break;
				case ECustomMessageBoxIcon.Question:
					Icon.IconSource = "../QuestionIcon.xaml";
					break;
				case ECustomMessageBoxIcon.Information:
					Icon.IconSource = "../InformationIcon.xaml";
					AnnulerenButton.Visibility = Visibility.Collapsed;
					BevestigenButton.Visibility = Visibility.Collapsed;
					SluitButton.Visibility = Visibility.Visible;
					break;
				case ECustomMessageBoxIcon.Error:
					Icon.IconSource = "../WarningIcon.xaml";
					AnnulerenButton.Visibility = Visibility.Collapsed;
					BevestigenButton.Visibility = Visibility.Collapsed;
					SluitButton.Visibility = Visibility.Visible;
					break;
			}

			if (this.ShowDialog() == true) {
				return result;
			}
			return result;
		}

		private void Button_Click(object sender, RoutedEventArgs e) {
			Button button = (Button)sender;
			if (button.Content.ToString() == "Bevestigen") result = ECustomMessageBoxResult.Bevestigen;
			if (button.Content.ToString() == "Annuleren") result = ECustomMessageBoxResult.Annuleren;
			if (button.Content.ToString() == "Sluit") result = ECustomMessageBoxResult.Sluit;
			this.DialogResult = true;
			this.Close();
		}
	}
}

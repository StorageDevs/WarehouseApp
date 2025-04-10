using System.Windows;
using System.Windows.Controls;

namespace WarehouseApp.Windows.Pages
{
    public partial class SettingsPage : Page
    {
        private string loggedInUsername = "admin"; // Jelenlegi bejelentkezett felhasználó

        public SettingsPage()
        {
            InitializeComponent();
            TxtCurrentUsername.Text = loggedInUsername; // Beállítjuk a jelenlegi felhasználónevet
        }

        private void BtnChangeUsername_Click(object sender, RoutedEventArgs e)
        {
            string newUsername = TxtNewUsername.Text;
            string confirmUsername = TxtConfirmUsername.Text;

            if (string.IsNullOrWhiteSpace(newUsername) || string.IsNullOrWhiteSpace(confirmUsername))
            {
                MessageBox.Show("A mezők nem lehetnek üresek!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (newUsername == confirmUsername)
            {
                MessageBox.Show("Felhasználónév megváltoztatva!", "Siker", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Az új felhasználónevek nem egyeznek!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            string newPassword = TxtNewPassword.Password;
            string confirmPassword = TxtConfirmPassword.Password;

            if (string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("A mezők nem lehetnek üresek!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (newPassword == confirmPassword)
            {
                MessageBox.Show("Jelszó megváltoztatva!", "Siker", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Az új jelszavak nem egyeznek!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

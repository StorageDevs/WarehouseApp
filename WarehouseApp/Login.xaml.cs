using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace WarehouseApp
{
    public partial class Login : Window
    {
        private readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5000/api/auth/") };

        public Login()
        {
            InitializeComponent();
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            // -----------------------------------------
            // 🔹 MOCK BEJELENTKEZÉS (NINCS BACKEND)
            if (username == "admin" && password == "admin")
            {
                // Fake token mentése
                Properties.Settings.Default.AccessToken = "fake-jwt-token";
                Properties.Settings.Default.Save();

                // Admin felület megnyitása
                AdminDashboard adminWindow = new AdminDashboard();
                adminWindow.Show();
                this.Close();
                return; // Megakadályozza a további futást
            }
            else
            {
                MessageBox.Show("Sikertelen bejelentkezés!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            // 🔹 Eddig tart a MOCKOLT bejelentkezés
            // -----------------------------------------

        }

        // 🔹 ENTER GOMB FIGYELÉSE A JELSZÓ MEZŐBEN
        private void txtPassword_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter) // Csak akkor fut le, ha az Entert nyomják meg
            {
                btnLogin_Click(sender, e);
            }
        }
    }
}

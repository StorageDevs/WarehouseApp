using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Windows;

namespace WarehouseApp.Windows.Popups
{
    public partial class FirstLoginWindow : Window
    {
        private readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7188/") };
        private readonly string _userId;
        private readonly string _userName;

        public FirstLoginWindow(string userName, string userId)
        {
            InitializeComponent();
            _userId = userId;
            _userName = userName;
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            string newPassword = TxtNewPassword.Password;
            string confirmPassword = TxtConfirmPassword.Password;

            if (string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("Minden mező kitöltése kötelező!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("A két jelszó nem egyezik!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!IsValidPassword(newPassword))
            {
                MessageBox.Show("A jelszónak legalább 6 karakter hosszúnak kell lennie, kis- és nagybetűt, számot és speciális karaktert kell tartalmaznia.", "Hibás jelszó", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", Properties.Settings.Default.AccessToken);

                var payload = new
                {
                    oldPassword = "TempPassword-123",
                    newPassword = newPassword
                };

                var json = JsonConvert.SerializeObject(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"auth/UpdatePassword/{_userId}", content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("A jelszavad sikeresen módosítva lett!", "Siker", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Hiba történt:\n{error}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hálózati hiba: " + ex.Message, "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txtPassword_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                BtnSave_Click(sender, e);
            }
        }

        private bool IsValidPassword(string password)
        {
            return password.Length >= 6 &&
                   password.Any(char.IsLower) &&
                   password.Any(char.IsUpper) &&
                   password.Any(char.IsDigit) &&
                   password.Any(c => "!@#$%^&*()_+-=[]{}/<>.,;:!?".Contains(c));
        }
    }
}

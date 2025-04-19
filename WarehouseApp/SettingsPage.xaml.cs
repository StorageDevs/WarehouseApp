using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;

namespace WarehouseApp.Windows.Pages
{
    public partial class SettingsPage : Page
    {
        private readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7188/") };

        public SettingsPage()
        {
            InitializeComponent();
        }

        private async void BtnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            string currentPassword = TxtCurrentPassword.Password;
            string newPassword = TxtNewPassword.Password;
            string confirmPassword = TxtConfirmPassword.Password;

            // 1. Mezők ellenőrzése
            if (string.IsNullOrWhiteSpace(currentPassword) ||
                string.IsNullOrWhiteSpace(newPassword) ||
                string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("Kérlek töltsd ki az összes mezőt!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 2. Új jelszavak egyezése
            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Az új jelszavak nem egyeznek!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 3. Új jelszó validálása
            if (!IsValidPassword(newPassword))
            {
                MessageBox.Show("Az új jelszónak legalább 6 karakter hosszúnak kell lennie, és tartalmaznia kell kis- és nagybetűt, számot, valamint speciális karaktert (!@#$...)", "Hibás formátum", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // 🔐 4. Hitelesítés a jelenlegi jelszóval a Login végponttal
                string userName = Properties.Settings.Default.UserName;

                var loginPayload = new
                {
                    userName = userName,
                    password = currentPassword
                };

                var loginJson = JsonConvert.SerializeObject(loginPayload);
                var loginContent = new StringContent(loginJson, Encoding.UTF8, "application/json");

                var loginResponse = await _httpClient.PostAsync("auth/Login", loginContent);
                string loginResponseData = await loginResponse.Content.ReadAsStringAsync();

                dynamic loginResult = JsonConvert.DeserializeObject(loginResponseData);
                string tokenFromLogin = loginResult?.token;

                if (!loginResponse.IsSuccessStatusCode || string.IsNullOrEmpty(tokenFromLogin))
                {
                    MessageBox.Show("A jelenlegi jelszó hibás!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // 5. Az új jelszó nem lehet azonos a jelenlegivel
                if (newPassword == currentPassword)
                {
                    MessageBox.Show("Az új jelszó nem lehet azonos a jelenlegi jelszóval!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // 6. Auth header beállítás a jelszóváltáshoz
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", Properties.Settings.Default.AccessToken);

                // 7. UserID ellenőrzése
                string userId = Properties.Settings.Default.UserID;
                if (string.IsNullOrWhiteSpace(userId))
                {
                    MessageBox.Show("Nem található a felhasználó azonosítója!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // 8. Payload a módosításhoz
                var payload = new
                {
                    oldPassword = currentPassword,
                    newPassword = newPassword
                };

                var json = JsonConvert.SerializeObject(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"auth/UpdatePassword/{userId}", content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Jelszó sikeresen megváltoztatva!", "Siker", MessageBoxButton.OK, MessageBoxImage.Information);
                    TxtCurrentPassword.Clear();
                    TxtNewPassword.Clear();
                    TxtConfirmPassword.Clear();
                }
                else
                {
                    string error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Hiba a jelszó módosításakor:\n{error}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hálózati hiba: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // 🔐 Jelszó validálás
        private bool IsValidPassword(string password)
        {
            if (password.Length < 6) return false;
            if (!password.Any(char.IsLower)) return false;
            if (!password.Any(char.IsUpper)) return false;
            if (!password.Any(char.IsDigit)) return false;
            if (!password.Any(c => "!@#$%^&*()_+-=[]{}/<>.,;:!?".Contains(c))) return false;

            return true;
        }
    }
}

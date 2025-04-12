using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using WarehouseApp.Windows.Pages;
using System.Windows.Controls;


namespace WarehouseApp.Windows.Popups
{
    public partial class AddUserWindow : Window
    {
        private readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7188/") };

        public AddUserWindow()
        {
            InitializeComponent();
        }

        private async void AddUser_Click(object sender, RoutedEventArgs e)
        {
            string userName = txtUserName.Text.Trim();
            string fullName = txtFullName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = "TempPassword-123!";

            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Kérlek töltsd ki az összes mezőt!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var newUser = new
            {
                userName = userName,
                fullName = fullName,
                password = password,
                email = email
            };

            try
            {
                string json = JsonConvert.SerializeObject(newUser);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", Properties.Settings.Default.AccessToken);

                HttpResponseMessage response = await _httpClient.PostAsync("auth/Register", content);

                if (response.IsSuccessStatusCode)
                {
                    // 👇 Itt lekérjük a szerepkört a ComboBoxból
                    string selectedRole = ((ComboBoxItem)cmbRole.SelectedItem).Content.ToString();

                    // 👇 Második kérés: szerepkör hozzárendelése
                    HttpResponseMessage roleResponse = await _httpClient.PostAsync($"auth/Assignrole?UserName={userName}&roleName={selectedRole}", null);

                    if (roleResponse.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Felhasználó és szerepkör sikeresen hozzárendelve!", "Siker", MessageBoxButton.OK, MessageBoxImage.Information);

                        var adminWindow = Application.Current.Windows[0] as AdminDashboard;
                        if (adminWindow != null)
                        {
                            adminWindow.ContentFrame.Content = new UsersPage();
                        }

                        this.DialogResult = true;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Felhasználó létrejött, de a szerepkör hozzárendelése sikertelen volt.", "Figyelmeztetés", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Hiba történt a felhasználó hozzáadásakor!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hálózati hiba: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Bezárja az ablakot anélkül, hogy bármit mentene
        }
    }
}

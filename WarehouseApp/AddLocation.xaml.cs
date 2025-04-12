using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Windows;
using Newtonsoft.Json;

namespace WarehouseApp.Windows.Popups
{
    public partial class AddLocationWindow : Window
    {
        private readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7055/api/") };

        public Location NewLocation { get; private set; }

        public AddLocationWindow()
        {
            InitializeComponent();
        }

        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            string name = txtName.Text.Trim();
            string description = txtDescription.Text.Trim();
            int capacity;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(description) || !int.TryParse(txtCapacity.Text, out capacity))
            {
                MessageBox.Show("Kérlek töltsd ki az összes mezőt helyesen!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var newLocation = new
            {
                locationName = name,
                locationDescription = description,
                locationCapacity = capacity
            };

            try
            {
                string json = JsonConvert.SerializeObject(newLocation);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", Properties.Settings.Default.AccessToken);

                HttpResponseMessage response = await _httpClient.PostAsync("Locations", content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Sikeresen hozzáadva!", "Siker", MessageBoxButton.OK, MessageBoxImage.Information);

                    NewLocation = new Location
                    {
                        LocationName = name,
                        LocationDescription = description,
                        LocationCapacity = capacity
                    };

                    var adminWindow = Application.Current.Windows[0] as AdminDashboard;
                    if (adminWindow != null)
                    {
                        adminWindow.ContentFrame.Content = new LocationsPage();
                    }

                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    string serverResponse = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Hiba történt a hozzáadáskor!\nSzerver válasza:\n{serverResponse}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hálózati hiba: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}

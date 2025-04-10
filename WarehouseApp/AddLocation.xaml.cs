using System;
using System.Net.Http;
using System.Text;
using System.Windows;
using Newtonsoft.Json;

namespace WarehouseApp.Windows.Popups
{
    public partial class AddLocationWindow : Window
    {
        private readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5118/api/") };

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

                HttpResponseMessage response = await _httpClient.PostAsync("Locations", content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Sikeresen hozzáadva!", "Siker", MessageBoxButton.OK, MessageBoxImage.Information);

                    var adminWindow = Application.Current.Windows[0] as AdminDashboard;
                    if (adminWindow != null)
                    {
                        adminWindow.ContentFrame.Content = new LocationsPage();
                    }

                    this.Close(); // bezárja a popupot
                }
                else
                {
                    MessageBox.Show("Hiba történt a hozzáadáskor!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hálózati hiba: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

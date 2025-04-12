using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Windows;
using Newtonsoft.Json;

namespace WarehouseApp
{
    public partial class EditLocationWindow : Window
    {
        private readonly Location _location;
        private readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7055/api/") };

        public Location UpdatedLocation { get; private set; }

        public EditLocationWindow(Location location)
        {
            InitializeComponent();
            _location = location;

            lblNameCurrent.Text = _location.LocationName;
            lblDescriptionCurrent.Text = _location.LocationDescription;
            lblCapacityCurrent.Text = _location.LocationCapacity.ToString();

            chkNameKeep.Checked += ToggleFields;
            chkNameKeep.Unchecked += ToggleFields;

            chkDescriptionKeep.Checked += ToggleFields;
            chkDescriptionKeep.Unchecked += ToggleFields;

            chkCapacityKeep.Checked += ToggleFields;
            chkCapacityKeep.Unchecked += ToggleFields;
        }

        private void ToggleFields(object sender, RoutedEventArgs e)
        {
            txtNameNew.IsEnabled = !(chkNameKeep.IsChecked ?? false);
            txtDescriptionNew.IsEnabled = !(chkDescriptionKeep.IsChecked ?? false);
            txtCapacityNew.IsEnabled = !(chkCapacityKeep.IsChecked ?? false);
        }

        private async void ConfirmEdit_Click(object sender, RoutedEventArgs e)
        {
            string name = chkNameKeep.IsChecked == true ? _location.LocationName : txtNameNew.Text.Trim();
            string description = chkDescriptionKeep.IsChecked == true ? _location.LocationDescription : txtDescriptionNew.Text.Trim();
            int capacity = chkCapacityKeep.IsChecked == true ? _location.LocationCapacity : int.TryParse(txtCapacityNew.Text, out int c) ? c : -1;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(description) || capacity < 0)
            {
                MessageBox.Show("Kérlek töltsd ki az adatokat helyesen!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var updatedLocation = new
            {
                locationID = _location.LocationID,
                locationName = name,
                locationDescription = description,
                locationCapacity = capacity
            };

            try
            {
                string json = JsonConvert.SerializeObject(updatedLocation);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // 🔐 Token beállítása
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", Properties.Settings.Default.AccessToken);

                HttpResponseMessage response = await _httpClient.PutAsync($"Locations/{_location.LocationID}", content);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Sikeres módosítás!", "OK", MessageBoxButton.OK, MessageBoxImage.Information);

                    UpdatedLocation = new Location
                    {
                        LocationID = _location.LocationID,
                        LocationName = name,
                        LocationDescription = description,
                        LocationCapacity = capacity
                    };

                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    string serverResponse = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Sikertelen módosítás!\nSzerver válasza:\n{serverResponse}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
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

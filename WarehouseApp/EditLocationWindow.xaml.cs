using System;
using System.Net.Http;
using System.Text;
using System.Windows;
using Newtonsoft.Json;

namespace WarehouseApp
{
    public partial class EditLocationWindow : Window
    {
        private readonly Location _location;
        private readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5118/api/") };

        public EditLocationWindow(Location location)
        {
            InitializeComponent();
            _location = location;

            // Aktuális értékek megjelenítése
            lblNameCurrent.Text = _location.LocationName;
            lblDescriptionCurrent.Text = _location.LocationDescription;
            lblCapacityCurrent.Text = _location.LocationCapacity.ToString();

            // Események a pipákhoz
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
            // NÉV validálás
            string name;
            if (chkNameKeep.IsChecked == true)
            {
                name = _location.LocationName;
            }
            else if (string.IsNullOrWhiteSpace(txtNameNew.Text))
            {
                MessageBox.Show("Az új név nem lehet üres!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            {
                name = txtNameNew.Text.Trim();
            }

            // LEÍRÁS validálás
            string description;
            if (chkDescriptionKeep.IsChecked == true)
            {
                description = _location.LocationDescription;
            }
            else if (string.IsNullOrWhiteSpace(txtDescriptionNew.Text))
            {
                MessageBox.Show("Az új leírás nem lehet üres!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            {
                description = txtDescriptionNew.Text.Trim();
            }

            // KAPACITÁS validálás
            int capacity;
            if (chkCapacityKeep.IsChecked == true)
            {
                capacity = _location.LocationCapacity;
            }
            else if (string.IsNullOrWhiteSpace(txtCapacityNew.Text) || !int.TryParse(txtCapacityNew.Text, out capacity))
            {
                MessageBox.Show("Az új kapacitás nem lehet üres és számnak kell lennie!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // JSON objektum létrehozása a backend formátum szerint
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

                HttpResponseMessage response = await _httpClient.PutAsync($"Locations/{_location.LocationID}", content);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Sikeres módosítás!", "OK", MessageBoxButton.OK, MessageBoxImage.Information);

                    var adminWindow = Application.Current.Windows[0] as AdminDashboard;
                    if (adminWindow != null)
                    {
                        adminWindow.ContentFrame.Content = new LocationsPage();
                    }

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
            this.Close();
        }
    }
}

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Windows;
using Newtonsoft.Json;
using WarehouseApp.Models;

namespace WarehouseApp.Windows.Popups
{
    public partial class EditMaterialWindow : Window
    {
        private readonly Material _material;
        private readonly HttpClient _httpClient = new HttpClient();

        public EditMaterialWindow(Material material)
        {
            InitializeComponent();
            _material = material;

            // Jelenlegi értékek megjelenítése
            lblNumberCurrent.Text = material.MaterialNumber.ToString();
            lblDescriptionCurrent.Text = material.MaterialDescription;
            lblUnitCurrent.Text = material.Unit;
            lblPriceCurrent.Text = material.PriceUnit.ToString();

            // Pipák eseményei
            chkNumberKeep.Checked += ToggleFields;
            chkNumberKeep.Unchecked += ToggleFields;

            chkDescriptionKeep.Checked += ToggleFields;
            chkDescriptionKeep.Unchecked += ToggleFields;

            chkUnitKeep.Checked += ToggleFields;
            chkUnitKeep.Unchecked += ToggleFields;

            chkPriceKeep.Checked += ToggleFields;
            chkPriceKeep.Unchecked += ToggleFields;

            ToggleFields(null, null);
        }

        private void ToggleFields(object sender, RoutedEventArgs e)
        {
            txtNumberNew.IsEnabled = !(chkNumberKeep.IsChecked ?? false);
            txtDescriptionNew.IsEnabled = !(chkDescriptionKeep.IsChecked ?? false);
            txtUnitNew.IsEnabled = !(chkUnitKeep.IsChecked ?? false);
            txtPriceNew.IsEnabled = !(chkPriceKeep.IsChecked ?? false);
        }

        private async void ConfirmEdit_Click(object sender, RoutedEventArgs e)
        {
            int number;
            decimal price;

            // Anyagszám
            if (chkNumberKeep.IsChecked == true)
                number = _material.MaterialNumber;
            else if (!int.TryParse(txtNumberNew.Text, out number))
            {
                MessageBox.Show("Az új anyagszám nem lehet üres és számnak kell lennie!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Leírás
            string description = chkDescriptionKeep.IsChecked == true ? _material.MaterialDescription : txtDescriptionNew.Text.Trim();
            if (chkDescriptionKeep.IsChecked != true && string.IsNullOrWhiteSpace(description))
            {
                MessageBox.Show("Az új leírás nem lehet üres!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Mértékegység
            string unit = chkUnitKeep.IsChecked == true ? _material.Unit : txtUnitNew.Text.Trim();
            if (chkUnitKeep.IsChecked != true && string.IsNullOrWhiteSpace(unit))
            {
                MessageBox.Show("Az új mértékegység nem lehet üres!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Ár
            if (chkPriceKeep.IsChecked == true)
                price = _material.PriceUnit;
            else if (!decimal.TryParse(txtPriceNew.Text, out price))
            {
                MessageBox.Show("Az új ár nem lehet üres és számnak kell lennie!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var updatedMaterial = new
            {
                materialId = _material.MaterialId,
                materialNumber = number,
                materialDescription = description,
                unit = unit,
                priceUnit = price
            };

            try
            {
                // Token ellenőrzés
                if (string.IsNullOrEmpty(Properties.Settings.Default.AccessToken))
                {
                    MessageBox.Show("Nincs bejelentkezett felhasználó (hiányzik a token).", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string json = JsonConvert.SerializeObject(updatedMaterial);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // 🔐 Token beállítása
                _httpClient.BaseAddress = new Uri("https://localhost:7055/api/");
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", Properties.Settings.Default.AccessToken);

                var response = await _httpClient.PutAsync($"Materials/{_material.MaterialId}", content);
                string serverResponse = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Sikeres módosítás!", "OK", MessageBoxButton.OK, MessageBoxImage.Information);

                    var adminWindow = Application.Current.Windows[0] as AdminDashboard;
                    if (adminWindow != null)
                    {
                        adminWindow.ContentFrame.Content = new MaterialsPage();
                    }

                    this.Close();
                }
                else
                {
                    MessageBox.Show($"Sikertelen módosítás!\nStatus: {response.StatusCode}\nSzerver válasza:\n{serverResponse}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba történt: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

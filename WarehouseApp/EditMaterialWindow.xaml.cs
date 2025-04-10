using System;
using System.Net.Http;
using System.Text;
using System.Windows;
using Newtonsoft.Json;
using WarehouseApp.Models;

namespace WarehouseApp.Windows.Popups
{
    public partial class EditMaterialWindow : Window
    {
        private readonly Material _material;
        private readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5118/api/") };

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
                string json = JsonConvert.SerializeObject(updatedMaterial);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"Materials/{_material.MaterialId}", content);
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
                    string serverResponse = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Sikertelen módosítás!\nSzerver válasza:\n{serverResponse}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void txtNumberNew_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void chkNumberKeep_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}

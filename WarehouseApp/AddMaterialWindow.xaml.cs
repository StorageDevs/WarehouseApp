using System;
using System.Net.Http;
using System.Text;
using System.Windows;
using Newtonsoft.Json;

namespace WarehouseApp.Windows.Popups
{
    public partial class AddMaterialWindow : Window
    {
        private readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5118/api/") };

        public AddMaterialWindow()
        {
            InitializeComponent();
        }

        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtNumber.Text, out int number) ||
                string.IsNullOrWhiteSpace(txtDescription.Text) ||
                string.IsNullOrWhiteSpace(txtUnit.Text) ||
                !decimal.TryParse(txtPrice.Text, out decimal price))
            {
                MessageBox.Show("Kérlek minden mezőt tölts ki helyesen!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var newMaterial = new
            {
                materialNumber = number,
                materialDescription = txtDescription.Text.Trim(),
                unit = txtUnit.Text.Trim(),
                priceUnit = price
            };

            try
            {
                string json = JsonConvert.SerializeObject(newMaterial);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("Materials", content);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Sikeres hozzáadás!", "OK", MessageBoxButton.OK, MessageBoxImage.Information);

                    var adminWindow = Application.Current.Windows[0] as AdminDashboard;
                    if (adminWindow != null)
                    {
                        adminWindow.ContentFrame.Content = new MaterialsPage();
                    }

                    this.Close();
                }
                else
                {
                    MessageBox.Show("Nem sikerült az anyag hozzáadása!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
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

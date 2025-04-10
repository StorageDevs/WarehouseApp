using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using WarehouseApp.Models;
using LocationModel = WarehouseApp.Models.Location;

namespace WarehouseApp.Windows.Popups
{
    public partial class StoreInWindow : Window
    {
        private readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5118/api/") };

        private readonly InventoryPage _inventoryPage;
        private List<LocationModel> _locations = new List<LocationModel>();
        private List<InventoryDisplayItem> _inventories = new List<InventoryDisplayItem>();
        private int _maxFreeSpace = 0;

        public StoreInWindow(InventoryPage inventoryPage)
        {
            InitializeComponent();
            _inventoryPage = inventoryPage;
            LoadData();
        }

        private async void LoadData()
        {
            try
            {
                var materialsResponse = await _httpClient.GetAsync("Materials");
                if (materialsResponse.IsSuccessStatusCode)
                {
                    var json = await materialsResponse.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ApiResponse<List<Material>>>(json);
                    cmbMaterials.ItemsSource = result.Result.OrderBy(m => m.MaterialDescription).ToList();
                }

                var locationsResponse = await _httpClient.GetAsync("Locations");
                if (locationsResponse.IsSuccessStatusCode)
                {
                    var json = await locationsResponse.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ApiResponse<List<LocationModel>>>(json);
                    _locations = result.Result;
                    cmbLocations.ItemsSource = _locations
                        .Where(loc => loc.LocationName != "Bevételezés" && loc.LocationName != "Kivezetés")
                        .OrderBy(l => l.LocationName)
                        .ToList();
                }

                // MOCK: Felhasználók betöltése amíg nincs auth
                var usersResponse = await _httpClient.GetAsync("Users");
                if (usersResponse.IsSuccessStatusCode)
                {
                    var json = await usersResponse.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<List<User>>(json);
                    cmbUsers.ItemsSource = result;
                    cmbUsers.DisplayMemberPath = "Username";
                }
                // /MOCK


                var inventoryResponse = await _httpClient.GetAsync("Inventories");
                if (inventoryResponse.IsSuccessStatusCode)
                {
                    var json = await inventoryResponse.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ApiResponse<List<InventoryDisplayItem>>>(json);
                    _inventories = result.Result;
                }

                numQuantity.Value = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba a betöltés során: " + ex.Message);
            }
        }

        private void cmbLocations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = cmbLocations.SelectedItem as LocationModel;
            if (selected == null) return;

            int used = _inventories
                .Where(i => i.LocationId == selected.LocationId)
                .Sum(i => i.Quantity);

            _maxFreeSpace = selected.LocationCapacity - used;
            _maxFreeSpace = Math.Max(_maxFreeSpace, 0);

            numQuantity.Maximum = _maxFreeSpace;
            numQuantity.Value = _maxFreeSpace > 0 ? 1 : 0;
            txtMaxCapacityLabel.Text = _maxFreeSpace > 0 ? $"(Szabad hely: {_maxFreeSpace})" : "(nincs hely)";
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            var selectedMaterial = cmbMaterials.SelectedItem as Material;
            var selectedLocation = cmbLocations.SelectedItem as LocationModel;
            // MOCK: Felhasználó lekérése a mock dropdownból
            var selectedUser = cmbUsers.SelectedItem as User;
            // /MOCK

            if (selectedMaterial == null || selectedLocation == null || selectedUser == null) // MOCK: "selectedUser == null"
            {
                MessageBox.Show("Kérlek válassz ki minden mezőt!");
                return;
            }

            int quantity = numQuantity.Value ?? 0;
            if (quantity <= 0 || quantity > _maxFreeSpace)
            {
                MessageBox.Show($"A mennyiség csak 1 és {_maxFreeSpace} közötti szám lehet.");
                return;
            }

            var payload = new
            {
                materialNumber = selectedMaterial.MaterialNumber,
                transactionFromLocationName = "Bevételezés",
                transactionToLocationName = selectedLocation.LocationName,
                transactedQty = quantity,
                userName = selectedUser.Username // MOCK
            };

            try
            {
                var json = JsonConvert.SerializeObject(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = _httpClient.PostAsync("Transactions", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Sikeres betárolás!", "OK", MessageBoxButton.OK, MessageBoxImage.Information);
                    _inventoryPage?.Filter();
                    this.Close();
                }
                else
                {
                    var errorText = response.Content.ReadAsStringAsync().Result;
                    MessageBox.Show("Hiba a szervertől: " + errorText);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba történt: " + ex.Message);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MaterialInfo_Click(object sender, RoutedEventArgs e)
        {
            var selected = cmbMaterials.SelectedItem as Material;
            if (selected != null)
            {
                MessageBox.Show(
                    $"ID: {selected.MaterialId}\n" +
                    $"Szám: {selected.MaterialNumber}\n" +
                    $"Leírás: {selected.MaterialDescription}\n" +
                    $"Egység: {selected.Unit}\n" +
                    $"Ár: {selected.PriceUnit}",
                    "Anyag információ");
            }
        }

        private void LocationInfo_Click(object sender, RoutedEventArgs e)
        {
            var selected = cmbLocations.SelectedItem as LocationModel;
            if (selected != null)
            {
                MessageBox.Show(
                    $"ID: {selected.LocationId}\n" +
                    $"Név: {selected.LocationName}\n" +
                    $"Leírás: {selected.LocationDescription}\n" +
                    $"Kapacitás: {selected.LocationCapacity}",
                    "Tárhely információ");
            }
        }

        public class ApiResponse<T>
        {
            [JsonProperty("result")]
            public T Result { get; set; }
        }
    }
}

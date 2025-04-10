using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WarehouseApp.Models;
using LocationModel = WarehouseApp.Models.Location;

namespace WarehouseApp.Windows.Popups
{
    public partial class StoreOutWindow : Window
    {
        private readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5118/api/") };
        private readonly InventoryPage _inventoryPage;
        private readonly InventoryDisplayItem _preselected;

        private List<InventoryDisplayItem> _inventories = new List<InventoryDisplayItem>();
        private List<Material> _allMaterials = new List<Material>();
        private List<LocationModel> _allLocations = new List<LocationModel>();
        private bool _suppressMaterialEvent = false;
        private bool _suppressLocationEvent = false;
        private int _maxQuantity = 0;

        public StoreOutWindow(InventoryPage inventoryPage, InventoryDisplayItem preselected = null)
        {
            InitializeComponent();
            _inventoryPage = inventoryPage;
            _preselected = preselected;
            LoadData();
        }

        private bool IsValidLocation(LocationModel loc)
        {
            return loc.LocationName != "Bevételezés" && loc.LocationName != "Kivezetés";
        }

        private async void LoadData()
        {
            try
            {
                var inventoryResponse = await _httpClient.GetAsync("Inventories");
                if (inventoryResponse.IsSuccessStatusCode)
                {
                    var json = await inventoryResponse.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ApiResponse<List<InventoryDisplayItem>>>(json);
                    _inventories = result.Result;
                }

                var materialsResponse = await _httpClient.GetAsync("Materials");
                if (materialsResponse.IsSuccessStatusCode)
                {
                    var json = await materialsResponse.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ApiResponse<List<Material>>>(json);
                    _allMaterials = result.Result.OrderBy(m => m.MaterialDescription).ToList();
                    _allMaterials.Insert(0, new Material { MaterialId = -1, MaterialDescription = "-" });
                    cmbMaterials.ItemsSource = _allMaterials;
                }

                var locationsResponse = await _httpClient.GetAsync("Locations");
                if (locationsResponse.IsSuccessStatusCode)
                {
                    var json = await locationsResponse.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ApiResponse<List<LocationModel>>>(json);
                    _allLocations = result.Result
                        .Where(IsValidLocation)
                        .OrderBy(l => l.LocationName)
                        .ToList();
                    _allLocations.Insert(0, new LocationModel { LocationId = -1, LocationName = "-" });
                    cmbLocations.ItemsSource = _allLocations;
                }

                // MOCK: Felhasználók betöltése amíg nincs auth rendszer
                var usersResponse = await _httpClient.GetAsync("Users");
                if (usersResponse.IsSuccessStatusCode)
                {
                    var json = await usersResponse.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<List<User>>(json);
                    cmbUsers.ItemsSource = result;
                    cmbUsers.DisplayMemberPath = "Username";
                }
                // /MOCK


                if (_preselected != null)
                {
                    var selectedMaterial = _allMaterials.FirstOrDefault(m => m.MaterialId == _preselected.MaterialId);
                    if (selectedMaterial != null) cmbMaterials.SelectedItem = selectedMaterial;

                    var selectedLocation = _allLocations.FirstOrDefault(l => l.LocationId == _preselected.LocationId);
                    if (selectedLocation != null) cmbLocations.SelectedItem = selectedLocation;
                }
                else
                {
                    cmbMaterials.SelectedIndex = 0;
                    cmbLocations.SelectedIndex = 0;
                }

                UpdateMaxQuantity();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba a betöltés során: " + ex.Message);
            }
        }

        private void UpdateMaxQuantity()
        {
            var selectedMaterial = cmbMaterials.SelectedItem as Material;
            var selectedLocation = cmbLocations.SelectedItem as LocationModel;

            if (selectedMaterial == null || selectedMaterial.MaterialId == -1 ||
                selectedLocation == null || selectedLocation.LocationId == -1)
            {
                _maxQuantity = 0;
                numQuantity.Maximum = 0;
                numQuantity.Value = 0;
                return;
            }

            var match = _inventories.FirstOrDefault(i =>
                i.MaterialId == selectedMaterial.MaterialId &&
                i.LocationId == selectedLocation.LocationId);

            _maxQuantity = match?.Quantity ?? 0;
            numQuantity.Maximum = _maxQuantity;
            numQuantity.Value = _maxQuantity > 0 ? 1 : 0;
            txtMaxQuantityLabel.Text = _maxQuantity > 0 ? $"(Tárhelyen: {_maxQuantity})" : "";
        }

        private void MaterialComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_suppressMaterialEvent) return;
            _suppressLocationEvent = true;

            var selectedMaterial = cmbMaterials.SelectedItem as Material;
            var currentLocation = cmbLocations.SelectedItem as LocationModel;

            if (selectedMaterial != null && selectedMaterial.MaterialId != -1)
            {
                var filteredLocationIds = _inventories
                    .Where(i => i.MaterialId == selectedMaterial.MaterialId)
                    .Select(i => i.LocationId)
                    .Distinct()
                    .ToList();

                var newList = _allLocations
                    .Where(l => l.LocationId == -1 || filteredLocationIds.Contains(l.LocationId))
                    .Where(IsValidLocation)
                    .OrderBy(l => l.LocationName)
                    .ToList();

                cmbLocations.ItemsSource = newList;
                if (currentLocation != null && newList.Any(l => l.LocationId == currentLocation.LocationId))
                    cmbLocations.SelectedItem = currentLocation;
                else
                    cmbLocations.SelectedIndex = 0;
            }
            else
            {
                cmbLocations.ItemsSource = _allLocations;
                if (currentLocation != null && _allLocations.Any(l => l.LocationId == currentLocation.LocationId))
                    cmbLocations.SelectedItem = currentLocation;
                else
                    cmbLocations.SelectedIndex = 0;
            }

            _suppressLocationEvent = false;
            UpdateMaxQuantity();
        }

        private void LocationComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_suppressLocationEvent) return;
            _suppressMaterialEvent = true;

            var selectedLocation = cmbLocations.SelectedItem as LocationModel;
            var currentMaterial = cmbMaterials.SelectedItem as Material;

            if (selectedLocation != null && selectedLocation.LocationId != -1)
            {
                var filteredMaterialIds = _inventories
                    .Where(i => i.LocationId == selectedLocation.LocationId)
                    .Select(i => i.MaterialId)
                    .Distinct()
                    .ToList();

                var newList = _allMaterials
                    .Where(m => m.MaterialId == -1 || filteredMaterialIds.Contains(m.MaterialId))
                    .OrderBy(m => m.MaterialDescription)
                    .ToList();

                cmbMaterials.ItemsSource = newList;
                if (currentMaterial != null && newList.Any(m => m.MaterialId == currentMaterial.MaterialId))
                    cmbMaterials.SelectedItem = currentMaterial;
                else
                    cmbMaterials.SelectedIndex = 0;
            }
            else
            {
                cmbMaterials.ItemsSource = _allMaterials;
                if (currentMaterial != null && _allMaterials.Any(m => m.MaterialId == currentMaterial.MaterialId))
                    cmbMaterials.SelectedItem = currentMaterial;
                else
                    cmbMaterials.SelectedIndex = 0;
            }

            _suppressMaterialEvent = false;
            UpdateMaxQuantity();
        }

        private async void Submit_Click(object sender, RoutedEventArgs e)
        {
            var selectedMaterial = cmbMaterials.SelectedItem as Material;
            var selectedLocation = cmbLocations.SelectedItem as LocationModel;
            // MOCK: Felhasználó kiválasztása mock dropdownból
            var selectedUser = cmbUsers.SelectedItem as User;
            // /MOCK

            if (selectedMaterial == null || selectedMaterial.MaterialId == -1 ||
                selectedLocation == null || selectedLocation.LocationId == -1 || selectedUser == null)
            {
                MessageBox.Show("Kérlek válassz ki minden mezőt!");
                return;
            }

            int quantity = numQuantity.Value ?? 0;
            if (quantity <= 0 || quantity > _maxQuantity)
            {
                MessageBox.Show($"A mennyiség csak 1 és {_maxQuantity} közötti szám lehet.");
                return;
            }

            var payload = new
            {
                materialNumber = selectedMaterial.MaterialNumber,
                transactionFromLocationName = selectedLocation.LocationName,
                transactionToLocationName = "Kivezetés",
                transactedQty = quantity,
                userName = selectedUser.Username // MOCK
            };

            try
            {
                var json = JsonConvert.SerializeObject(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("Transactions", content);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Sikeres kitárolás!", "OK", MessageBoxButton.OK, MessageBoxImage.Information);
                    _inventoryPage?.Filter();
                    this.Close();
                }
                else
                {
                    var errorText = await response.Content.ReadAsStringAsync();
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
            if (selected == null || selected.MaterialId == -1) return;

            MessageBox.Show(
                $"ID: {selected.MaterialId}\n" +
                $"Szám: {selected.MaterialNumber}\n" +
                $"Leírás: {selected.MaterialDescription}\n" +
                $"Egység: {selected.Unit}\n" +
                $"Ár: {selected.PriceUnit}",
                "Anyag információ");
        }

        private void LocationInfo_Click(object sender, RoutedEventArgs e)
        {
            var selected = cmbLocations.SelectedItem as LocationModel;
            if (selected == null || selected.LocationId == -1) return;

            MessageBox.Show(
                $"ID: {selected.LocationId}\n" +
                $"Név: {selected.LocationName}\n" +
                $"Leírás: {selected.LocationDescription}\n" +
                $"Kapacitás: {selected.LocationCapacity}",
                "Tárhely információ");
        }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WarehouseApp.Models;
using WarehouseApp.Windows.Popups;

namespace WarehouseApp
{
    public partial class InventoryPage : Page
    {
        private readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5118/api/") };
        public List<InventoryDisplayItem> Inventories { get; set; } = new List<InventoryDisplayItem>();

        public InventoryPage()
        {
            InitializeComponent();
            DataContext = this;

            txtFilterMaterial.TextChanged += (s, e) => Filter();
            txtFilterLocation.TextChanged += (s, e) => Filter();

            _ = LoadInventories();
        }

        private async Task LoadInventories(int? materialId = null, int? locationId = null)
        {
            try
            {
                string query;

                if (!materialId.HasValue && !locationId.HasValue)
                {
                    query = "Inventories"; // teljes lekérdezés
                }
                else
                {
                    query = "Inventories/CustomQuery";
                    List<string> queryParams = new List<string>();
                    if (materialId.HasValue) queryParams.Add($"materialId={materialId.Value}");
                    if (locationId.HasValue) queryParams.Add($"locationId={locationId.Value}");
                    query += "?" + string.Join("&", queryParams);
                }

                var response = await _httpClient.GetAsync(query);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<ApiResponse<List<InventoryDisplayItem>>>(json);

                    Inventories = data.Result ?? new List<InventoryDisplayItem>();
                    InventoryList.ItemsSource = Inventories;
                }
                else
                {
                    InventoryList.ItemsSource = new List<InventoryDisplayItem>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba történt a lekérdezés közben: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void Filter()
        {
            int? materialId = int.TryParse(txtFilterMaterial.Text, out int matId) ? matId : (int?)null;
            int? locationId = int.TryParse(txtFilterLocation.Text, out int locId) ? locId : (int?)null;

            _ = LoadInventories(materialId, locationId);
        }

        private void StoreIn_Click(object sender, RoutedEventArgs e)
        {
            var storeWindow = new StoreInWindow(this);
            storeWindow.ShowDialog();
        }

        private void StoreOut_Click(object sender, RoutedEventArgs e)
        {
            var selectedItems = InventoryList.SelectedItems.Cast<InventoryDisplayItem>().ToList();

            if (selectedItems.Count == 0)
            {
                var window = new StoreOutWindow(this);
                window.ShowDialog();
            }
            else
            {
                foreach (var item in selectedItems)
                {
                    var window = new StoreOutWindow(this, item);
                    window.ShowDialog();
                }
            }
        }

        private void Transfer_Click(object sender, RoutedEventArgs e)
        {
            var selectedItems = InventoryList.SelectedItems.Cast<InventoryDisplayItem>().ToList();

            if (selectedItems.Count == 0)
            {
                var window = new TransferWindow(this);
                window.ShowDialog();
            }
            else
            {
                foreach (var item in selectedItems)
                {
                    var window = new TransferWindow(this, item);
                    window.ShowDialog();
                }
            }
        }


        private void TransferHistory_click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.Windows[0] as AdminDashboard;
            if (mainWindow != null)
            {
                mainWindow.ContentFrame.Content = new TransactionsPage();
            }
        }
    }

    public class ApiResponse<T>
    {
        [JsonProperty("result")]
        public T Result { get; set; }
    }
}

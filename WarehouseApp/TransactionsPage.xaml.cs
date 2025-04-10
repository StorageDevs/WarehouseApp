using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Threading.Tasks;
using WarehouseApp.Models;

namespace WarehouseApp
{
    public partial class TransactionsPage : Page
    {
        private readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5118/api/") };

        public TransactionsPage()
        {
            InitializeComponent();
            _ = LoadTransactions();
        }

        private async Task LoadTransactions()
        {
            try
            {
                var response = await _httpClient.GetAsync("Transactions");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<ApiResponse<List<TransactionDisplayItem>>>(json);

                    // Rendezés dátum szerint csökkenő sorrendbe
                    var sorted = (data.Result ?? new List<TransactionDisplayItem>())
                                 .OrderByDescending(t => t.TransferDate)
                                 .ToList();

                    TransactionsList.ItemsSource = sorted;
                }
                else
                {
                    MessageBox.Show("Nem sikerült betölteni a tranzakciókat!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba történt a lekérés közben: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BlockTransactions_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Mozgatások blokkolása funkció teszt!", "Info");
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.Windows[0] as AdminDashboard;
            if (mainWindow != null)
            {
                mainWindow.ContentFrame.Content = new InventoryPage();
            }
        }


        public class ApiResponse<T>
        {
            [JsonProperty("result")]
            public T Result { get; set; }
        }
    }
}

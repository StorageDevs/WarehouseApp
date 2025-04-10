using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using WarehouseApp.Models;
using WarehouseApp.Windows.Popups;

namespace WarehouseApp
{
    public partial class MaterialsPage : Page
    {
        private readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5118/api/") };
        public ObservableCollection<Material> Materials { get; set; }

        public MaterialsPage()
        {
            InitializeComponent();
            Materials = new ObservableCollection<Material>();
            DataContext = this;
            _ = LoadMaterials();
        }

        private async Task LoadMaterials()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("Materials");
                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();

                    var jsonObject = JsonConvert.DeserializeObject<dynamic>(responseData);
                    if (jsonObject.result != null)
                    {
                        string resultJson = JsonConvert.SerializeObject(jsonObject.result);
                        var materials = JsonConvert.DeserializeObject<ObservableCollection<Material>>(resultJson);

                        Materials.Clear();
                        foreach (var material in materials)
                        {
                            Materials.Add(material);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Hiba: Az API válasz nem tartalmaz 'result' mezőt.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba történt az anyagok betöltésekor: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddMaterial_Click(object sender, RoutedEventArgs e)
        {
            var popup = new AddMaterialWindow();
            popup.ShowDialog();
            _ = LoadMaterials();
        }

        private async void RemoveMaterial_Click(object sender, RoutedEventArgs e)
        {
            if (MaterialsListView.SelectedItems.Count == 0)
            {
                MessageBox.Show("Kérlek válassz ki legalább egy anyagot törléshez.", "Nincs kijelölés", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selectedMaterials = MaterialsListView.SelectedItems.Cast<Material>().ToList();

            foreach (var selected in selectedMaterials)
            {
                var confirm = MessageBox.Show(
                    $"Biztosan törlöd ezt az anyagot?\n\nSzám: {selected.MaterialNumber}\nLeírás: {selected.MaterialDescription}\nEgység: {selected.Unit}\nÁr: {selected.PriceUnit}",
                    "Megerősítés", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (confirm == MessageBoxResult.Yes)
                {
                    try
                    {
                        HttpResponseMessage response = await _httpClient.DeleteAsync($"Materials/{selected.MaterialId}");
                        if (response.IsSuccessStatusCode)
                        {
                            Materials.Remove(selected);
                        }
                        else
                        {
                            MessageBox.Show("Nem sikerült törölni az anyagot!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Hiba történt törlés közben: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void EditMaterial_Click(object sender, RoutedEventArgs e)
        {
            if (MaterialsListView.SelectedItem is Material selected)
            {
                var popup = new EditMaterialWindow(selected);
                popup.ShowDialog();
                _ = LoadMaterials();
            }
            else
            {
                MessageBox.Show("Kérlek válassz ki egy anyagot módosításhoz.", "Nincs kijelölés", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}

using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using WarehouseApp.Models;
using WarehouseApp.Windows.Popups;
using System.Linq;

namespace WarehouseApp
{
    public partial class LocationsPage : Page
    {
        private readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5118/api/Locations/") };
        public ObservableCollection<Location> Locations { get; set; }

        public LocationsPage()
        {
            InitializeComponent();
            Locations = new ObservableCollection<Location>();
            DataContext = this;
            _ = LoadLocations();
        }

        private async Task LoadLocations()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("");
                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();

                    var jsonObject = JsonConvert.DeserializeObject<dynamic>(responseData);
                    if (jsonObject.result != null)
                    {
                        string resultJson = JsonConvert.SerializeObject(jsonObject.result);
                        var locations = JsonConvert.DeserializeObject<ObservableCollection<Location>>(resultJson);

                        Locations.Clear();
                        foreach (var location in locations)
                        {
                            if (location.LocationID > 2)
                            {
                                Locations.Add(location);
                            }
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
                MessageBox.Show($"Hiba történt a helyszínek betöltésekor: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddLocation_Click(object sender, RoutedEventArgs e)
        {
            var popup = new AddLocationWindow();
            popup.ShowDialog();
        }

        private async void RemoveLocation_Click(object sender, RoutedEventArgs e)
        {
            var selectedLocations = LocationList.SelectedItems.Cast<Location>().ToList();

            if (selectedLocations.Count == 0)
            {
                MessageBox.Show("Kérlek jelölj ki legalább egy helyszínt!", "Figyelmeztetés", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            foreach (var location in selectedLocations.ToList()) // ToList(), mert módosítjuk
            {
                var confirm = MessageBox.Show(
                    $"Biztosan törölni szeretnéd ezt a helyszínt?\n\n" +
                    $"ID: {location.LocationID}\n" +
                    $"Név: {location.LocationName}\n" +
                    $"Leírás: {location.LocationDescription}\n" +
                    $"Kapacitás: {location.LocationCapacity}",
                    "Törlés megerősítése",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (confirm != MessageBoxResult.Yes)
                    continue;

                try
                {
                    var response = await _httpClient.DeleteAsync($"{location.LocationID}");
                    if (response.IsSuccessStatusCode)
                    {
                        Locations.Remove(location);
                    }
                    else
                    {
                        MessageBox.Show($"Nem sikerült törölni: {location.LocationName}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Hiba történt: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }



        private void EditLocation_Click(object sender, RoutedEventArgs e)
        {
            var selectedItems = LocationList.SelectedItems.Cast<Location>().ToList();

            foreach (var location in selectedItems)
            {
                var popup = new EditLocationWindow(location);
                popup.ShowDialog();
            }

            _ = LoadLocations();
        }
    }

    public class Location
    {
        public int LocationID { get; set; }
        public string LocationName { get; set; }
        public string LocationDescription { get; set; }
        public int LocationCapacity { get; set; }
    }
}

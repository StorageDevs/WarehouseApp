using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using WarehouseApp.Models;


namespace WarehouseApp.Windows.Pages
{
    public partial class UsersPage : Page
    {
        private readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5118/api/") };
        public ObservableCollection<User> Users { get; set; }

        public UsersPage()
        {
            InitializeComponent();
            Users = new ObservableCollection<User>();
            DataContext = this;
            _ = LoadUsers();
        }

        private async Task LoadUsers()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("Users");
                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    var users = JsonConvert.DeserializeObject<ObservableCollection<User>>(responseData);

                    // Frissítjük a listát
                    Users.Clear();
                    foreach (var user in users)
                    {
                        Users.Add(user); // Az új DisplayRole automatikusan generálódik
                    }
                }
                else
                {
                    MessageBox.Show("Nem sikerült betölteni a felhasználókat!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba történt: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Add User gomb megnyomva!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Edit User gomb megnyomva!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Delete User gomb megnyomva!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}

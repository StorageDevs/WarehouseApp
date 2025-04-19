using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using System.Linq;
using WarehouseApp.Models;
using WarehouseApp.Windows.Popups;


namespace WarehouseApp.Windows.Pages
{
    public partial class UsersPage : Page
    {
        private readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7188/") };
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
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", Properties.Settings.Default.AccessToken);

                HttpResponseMessage response = await _httpClient.GetAsync("auth/GetAllUser");
                string responseData = await response.Content.ReadAsStringAsync();

                var jsonObject = JsonConvert.DeserializeObject<dynamic>(responseData);
                if (jsonObject.result != null)
                {
                    string resultJson = JsonConvert.SerializeObject(jsonObject.result);
                    var users = JsonConvert.DeserializeObject<ObservableCollection<User>>(resultJson);

                    Users.Clear();
                    foreach (var user in users)
                    {
                        Users.Add(user);
                    }
                }
                else
                {
                    MessageBox.Show("A válasz nem tartalmazott 'result' mezőt.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba történt: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", Properties.Settings.Default.AccessToken);

            var selectedUsers = UserList.SelectedItems.Cast<User>().ToList();

            if (selectedUsers.Count == 0)
            {
                MessageBox.Show("Kérlek jelölj ki legalább egy felhasználót!", "Figyelmeztetés", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            foreach (var user in selectedUsers.ToList())
            {
                var confirm = MessageBox.Show(
                    $"Biztosan törölni szeretnéd ezt a felhasználót?\n\n" +
                    $"Felhasználónév: {user.UserName}\n" +
                    $"Teljes név: {user.FullName}\n" +
                    $"Email: {user.Email}\n" +
                    $"Szerepkör: {user.DisplayRole}",
                    "Törlés megerősítése",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (confirm != MessageBoxResult.Yes)
                    continue;

                var confirmSecond = MessageBox.Show("Biztos vagy benne?", "Végső megerősítés", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (confirmSecond != MessageBoxResult.Yes)
                    continue;

                try
                {
                    HttpResponseMessage response = await _httpClient.DeleteAsync($"auth/DeleteUser/{user.UserID}");

                    if (response.IsSuccessStatusCode)
                    {
                        Users.Remove(user);
                        MessageBox.Show($"Felhasználó törölve: {user.UserName}", "Siker", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        string errorMsg = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Nem sikerült törölni: {user.UserName}\n\nSzerver válasza: {errorMsg}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Hiba történt: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            var addUserWindow = new AddUserWindow();
            addUserWindow.ShowDialog(); // Megnyitja az AddUserWindow ablakot
        }
    }
}

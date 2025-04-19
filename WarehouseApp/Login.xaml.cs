using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using WarehouseApp.Windows.Popups;

namespace WarehouseApp
{
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            var loginData = new
            {
                userName = username,
                password = password
            };

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7188/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var json = JsonConvert.SerializeObject(loginData);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync("auth/Login", content);
                    string responseData = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        dynamic result = JsonConvert.DeserializeObject(responseData);
                        string token = result.token;

                        if (!string.IsNullOrEmpty(token))
                        {
                            // Token mentése
                            Properties.Settings.Default.AccessToken = token;

                            // 🔐 GetAllUser lekérés a userID, role, név stb. megszerzésére
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                            HttpResponseMessage usersResponse = await client.GetAsync("auth/GetAllUser");

                            if (usersResponse.IsSuccessStatusCode)
                            {
                                string usersJson = await usersResponse.Content.ReadAsStringAsync();
                                dynamic usersResult = JsonConvert.DeserializeObject(usersJson);

                                foreach (var user in usersResult.result)
                                {
                                    if ((string)user.userName == username)
                                    {
                                        var roles = user.role.ToObject<List<string>>();
                                        bool isAdmin = roles.Contains("admin");

                                        if (!isAdmin)
                                        {
                                            MessageBox.Show("Csak admin felhasználók férhetnek hozzá az alkalmazáshoz!", "Hozzáférés megtagadva", MessageBoxButton.OK, MessageBoxImage.Warning);
                                            return;
                                        }

                                        Properties.Settings.Default.UserID = user.userID;
                                        Properties.Settings.Default.UserName = username;
                                        Properties.Settings.Default.FullName = user.fullName;

                                        // ⚠️ Kötelező jelszócsere ha TempPassword van
                                        if (password == "TempPassword-123")
                                        {
                                            MessageBox.Show("Ideiglenes jelszóval léptél be. Kérlek változtasd meg a jelszavadat!", "Biztonsági figyelmeztetés", MessageBoxButton.OK, MessageBoxImage.Information);
                                            var changePasswordWindow = new FirstLoginWindow(username, user.userID.ToString());
                                            bool? resultChange = changePasswordWindow.ShowDialog();

                                            if (resultChange == true)
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                MessageBox.Show("Nem változtattad meg a jelszavad, ezért a belépés megszakadt.", "Hozzáférés megtagadva", MessageBoxButton.OK, MessageBoxImage.Warning);
                                                return;
                                            }
                                        }
                                        break;
                                    }
                                }

                                Properties.Settings.Default.Save();

                                // Navigáció
                                AdminDashboard adminWindow = new AdminDashboard();
                                adminWindow.Show();
                                this.Close();
                            }
                            else
                            {
                                MessageBox.Show("Hiba a felhasználók lekérdezésekor!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Hibás felhasználónév vagy jelszó!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Bejelentkezés sikertelen! {(int)response.StatusCode} - {response.StatusCode}\n{responseData}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba történt a bejelentkezéskor: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txtPassword_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                btnLogin_Click(sender, e);
            }
        }
    }
}

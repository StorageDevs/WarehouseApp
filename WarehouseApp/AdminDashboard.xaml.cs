using System.Windows;
using System.Windows.Controls;
using WarehouseApp.Windows.Pages;



namespace WarehouseApp
{
    public partial class AdminDashboard : Window
    {
        private string _userName;

        public AdminDashboard(string userName)
        {
            InitializeComponent();
            _userName = userName;

            // Itt adjuk át a nevet a DashboardPage-nek
            ContentFrame.Content = new DashboardPage(_userName);
        }

        private void BtnDashboard_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = new DashboardPage(_userName); // A gombnál is átadjuk
        }

        private void BtnInventory_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = new InventoryPage();
        }

        private void BtnMaterials_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = new MaterialsPage();
        }

        private void BtnLocations_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = new LocationsPage();
        }

        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = new SettingsPage();
        }

        private void BtnUsers_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = new UsersPage();
        }
    }
}
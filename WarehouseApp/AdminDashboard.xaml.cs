using System.Windows;
using System.Windows.Controls;
using WarehouseApp.Windows.Pages;



namespace WarehouseApp
{
    public partial class AdminDashboard : Window
    {
        public AdminDashboard()
        {
            InitializeComponent();
            ContentFrame.Content = new DashboardPage();
        }

        private void BtnDashboard_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = new DashboardPage();
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

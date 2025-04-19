using System.Windows.Controls;

namespace WarehouseApp
{
    public partial class DashboardPage : Page
    {
        public string CurrentFullName { get; set; }

        public DashboardPage()
        {
            InitializeComponent();

            // Settingsből olvassuk ki a bejelentkezett felhasználó teljes nevét
            CurrentFullName = Properties.Settings.Default.FullName;

            DataContext = this;
        }
    }
}

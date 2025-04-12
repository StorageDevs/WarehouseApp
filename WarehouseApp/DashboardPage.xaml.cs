using System.Windows.Controls;

namespace WarehouseApp
{
    public partial class DashboardPage : Page
    {
        public string CurrentUserName { get; set; }

        public DashboardPage(string userName)
        {
            InitializeComponent();
            CurrentUserName = userName;

            // Ez kell, hogy a Binding működjön
            DataContext = this;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseApp.Models
{
    public class User
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public List<string> Role { get; set; }

        public string DisplayRole => Role != null && Role.Count > 0 ? Role[0] : "N/A";
    }
}

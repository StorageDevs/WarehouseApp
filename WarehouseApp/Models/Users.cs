using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseApp.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public int Role { get; set; } // Eredeti numerikus szerepkör
        public string DisplayRole // Új tulajdonság a szöveges szerepkörhöz
        {
            get
            {
                if (Role == 1) return "user";
                if (Role == 2) return "superuser";
                if (Role == 3) return "admin";
                return "unknown";
            }
        }
    }

}

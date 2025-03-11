using System;
using System.Collections.Generic;

namespace WarehouseBackend.Models;

public partial class Roletype
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public string RoleDescription { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}

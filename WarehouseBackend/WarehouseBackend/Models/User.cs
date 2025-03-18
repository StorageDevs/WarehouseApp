using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WarehouseBackend.Models;

public partial class User
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Salt { get; set; } = null!;

    public string Hash { get; set; } = null!;

    public int Role { get; set; }

    [JsonIgnore]

    public virtual Roletype RoleNavigation { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}

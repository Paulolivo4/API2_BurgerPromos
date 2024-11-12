using System;
using System.Collections.Generic;

namespace API2.Data.Models;

public partial class Burger
{
    public int BurgerId { get; set; }

    public string BurgerName { get; set; } = null!;

    public bool WithCheese { get; set; }

    public decimal Price { get; set; }

    public decimal PriceCeiling { get; set; }

    public virtual ICollection<Promo> Promos { get; set; } = new List<Promo>();
}

using System;

namespace S2Search.Backend.Domain.Customer.Models;

public class SearchInsight
{
    public string DataCategory { get; set; }
    public string DataPoint { get; set; }
    public int Count { get; set; }
    public DateTime Date { get; set; }
}

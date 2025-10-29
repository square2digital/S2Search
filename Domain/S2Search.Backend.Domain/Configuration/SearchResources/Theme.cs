using System;

namespace S2Search.Backend.Domain.Configuration.SearchResources;

public class Theme
{
    public Guid id { get; set; }
    public string primary_hex_colour { get; set; }
    public string secondary_hex_colour { get; set; }
    public string nav_bar_hex_colour { get; set; }
    public string logo_url { get; set; }
    public string missing_image_url { get; set; }
    public Guid customer_id { get; set; }
    public Guid search_index_id { get; set; }
    public DateTime created_date { get; set; }
    public DateTime modified_date { get; set; }
}

using Norison.TransactionSync.Persistence.Attributes;

using Notion.Client;

namespace Norison.TransactionSync.Persistence.Storages.Models;

public class AccountDbModel : IDbModel
{
    public string? Id { get; set; }
    public string? IconUrl { get; set; }

    [NotionProperty("Name", PropertyType.Title)]
    public string? Name { get; set; }
    
    [NotionProperty("Ticker", PropertyType.Checkbox)]
    public bool? Ticker { get; set; }
    
    [NotionProperty("Price", PropertyType.Number)]
    public decimal? Price { get; set; }
}
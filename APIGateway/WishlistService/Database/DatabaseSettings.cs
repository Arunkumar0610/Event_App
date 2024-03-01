namespace WishlistService.Database
{
    public class DatabaseSettings:IDatabaseSettings
    {
        public required string ConnectionString { get; set; }
        public required string DatabaseName { get; set; }
        public required string CollectionName { get; set; }
    }
}

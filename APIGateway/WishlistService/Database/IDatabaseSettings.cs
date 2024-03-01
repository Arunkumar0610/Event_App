namespace WishlistService.Database
{
    public interface IDatabaseSettings
    {
        public  string ConnectionString { get; set; }
        public  string DatabaseName { get; set; }
        public  string CollectionName { get; set; }
    }
}

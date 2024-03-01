namespace WishlistService.Exceptions
{
    public class WishlistNotFoundException:ApplicationException
    {
        public WishlistNotFoundException() { }
        public WishlistNotFoundException(string message) : base(message) { }
    }
}

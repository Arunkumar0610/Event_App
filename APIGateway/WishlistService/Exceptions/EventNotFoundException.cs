namespace WishlistService.Exceptions
{
    public class EventNotFoundException:ApplicationException
    {
        public EventNotFoundException() { }
        public EventNotFoundException(string message):base(message) { }
    }
}

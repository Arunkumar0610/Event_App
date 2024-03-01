namespace EventService.Exceptions
{
    public class FetchingEventsException:ApplicationException
    {
        public FetchingEventsException(){}
        public FetchingEventsException(string message) : base(message) { }
    }
}

namespace UserProfileService.Exceptions
{
    public class KafkaProducerException:ApplicationException
    {
        public KafkaProducerException() { }
        public KafkaProducerException(string message) : base(message) { }
    }
}

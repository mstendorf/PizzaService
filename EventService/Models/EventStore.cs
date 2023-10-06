namespace EventService.Models
{
    public interface IEventStore
    {
        void RaiseEvent(Event e);
        IEnumerable<StoredEvent> GetEvents(int startIndex, int endIndex);
    }

    public class EventStore : IEventStore
    {
        private static long currentSequenceNumber = 0;
        private static readonly List<StoredEvent> events = new List<StoredEvent>();

        public void RaiseEvent(Event e)
        {
            var sequenceNumber = Interlocked.Increment(ref currentSequenceNumber);
            var storedEvent = new StoredEvent(sequenceNumber, e);
            events.Add(storedEvent);
        }

        public IEnumerable<StoredEvent> GetEvents(int startIndex, int endIndex)
        {
            return events.Where(e => e.Id >= startIndex && e.Id <= endIndex).OrderBy(e => e.Id);
        }
    }
}

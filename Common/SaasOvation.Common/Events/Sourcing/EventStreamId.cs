namespace SaasOvation.Common.Events.Sourcing {
    public class EventStreamId {
        public string StreamName { get; private set; }
        public int StreamVersion { get; private set; }

        public EventStreamId(string streamName): this(streamName, 1) {
        }

        public EventStreamId(string streamName, int streamVersion) {
            this.StreamName = streamName;
            this.StreamVersion = streamVersion;
        }

        public EventStreamId(string streamNameSegment1, string streamNameSegment2)
            : this(streamNameSegment1, streamNameSegment2, 1) {}

        public EventStreamId(string streamNameSegment1, string streamNameSegment2, int streamVersion)
            : this(streamNameSegment1 + ":" + streamNameSegment2, streamVersion) {}

        public EventStreamId WithStreamVersion(int streamVersion) {
            return new EventStreamId(this.StreamName, streamVersion);
        }
    }
}
using System;
using Newtonsoft.Json;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.Common.Events {
    public class EventSerializer {
        private readonly bool _isPretty;

        private static readonly Lazy<EventSerializer> _instance = new Lazy<EventSerializer>(
            () => new EventSerializer(), true);

        public static EventSerializer Instance {
            get { return _instance.Value; }
        }

        public EventSerializer(bool isPretty = false) {
            this._isPretty = isPretty;
        }

        public T Deserialize<T>(string serialization) {
            return JsonConvert.DeserializeObject<T>(serialization);
        }

        public object Deserialize(string serialization, Type type) {
            return JsonConvert.DeserializeObject(serialization, type);
        }

        public string Serialize(IDomainEvent domainEvent) {
            return JsonConvert.SerializeObject(domainEvent, this._isPretty ? Formatting.Indented : Formatting.None);
        }
    }
}
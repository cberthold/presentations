using System.Threading;
using System.Threading.Tasks;
using CQRSlite.Events;
using Newtonsoft.Json;
using SqlStreamStore.Streams;

namespace System
{
    public static class StreamMessageExtensions
    {

        private static readonly JsonSerializerSettings SETTINGS = new JsonSerializerSettings {
            TypeNameHandling = TypeNameHandling.None,
        };

        public static async Task<IEvent> RehydrateEvent(this StreamMessage message, CancellationToken cancellationToken)
        {
            var jsonData = await message.GetJsonData(cancellationToken);
            return (IEvent)DeserializeData(jsonData, message.Type);
        } 

        public static async Task<object> RehydrateEventObject(this StreamMessage message, CancellationToken cancellationToken = default(CancellationToken))
        {
            var jsonData = await message.GetJsonData(cancellationToken);
            return DeserializeData(jsonData, message.Type);
        } 
        
        public static string SerializeData<TObject>(this TObject obj)
        {
            return JsonConvert.SerializeObject(obj, SETTINGS);
        }

        public static object DeserializeData(string data, string typeString)
        {
            var type = Type.GetType(typeString);
            return JsonConvert.DeserializeObject(data, type, SETTINGS);
        }

    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxApp.Common.Serialization
{
    public interface IJsonObjectSerializer
    {
        void CreateJsonToStream<T>(Stream stream, T objectToSerialize, bool preserveReferences);
        string CreateJsonString<T>(T objectToSerialize);
        T DeserializeJsonString<T>(string jsonString);
        T DeserializeJsonStream<T>(Stream stream);
        object DeserializeJsonStream(Stream stream);
    }
}

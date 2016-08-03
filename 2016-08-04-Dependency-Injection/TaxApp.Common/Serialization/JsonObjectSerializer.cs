using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxApp.Common.Serialization
{
    public class JsonObjectSerializer : IJsonObjectSerializer
    {

        JsonSerializer serializer;
        
        public JsonObjectSerializer()
        {
            serializer = GetDefaultSerializerSettings();
        }

        private JsonSerializer GetDefaultSerializerSettings()
        {
            var serializer = new JsonSerializer();
            serializer.NullValueHandling = NullValueHandling.Ignore;
            serializer.TypeNameHandling = TypeNameHandling.All;
            //serializer.TraceWriter = new NLoggerTraceWriter();
            return serializer;
        }

        public string CreateJsonString<T>(T objectToSerialize)
        {
            string returnString;

            using (var sw = new StringWriter())
            using (var jw = new JsonTextWriter(sw))
            {
                serializer.Serialize(jw, objectToSerialize, typeof(T));
                returnString = sw.ToString();
            }

            return returnString;
        }

        public void CreateJsonToStream<T>(Stream stream, T objectToSerialize, bool preserveReferences)
        {
            var streamSerializer = serializer;

            // override default serializer settings to include
            // preserving references and reference loop handling
            if (preserveReferences)
            {
                streamSerializer = GetDefaultSerializerSettings();
                streamSerializer.PreserveReferencesHandling = PreserveReferencesHandling.All;
                streamSerializer.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
                streamSerializer.MaxDepth = 10;
            }

            using (var sw = new StreamWriter(stream))
            using (var tw = new JsonTextWriter(sw))
            {
                tw.Formatting = Formatting.None;
                streamSerializer.Serialize(tw, objectToSerialize);

                tw.Flush();
            }

        }

        public object DeserializeJsonStream(Stream stream)
        {
            object returnObject = null;
            using (var sr = new StreamReader(stream))
            using (var jr = new JsonTextReader(sr))
            {
                returnObject = serializer.Deserialize(jr);
            }

            return returnObject;
        }

        public T DeserializeJsonStream<T>(Stream stream)
        {
            T returnObject = default(T);

            using (var sr = new StreamReader(stream))
            using (var jr = new JsonTextReader(sr))
            {
                returnObject = serializer.Deserialize<T>(jr);
            }

            return returnObject;
        }

        public T DeserializeJsonString<T>(string jsonString)
        {
            using (var sr = new StringReader(jsonString))
            using (var jw = new JsonTextReader(sr))
            {
                return serializer.Deserialize<T>(jw);
            }

        }

        public class NLoggerTraceWriter : ITraceWriter
        {
            public Logger Logger = LogManager.GetCurrentClassLogger();

            public NLoggerTraceWriter()
            {
               
            }

            public TraceLevel LevelFilter
            {
                // trace all messages. nlog can handle filtering
                get { return TraceLevel.Verbose; }
            }

            public void Trace(TraceLevel level, string message, Exception ex)
            {
                LogEventInfo logEvent = new LogEventInfo
                {
                    Message = message,
                    Level = GetLogLevel(level),
                    Exception = ex
                };

                // log Json.NET message to NLog
                Logger.Log(logEvent);
            }

            private LogLevel GetLogLevel(TraceLevel level)
            {
                switch (level)
                {
                    case TraceLevel.Error:
                        return LogLevel.Error;
                    case TraceLevel.Warning:
                        return LogLevel.Warn;
                    case TraceLevel.Info:
                        return LogLevel.Info;
                    case TraceLevel.Off:
                        return LogLevel.Off;
                    default:
                        return LogLevel.Trace;
                }
            }
        }
    }
}

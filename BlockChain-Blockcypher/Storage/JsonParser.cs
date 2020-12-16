using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BlockChainBlockcypher.Storage
{
    public class JsonParser
    {
        public T Deserialize<T>(string obj)
        {
            return JsonConvert.DeserializeObject<T>(obj);
        }

        public string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }


        public string SerializeArray<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public List<T> DeserializeArray<T>(string array)
        {
            return JsonConvert.DeserializeObject<List<T>>(array);
        }

    }
}

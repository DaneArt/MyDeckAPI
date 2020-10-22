using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyDeckAPI.Interfaces
{
    public class SnakeCaseConverter
    {
       protected static DefaultContractResolver contractResolver = new DefaultContractResolver
       {
           NamingStrategy = new SnakeCaseNamingStrategy()
       };
       public virtual string ConvertToSnakeCase(dynamic obj) 
       {
            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            });
        }

    }
}

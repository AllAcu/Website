using System.Collections.Generic;
using Microsoft.Its.Domain.Serialization;
using Newtonsoft.Json;

namespace AllAcu
{
    public class SerialList<T> : HashSet<T>
    {
        public string Serialized
        {
            get { return this.ToJson(); }
            set
            {
                foreach (var item in JsonConvert.DeserializeObject<T[]>(value))
                {
                    Add(item);
                }
            }
        }
    }
}
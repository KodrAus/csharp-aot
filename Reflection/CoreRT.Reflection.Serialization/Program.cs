using System;
using System.Text.Json;

namespace CoreRT.Reflection.Serialization
{
    class Program
    {
        static void Main(string[] args)
        {
            var de = JsonSerializer.Deserialize(@"{""id"": 42, ""title"": ""A Title""}", typeof(Data));
            var data = (Data)de;
            
            Console.WriteLine("{0}: {1}", data.Id, data.Title);
        }
    }

    class Data
    {
        public string Id { get; set; }
        public string Title { get; set; }
    }
}

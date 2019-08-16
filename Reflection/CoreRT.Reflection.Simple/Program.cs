using System;
using System.Reflection;
using System.Collections.Generic;

namespace CoreRT.Reflection.Simple
{
    class Program
    {
        static void Main(string[] args)
        {
            var type = typeof(List<int>);

            var instance = Activator.CreateInstance(type);

            type.GetMethod(nameof(List<int>.Add)).Invoke(instance, new object[] { 42 });

            var list = (List<int>)instance;

            Console.WriteLine("{0}", list[0]);
        }
    }
}

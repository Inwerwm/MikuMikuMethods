using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    static class Extensions
    {
        public static string ToJson<T>(this T source, string outputPath)
        {
            using (FileStream jsonFile = new(outputPath, FileMode.Create))
            {
                OutputJson(source, jsonFile);
                using (StreamReader jsonReader = new(jsonFile))
                {
                    return jsonReader.ReadToEnd();
                }
            }
        }

        private static void OutputJson<T>(T source, FileStream jsonFile)
        {
            DataContractJsonSerializer serializer = new(typeof(T));
            serializer.WriteObject(jsonFile, source);
        }
    }
}

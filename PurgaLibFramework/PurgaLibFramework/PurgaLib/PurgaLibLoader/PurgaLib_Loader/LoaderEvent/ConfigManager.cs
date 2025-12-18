using System;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibLoader.PurgaLib_Loader.LoaderEvent
{
    public static class ConfigManager
    {
        private static readonly IDeserializer Deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        private static readonly ISerializer Serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        public static object LoadConfig(Type type, string path)
        {
            if (!File.Exists(path))
            {
                var instance = Activator.CreateInstance(type);
                SaveConfig(path, instance);
                return instance;
            }

            var yaml = File.ReadAllText(path);
            return Deserializer.Deserialize(yaml, type);
        }

        public static void SaveConfig(string path, object config)
        {
            var yaml = Serializer.Serialize(config);
            File.WriteAllText(path, yaml);
        }
    }
}
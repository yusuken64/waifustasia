using Microsoft.AspNetCore.Authentication;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Waifustasia.Data.Authentication
{
    public class EncryptedConfiguration
    {
        private readonly CryptoString cryptoString;

        public EncryptedConfiguration(CryptoString cryptoString)
        {
            this.cryptoString = cryptoString;
        }

        public string EncryptConfiguration(IConfiguration configuration)
        {
            var enumerable = configuration.AsEnumerable().ToList();

            StringBuilder stringBuilder = new StringBuilder();
            foreach (var setting in enumerable)
            {
                string key = setting.Key;
                string value = setting.Value;
                string encrypted = cryptoString.EncryptString(setting.Value).KeyAsString();
                stringBuilder.Append($"{key}:{value}:{encrypted}{System.Environment.NewLine}");
            }

            return stringBuilder.ToString();
        }

        private static string ToEncryptedName(string name)
        {
            return name + "_encrypted";
        }

        private static bool IsDecryptedName(string name)
        {
            return name.EndsWith("_decrypted");
        }

        private static bool IsEncryptedName(string name)
        {
            return name.EndsWith("_encrypted");
        }

        private static string FromEncryptedName(string name)
        {
            return name.TrimEnd("_encrypted".ToCharArray());
        }
    }

    public static class JsonNodeExtensions
    {
        public static JsonNode CloneJsonNode(this JsonNode node)
        {
            switch (node)
            {
                case JsonObject jsonObject:
                    var clonedObject = new JsonObject();
                    foreach (var property in jsonObject)
                    {
                        clonedObject.Add(property.Key, CloneJsonNode(property.Value));
                    }
                    return clonedObject;

                case JsonArray jsonArray:
                    var clonedArray = new JsonArray();
                    foreach (var item in jsonArray)
                    {
                        clonedArray.Add(CloneJsonNode(item));
                    }
                    return clonedArray;

                default:
                    return node;
            }
        }
    }
}

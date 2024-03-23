using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Program
{
    public static void Main()
    {
        string jsonString = @"{
            ""removePageCache"": ""/content/admin/remove?cache=pages&id="",
            ""removeTemplateCache"": ""/content/admin/remove?cache=templates&id="",
            ""fileTransferFolder"": ""/usr/local/tomcat/webapps/content/fileTransferFolder"",
            ""lookInContext"": 1,
            ""adminGroupID"": 4,
            ""widget"": {
                ""debug"": ""on"",
                ""window"": {
                    ""title"": ""Sample Konfabulator Widget"",
                    ""name"": ""main_window"",
                    ""width"": 500,
                    ""height"": 500
                },
                ""image"": {
                    ""src"": ""Images/Sun.png"",
                    ""name"": ""sun1"",
                    ""hOffset"": 250,
                    ""vOffset"": 250,
                    ""alignment"": ""center""
                },
                ""text"": {
                    ""data"": ""Click Here"",
                    ""size"": 36,
                    ""style"": ""bold"",
                    ""name"": ""text1"",
                    ""hOffset"": 250,
                    ""vOffset"": 100,
                    ""alignment"": ""center"",
                    ""onMouseUp"": ""sun1.opacity = (sun1.opacity / 100) * 90;""
                }
            }
        }";

        var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);

        var flattenedObject = FlattenJsonObject(jsonObject);

        foreach (var pair in flattenedObject)
        {
            string[] keys = pair.Key.Split('.');
            string lastKey = keys[keys.Length - 1];
            Console.WriteLine($"Última parte da chave {pair.Key}: {lastKey}, Valor: {pair.Value}");
        }

        var flattenedJsonString = JsonConvert.SerializeObject(flattenedObject, Formatting.Indented);

        Console.WriteLine(flattenedJsonString);
    }

    public static Dictionary<string, object> FlattenJsonObject(Dictionary<string, object> json, string parentKey = "")
    {
        var result = new Dictionary<string, object>();

        foreach (var pair in json)
        {
            var key = string.IsNullOrEmpty(parentKey) ? pair.Key : $"{parentKey}.{pair.Key}";

            if (pair.Value is JObject nestedObject)
            {
                var nestedDictionary = nestedObject.ToObject<Dictionary<string, object>>();
                var flattenedNested = FlattenJsonObject(nestedDictionary, key);
                foreach (var nestedPair in flattenedNested)
                {
                    result.Add(nestedPair.Key, nestedPair.Value);
                }
            }
            else
            {
                result.Add(key, pair.Value);
            }
        }

        return result;
    }
}

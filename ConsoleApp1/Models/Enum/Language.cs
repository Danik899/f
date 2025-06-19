using System.Text.Json.Serialization;

namespace KBIPMobileBackend.Models.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Language
    {
        Java = 0,
        CSharp = 1,
        Russian = 2,
    }


}

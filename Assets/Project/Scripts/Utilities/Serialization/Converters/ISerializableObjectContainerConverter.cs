using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class ISerializableObjectContainerConverter : JsonConverter<ISerializableObjectContainer>
{
    public override void WriteJson(JsonWriter writer, ISerializableObjectContainer value, JsonSerializer serializer)
    {
        object innerObject = value.InnerObject;
        JObject obj = JObject.FromObject(innerObject);
        obj.WriteTo(writer);
    }

    public override ISerializableObjectContainer ReadJson(JsonReader reader, Type objectType, ISerializableObjectContainer existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JToken jToken = JToken.ReadFrom(reader);
        Type innerType = existingValue.InnerObject.GetType();
        string jTokenString = jToken.ToString();
        existingValue.InnerObject = JsonConvert.DeserializeObject(jTokenString, innerType);
        return existingValue;
    }
}
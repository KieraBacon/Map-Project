using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class PolymorphicObjectConverter : JsonConverter<IPolymorphicObject>
{
    private object _lock = new object();
    private bool _canRead = true;
    public override bool CanRead =>
        _canRead;
    private bool _canWrite = true;
    public override bool CanWrite =>
        _canWrite;

    public override void WriteJson(JsonWriter writer, IPolymorphicObject value, JsonSerializer serializer)
    {
        lock (_lock)
        {
            if (value is IValidatable validatable)
                validatable.Validate();

            _canWrite = false;
            JObject jobj = JObject.FromObject(value);
            _canWrite = true;

            jobj.AddFirst(new JProperty(IPolymorphicObject.k_TypeIdentifier, value.GetType().AssemblyQualifiedName));
            jobj.WriteTo(writer);
        }
    }

    public override IPolymorphicObject ReadJson(JsonReader reader, Type objectType, IPolymorphicObject existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        lock (_lock)
        {
            JToken token = JToken.ReadFrom(reader);
            string typeString = (string)token[IPolymorphicObject.k_TypeIdentifier];
            if (string.IsNullOrWhiteSpace(typeString)) return null;

            Type type = Type.GetType(typeString);
            if (type == null)
            {
                Debug.LogError($"Unable to locate type {typeString}.");
                return null;
            }

            _canRead = false;
            IPolymorphicObject result = JsonConvert.DeserializeObject(token.ToString(), type) as IPolymorphicObject;
            _canRead = true;

            if (result is IValidatable validatable)
                validatable.Validate();
            return result;
        }
    }
}
using NovaRecipesProject.Common.JsonConverters;

namespace NovaRecipesProject.Common.Extensions; 

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

/// <summary>
/// Extension class for object json
/// </summary>
public static class ObjectJsonExtension
{
    /// <summary>
    /// Sets default settings for JsonSerializerSettings:
    /// adds as converter JsonTrimmingConverter, StringEnumConverter(camelCaseText: true)
    /// </summary>
    /// <param name="settings"></param>
    /// <returns></returns>
    public static JsonSerializerSettings SetDefaultSettings(this JsonSerializerSettings settings)
    {
        settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        settings.Converters.Add(new JsonTrimmingConverter());
        settings.Converters.Add(new StringEnumConverter(typeof(CamelCaseNamingStrategy)));

        return settings;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static JsonSerializerSettings DefaultJsonSerializerSettings()
    {
        return new JsonSerializerSettings().SetDefaultSettings();
    }

    /// <summary>
    /// Serialize object to JSON string
    /// </summary>
    public static string ToJsonString(this object obj, JsonSerializerSettings? settings = null)
    {
        try
        {
            return JsonConvert.SerializeObject(obj, settings ?? DefaultJsonSerializerSettings());
        }
        catch (Exception ex)
        {
            throw new JsonException("Failed to convert to json string", ex);
        }
    }

    /// <summary>
    /// Deserialize object from JSON string
    /// </summary>
    public static T? FromJsonString<T>(this string obj, JsonSerializerSettings? settings = null)
    {
        return JsonConvert.DeserializeObject<T>(obj, settings ?? DefaultJsonSerializerSettings());
    }

    /// <summary>
    /// Deserialize object from JSON string
    /// </summary>
    public static object? FromJsonString(this string obj, JsonSerializerSettings? settings = null)
    {
        return JsonConvert.DeserializeObject(obj, typeof(object), settings ?? DefaultJsonSerializerSettings());
    }

    /// <summary>
    /// Try deserialize object from JSON string
    /// </summary>
    public static bool TryFromJsonString<T>(this string obj, out T? result)
    {
        try
        {
            result = JsonConvert.DeserializeObject<T>(obj);
            return true;
        }
        catch
        {
            result = default;
            return false;
        }
    }
}

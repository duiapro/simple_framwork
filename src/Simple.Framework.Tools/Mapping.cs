using Newtonsoft.Json;

namespace Simple.Framework.Tools;

public class Mapping
{
    public static T Convert<T>(object value)
    {
        var valueString = JsonConvert.SerializeObject(value, null, new JsonSerializerSettings()
        {
            //解决自循环
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });

        return JsonConvert.DeserializeObject<T>(valueString);
    }
}
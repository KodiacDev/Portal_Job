using System.Text.Json;

namespace portal_job_FN.Session
{
    public static class SeesionExtension //fix chuc nang luu bai tuyen dung
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }
        public static T? GetObjectFromJson<T>(this ISession session, string
        key)
        {
            var value = session.GetString(key);
            return value == null ? default :
            JsonSerializer.Deserialize<T>(value);
        }
    }
}

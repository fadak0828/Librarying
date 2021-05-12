using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class PrefUtil : MonoBehaviour
{
    public static void Save<T>(string name, T instance)
    {
        using (var ms = new MemoryStream())
        {
            new BinaryFormatter().Serialize(ms, instance);
            PlayerPrefs.SetString(name, System.Convert.ToBase64String(ms.ToArray()));
        }
    }

    public static T Load<T>(string name) where T : new()
    {
        if (!PlayerPrefs.HasKey(name)) return default(T);
        byte[] bytes = System.Convert.FromBase64String(PlayerPrefs.GetString(name));
        using (var ms = new MemoryStream(bytes))
        {
            object obj = new BinaryFormatter().Deserialize(ms);
            return (T)obj;
        }
    }
}

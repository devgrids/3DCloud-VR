using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Util : MonoBehaviour
{
    public const string BaseUrl = "https://leonidas-mv.com";
    public const string Error = "*$";

    public static String DebugNetwork(UnityWebRequest www)
    {
        if (!www.isNetworkError && !www.isHttpError)
            return www.downloadHandler.text;
        return Error;
    }

    public static List<T> GetJsonList<T>(string json)
    {
        List<T> lista = new List<T>();
        string[] datos = json.Split('}');

        for (int i = 0; i < datos.Length - 1; i++) 
        {
            datos[i] = datos[i].Remove(0, 1);
            datos[i] = datos[i].Insert(datos[i].Length, "}");

            T obj = JsonUtility.FromJson<T>(datos[i]);
            lista.Add(obj);
        }

        return lista;
    }

    public static T[] FromJsonArray<T>(string json)
    {
        string newJson = "{ \"array\": " + json + "}";
        Wrapper<T> wrapper = UnityEngine.JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.array;
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] array;
    }
    
    public static bool CheckGround(Transform transform, float distance)
    {
        Vector3 origin = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y * .5f),
            transform.position.z);
        
        return Physics.Raycast(origin, transform.TransformDirection(Vector3.down), out RaycastHit hit, distance);
    }

}

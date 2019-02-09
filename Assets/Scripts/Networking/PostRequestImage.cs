using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PostRequestImage : MonoBehaviour {

    public void MakePostRequestBytes(byte[] image, string url, string extension)
    {
        StartCoroutine(PostRequestAnImageBytes(image, url, extension));
    }

    IEnumerator PostRequestAnImageBytes(byte[] image, string url, string extension)
    {
        WWWForm form = new WWWForm();
        form.AddBinaryData("mediafile", image, null, "image/" + extension);

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        }
    }

    public void MakePostRequestBytesMethodAlpha(Func<string, GameObject, int> Method, byte[] image, string url, GameObject gameObject, string extension)
    {
        StartCoroutine(PostRequestAnImageBytesMethodAlpha(Method, image, url , gameObject, extension));
    }

    IEnumerator PostRequestAnImageBytesMethodAlpha(Func<string, GameObject, int> Method, byte[] image, string url, GameObject gameObject, string extension)
    {
        WWWForm form = new WWWForm();
        form.AddBinaryData("mediafile", image, null, "image/" + extension);

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("{'success' : 'enviado'}");
                Method(www.downloadHandler.text, gameObject);
            }
        }
    }





    /*  ************************************************ ANTIGUA MANERA DE HACER LA PETICION -- NO BORRAR AUN */


    public void MakePostRequest(byte[] image, string url)
    {
        string base64encoded = Convert.ToBase64String(image);
        base64encoded = base64encoded.Replace("+", @"%2B");
        base64encoded = base64encoded.Replace("/", @"%2F");
        base64encoded = base64encoded.Replace("=", @"%3D");

        StartCoroutine(PostRequestAnImage(base64encoded, url));
    }

    public void MakePostRequestMethodAlpha(Func<string, GameObject, int> Method, byte[] image, string url, GameObject gameObject)
    {
        string base64encoded = Convert.ToBase64String(image);
        base64encoded = base64encoded.Replace("+", @"%2B");
        base64encoded = base64encoded.Replace("/", @"%2F");
        base64encoded = base64encoded.Replace("=", @"%3D");

        StartCoroutine(PostRequestAnImageMethod(Method, base64encoded, url, gameObject));
    }

    IEnumerator PostRequestAnImage(string base64, string url)
    {
        WWWForm form = new WWWForm();
        form.AddField("file", base64);
        Debug.Log(base64.Length);
        
        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        }        
    }

    IEnumerator PostRequestAnImageMethod(Func<string, GameObject, int> Method, string base64, string url, GameObject gameObject)
    {
        WWWForm form = new WWWForm();
        form.AddField("file", base64);
        Debug.Log(base64.Length);

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Method(www.downloadHandler.text, gameObject);
            }
        }
    }

}

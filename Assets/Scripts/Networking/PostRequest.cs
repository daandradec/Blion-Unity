
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Assets.Scripts.Networking;

public class PostRequest : MonoBehaviour {

    public void MakePostRequestMethod(Func<string, int> Method, string[] keys, string[] values, string url) { StartCoroutine(PostRequestMethod(Method, keys, values, url)); }

    public void MakePostRequest(string[] keys, string[] values, string url) { StartCoroutine(PostRequestSimple(keys, values, url)); }

    IEnumerator PostRequestMethod(Func<string, int> Method, string[] keys, string[] values, string url)
    {
        WWWForm form = CreateForm(keys, values);
        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                Response response = JsonUtility.FromJson<Response>(www.downloadHandler.text);
                this.gameObject.GetComponent<NetworkController>().LogRequestErrorMessage(www.error +"\n"+ response.message);
            }
            else
            {            
                Method(www.downloadHandler.text);
            }
        }
    }

    IEnumerator PostRequestSimple(string[] keys, string[] values, string url)
    {
        WWWForm form = CreateForm(keys, values);
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

    public WWWForm CreateForm(string[] keys,string[] values)
    {
        WWWForm form = new WWWForm();
        for (int i = 0; i < keys.Length; ++i)
        {
            form.AddField(keys[i], values[i]);
        }
        return form;
    }
}

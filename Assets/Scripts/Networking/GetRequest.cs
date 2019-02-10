using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

public class GetRequest : MonoBehaviour {

    public void MakeGetRequestMethod(Func<string, int> Method, string url) { StartCoroutine( GetRequestMethod(Method, url) ); }

	//public void MakeGetRequest() { StartCoroutine(GetRequestSimple()); }

    IEnumerator GetRequestMethod(Func<string, int> Method, string url)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            UnityWebRequestAsyncOperation request = www.SendWebRequest();
            while (!request.isDone)
            {
                //Debug.Log("www img progress: "+www.downloadProgress);
                yield return null;
            }

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Method(www.downloadHandler.text);
            }
        }
    }
    /*
    IEnumerator GetRequestSimple()
    {
        yield return 1;
    }*/
}

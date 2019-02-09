using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkController : MonoBehaviour {

    /* TODAS LAS VARIABLES ATRIBUTOS DE REQUEST */
    private GetRequest getRequest;
    private GetRequestTexture getRequestTexture;
    private PostRequest postRequest;
    private PostRequestImage postRequestImage;
    private PostRequestVideo postRequestVideo;

    /* VARIABLES ATRIBUTOS PERSISTENTES */
    private PersistentObjects persistentObjects;
    private NetworkUrls urls;
    
    /*DEBUG PANEL*/
    public PauseManager pauseManager;



    /* ################################### INICIALIZACIÓN ################################### */
    private void Awake()
    {
        getRequest = this.GetComponent<GetRequest>();
        getRequestTexture = this.GetComponent<GetRequestTexture>();
        postRequest = this.GetComponent<PostRequest>();
        postRequestImage = this.GetComponent<PostRequestImage>();
        postRequestVideo = this.GetComponent<PostRequestVideo>();

        persistentObjects = this.GetComponent<PersistentObjects>();
        urls = this.GetComponent<NetworkUrls>();
        RequireCanvasPauseManager();
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }



    /* ###################################### REQUEST GET GENERICO ###################################### */

    public void GetRequest(Func<string, int> Method, string url)
    {
        this.getRequest.MakeGetRequestMethod(Method, url);
    }


    /* ###################################### REQUEST GET ASOCIADOS A IMAGENES ###################################### */

    public void GetRequestTexture(Func<Texture2D, int> Method, string url)
    {
        this.getRequestTexture.MakeGetRequestMethod(Method, url);
    }

    public void GetRequestTextureAlpha(Func<Texture2D, string, int> Method, string url, string pathMediaContent)
    {
        this.getRequestTexture.MakeGetRequestMethodAlpha(Method, url, pathMediaContent);
    }



    /* ###################################### REQUEST POST COMUNES ###################################### */

    public void PostRequest(string[] keys, string[] values, string url)
    {
        this.postRequest.MakePostRequest(keys, values, url);
    }

    public void PostRequestMethod(Func<string, int> Method, string[] keys, string[] values, string url)
    {
        this.postRequest.MakePostRequestMethod(Method, keys, values, url);
    }





    /* ###################################### REQUEST POST ASOCIADOS A IMAGENES ###################################### */

    public void PostRequestImageBytes(byte[] image, string url, string extension)
    {
        this.postRequestImage.MakePostRequestBytes(image, url, extension);
    }
    public void PostRequestImageBytesMethodAlpha(Func<string, GameObject, int> Method, byte[] image, string url, GameObject gameObject, string extension)
    {
        this.postRequestImage.MakePostRequestBytesMethodAlpha(Method, image, url, gameObject, extension);
    }




    /* ###################################### REQUEST POST ASOCIADOS A VIDEOS ###################################### */

    public void PostRequestVideoBytesMethodAlpha(Func<string, GameObject, int> Method, byte[] video, string url, GameObject gameObject, string extension)
    {
        this.postRequestVideo.MakePostRequestBytesMethodAlpha(Method, video, url, gameObject, extension);
    }







    /* ###################################### PANEL DE DEBUGGING ###################################### */

    public void LogRequestErrorMessage(string text)
    {
        pauseManager.PauseWithTextParameter(text);
    }




    /* ###################################### GETTERS ###################################### */

    public NetworkUrls GetUrls()
    {
        return this.urls;
    }
    public PersistentObjects GetPersistentObjects()
    {
        return this.persistentObjects;
    }



    /* ###################################### SCENES EVENTS ###################################### */

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "01_Login":
                this.RequireCanvasPauseManager();
                break;
            case "02_Registration":
                this.RequireCanvasPauseManager();
                break;
        }
    }

    private void RequireCanvasPauseManager()
    {
        this.pauseManager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<PauseManager>();
    }
}
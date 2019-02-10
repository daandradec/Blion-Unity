using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkUrls : MonoBehaviour {

    private string DOMAIN = "http://aqueous-tor-84195.herokuapp.com";/** http://blion.test **/

    /* APIS URLS */ /* Recordar que toca tambien modificarlos en el inspector en el objeto NetworkController */
    public string GET_USER = "/api/users/";
    public string GET_USER_IMAGE = "/image";
    public string GET_USER_CONTENTS = "/contents";
    public string GET_USER_MEDIA_CONTENTS = "/mediacontent/media";
    public string POST_USER = "/api/users/";
    public string POST_USER_IMAGE = "/image";
    public string POST_USER_MEDIA_CONTENTS = "/mediacontent/media";
    public string POST_USER_DESTROY_MEDIA_CONTENT = "/mediacontent/destroy";
    

    public string POST_LOGIN_API = "/api/login";
    public string POST_REGISTER_API = "/api/register";

    public string GetMainDomain()
    {
        return this.DOMAIN;
    }
}

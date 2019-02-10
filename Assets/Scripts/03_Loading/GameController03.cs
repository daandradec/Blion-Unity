using System.IO;
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Assets.Scripts.Networking;

public class GameController03 : MonoBehaviour {

    /* VARIABLE ATRIBUTO PREFAB DEL CONTENEDOR DE VIDEO */
    public GameObject videoMediaContent;

    /* VARIABLE ATRIBUTO CONTROLADOR DE REDES */
    private NetworkController networkController;

    /* VARIABLES ATRIBUTOS DE CALCULOS */
    private int resources;
    private int LEN_RESOURCES = 0;




    /* ################################### INICIALIZACIÓN ################################### */
    private void Awake()
    {
        this.networkController = GameObject.FindGameObjectWithTag("NetworkController").GetComponent<NetworkController>();
        this.resources = 0;
    }

    private void Start()
    {
        // si el usuario existe en el networkController garantiza que paso por la escena 01_login o 02_registration  
        if(this.networkController.GetPersistentObjects().GetUser() != null)
            CallRequest();      
    }





    /* ####################### TODOS LOS METODOS DE PETICIONES NECESARIOS ANTES DE ENTRAR AL HOME DE LA APLICACIÓN ####################### */


    /// <summary> #####################################################################################
    ///        Metodo para hacer la peticion de la imagen de perfil del usuario, al completarse la peticion ejecutara el metodo RequireImage 
    /// </summary> 
    private void CallRequest()
    {
        var netURLS = this.networkController.GetUrls();
        UserResponse user = this.networkController.GetPersistentObjects().GetUser();
        this.networkController.GetRequestTexture(RequireImage, netURLS.GetMainDomain() + netURLS.GET_USER + user.id + "/" + user.auth_token + netURLS.GET_USER_IMAGE);        
    }




    /// <summary> #####################################################################################
    ///        Metodo que se ejecuta cuando la peticion de la imagen fue hecha al servidor, se encarga de a partir de la textura obtenida
    ///        crear un sprite y guardarlo como la imagen de perfil del usuario logueado, esta se guarda como un sprite persistente de la clase 
    ///        Objeto UserResponse (objeto persistente del NetworkController).
    /// </summary> 
    private int RequireImage(Texture2D texture)
    {
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero, 1f);
        this.networkController.GetPersistentObjects().SetImageToCurrentUser(sprite);
        CallMainRequest();
        return 1;
    }




    /// <summary> #####################################################################################
    ///        Metodo para hacer la peticion de los contenidos Media (imagenes,y video) del usuario logueado, se ejecuta despues de que se
    ///        que pide la imagen de perfil del usuario.
    /// </summary> 
    private void CallMainRequest()
    {
        var netURLS = this.networkController.GetUrls();
        UserResponse user = this.networkController.GetPersistentObjects().GetUser();
        this.networkController.GetRequest(RequireMediaContentsUser, netURLS.GetMainDomain() + netURLS.GET_USER + user.id + "/" + user.auth_token + netURLS.GET_USER_CONTENTS);
    }




    /// <summary> #####################################################################################
    ///        Metodo que se ejecuta cuando la peticion de los contenidos media del usuario fueron hechas, lo guarda en la lista de objetos
    ///        persistentes del UserResponse del NetworkController, despues de guardar las urls entonces hace el ultimo request y mas importante
    ///        antes de pasar a la siguiente escena en RequestAllRemainingBlion()
    /// </summary> 
    private int RequireMediaContentsUser(string answer)
    {
        ResponseList response = JsonUtility.FromJson<ResponseList>(answer);
        
        if (response.success)
        {
            this.networkController.GetPersistentObjects().SetMediaContentsUserURLS(response.message);
            RequestAllRemainingBlion();
        }

        return 1;
    }




    /// <summary> #####################################################################################
    ///        Metodo para hacer la peticion de las texturas correspondientes a las imagenes, y de los videos que reproduciran los VideoPlayer
    ///        de los contenidos media del usuario.
    ///        El metodo comienza inicializando unas variables, una de esas es el array de strings de las urls de los contenidos media del
    ///        usuario, luego recorre la lista y pregunta cuales corresponden a la extension del tipo imagen, y las guarda en una lista para
    ///        procesarlas despues del primer bucle, y lo que no son imagen los toma como video para hacer la peticion de los videos.
    ///        Luego segun el numero de imagenes en la lista lo asigna a la variable LEN_RESOURCES que se usara para saber despues de hacer
    ///        la peticion de todas las imagenes en el segundo bucle si todas las imagenes fueron pedidas.
    /// </summary> 
    private void RequestAllRemainingBlion()
    {
        string[] mediaContentsURLS = this.networkController.GetPersistentObjects().GetUser().mediaContentsURLS;      
        string[] image_extensions = { ".png", ".jpg", ".jpeg" };
        var netURLS = this.networkController.GetUrls();
        List<string> images_urls = new List<string>();
        UserResponse user = this.networkController.GetPersistentObjects().GetUser();
        Debug.Log(netURLS.GET_USER_MEDIA_CONTENTS);
        foreach (string url in mediaContentsURLS)
        {
            Debug.Log(url);
            if (image_extensions.Contains(Path.GetExtension(url)))
                images_urls.Add(url);
            else
                RequireVideosPlayersMediaContents(url, netURLS);                            
        }

        this.LEN_RESOURCES = images_urls.Count(); 
        
        foreach (string url_image in images_urls)
        {
            this.networkController.GetRequestTextureAlpha(RequireMediaContentImage, netURLS.GetMainDomain() + netURLS.GET_USER + user.id + "/" + user.auth_token + netURLS.GET_USER_MEDIA_CONTENTS + "?path=" + url_image, url_image);
        }

        if (this.LEN_RESOURCES == 0)
            LoadSceneByName("04_User");
    }







    /* ################################### Metodos asociados a los contenidos media de tipo imagenes ################################### */




    /// <summary> #####################################################################################
    ///         Metodo que se ejecuta cuando la peticion de la imagen fue hecha al servidor, se encarga de a partir de la textura obtenida
    ///         crear un sprite y guardarlo en la lista de objetos persistentes de imageMediaContents para poder tener su referencia.
    ///         Por ultimo actualiza el numero de recursos cargados.
    /// </summary>    
    public int RequireMediaContentImage(Texture2D texture, string pathMediaContent)
    {
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero, 1f);
        this.networkController.GetPersistentObjects().SetNewMediaContentsImage(sprite);
        this.networkController.GetPersistentObjects().SetMediaContentsImagePathOrder(pathMediaContent);

        EventChangeSceneWhenMaxResourcesLoaded();

        return 1;
    }




    /// <summary> #####################################################################################
    ///         Metodo para llevar la cuenta del numero de recursos e incrementarlo para saber si todas las imagenes pedidas al hacer la peticion
    ///         alcanza el numero de imagenes que tiene el usuario, si todas las imagenes fueron pedidas entonces cambie a la siguiente escena.
    /// </summary>  
    private void EventChangeSceneWhenMaxResourcesLoaded()
    {
        ++this.resources;
        if (this.resources >= this.LEN_RESOURCES)
            LoadSceneByName("04_User");
    }






    /* ################################### Metodos asociados a los contenidos media de tipo video ################################### */




    /// <summary> #####################################################################################
    ///     RequireVideosPlayersMediaContents() recibe la url del archivo al cual se va a hacer un request GET y el networkURLS para acceder a
    ///     las rutas de las APIS del hosting.
    ///     Se encarga de Instanciar el prefab del contenedor de un video, el cual tiene al videoplayer y el boton de borrar.
    ///     Le asigna la url asociada al objeto para cuando se vaya a destruir en el sistema de archivos poder tener acceso a esta.
    ///     Luego configure el componente videoplayer para que muestre el video de la url a pedir, y por ultimo guarda la referencia del objeto
    ///     en la lista de objetos persistentes de videomediacontents del networkcontroller.
    /// </summary>    
    private void RequireVideosPlayersMediaContents(string url, NetworkUrls netURLS)
    {
        GameObject video = InstantiateVideoMediaContentPersistent();
        UserResponse user = this.networkController.GetPersistentObjects().GetUser();
        video.GetComponent<MediaContent>().SetAssociatedPath(url);
        video.transform.GetChild(0).GetComponent<VideoMediaContent>().ConfigureVideoPlayer(netURLS.GetMainDomain() + netURLS.GET_USER + user.id + "/" + user.auth_token + netURLS.GET_USER_MEDIA_CONTENTS + "?path=" + url);
        this.networkController.GetPersistentObjects().SetMediaContentsVideo(video);
    }




    /// <summary> #####################################################################################
    ///         Metodo para instanciar un prefab de contenedor de video media contents, y luego lo vuelve persistente con el DontDestoryOnLoad
    /// </summary>  
    private GameObject InstantiateVideoMediaContentPersistent()
    {
        GameObject video = Instantiate(videoMediaContent);
        DontDestroyOnLoad(video);
        return video;
    }






    /* ################################### Metodos exclusivos del GameController03 ################################### */

    public void LoadSceneByName(string scene)
    {
        SceneManager.LoadScene(scene);
    }

}

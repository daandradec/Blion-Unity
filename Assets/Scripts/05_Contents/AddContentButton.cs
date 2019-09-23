using Assets.Scripts.Networking;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class AddContentButton : MonoBehaviour {

    /* VARIABLE ATRIBUTO REFERENCIA DEL GAMECONTROLLER DE LA ESCENA */
    public GameController05 gameController;



    /* ################################### METODO DEL EVENTO DE CLICK AL BOTON EN LA IMAGEN DE PERFIL ################################### */


    /// <summary> #####################################################################################
    ///         Este metodo pregunta si la aplicacion es el editor o si ya es una aplicacion creada para alguna plataforma de sistema operativo
    ///         segun el caso utiliza el panel de selector de archivos por defecto para el editor de unity o el FileBrowserAndroid, ambos
    ///         con funcionalidad de obtener el path de un archivo seleccionado y ejecutar un metodo con ese path
    /// </summary> 
    public void AddNewContent()
    {
        if (Application.isEditor)
        {
            string path = gameController.GetFilePanel().SearchFile();
            BuildMedia(path);
        }
        else
        {
            gameController.GetFilePanel().SearchFileWithFileManager(BuildMedia);

        }
    }




    /* ################################### METODO PARA CONSTRUIR UN ARCHIVO MEDIA APARTIR DE SU PATH ################################### */


    /// <summary> #####################################################################################
    ///         Metodo que es llamado desde cualquiera de los FileBrowsers, que recibe un path y si no es nulo entonces comprueba si
    ///         la extension del archivo es una imagen, sino entonces es tomado como video, y genera en el caso de imagen un sprite
    ///         para guardarlo en la lista de objetos persistentes ImageMediaContent, y luego hace una peticion de tipo imagen,
    ///         si es un video solo hace una peticion de tipo video, por ultimo para ambos casos se instancian contenedores de componentes
    ///         media que permitiran ver el archivo media en la escena.
    /// </summary>  
    private int BuildMedia(string path)
    {
        if (path.Length > 0)
        {
            // si la extension es una imagen, si es un video haga otra cosa

            byte[] img = File.ReadAllBytes(path);
            var netController = gameController.GetNetworkController();
            UserResponse user = netController.GetPersistentObjects().GetUser();
            var netURLS = netController.GetUrls();
            

            string[] image_extensions = { ".png", ".jpg", ".jpeg" };

            if ( image_extensions.Contains(Path.GetExtension(path)))
            {
                Texture2D texture = GenerateImage(img);
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero, 1f);

                GameObject mediaImage = gameController.InstantianteMediaContentComponent(gameController.GetPrefabImageMediaContent(), gameController.GetMedConOffsetX(), gameController.GetMedConOffsetY());
                gameController.ConfigureMediaContentImage(mediaImage, sprite);
                gameController.UpdateMediaContentCoordinates(mediaImage);

                netController.GetPersistentObjects().SetNewMediaContentsImage(sprite);

                netController.PostRequestImageBytesMethodAlpha(SetMediaContentPathToGameObject, img,
                    netURLS.GetMainDomain() + netURLS.POST_USER + user.id + netURLS.POST_USER_MEDIA_CONTENTS + "?token=" + user.token,
                    mediaImage, Path.GetExtension(path).Substring(1));
            }
            else
            {
                GameObject mediaVideo = gameController.InstantianteMediaVideoComponent(gameController.GetPrefabVideoMediaContent(), gameController.GetMedConOffsetX(), gameController.GetMedConOffsetY());
                gameController.UpdateMediaContentCoordinates(mediaVideo);                

                netController.PostRequestVideoBytesMethodAlpha(SetMediaContentVideoPathToGameObject, img,
                    netURLS.GetMainDomain() + netURLS.POST_USER + user.id + netURLS.POST_USER_MEDIA_CONTENTS + "?token=" + user.token,
                    mediaVideo, Path.GetExtension(path).Substring(1));
            }
        }
        return 0;
    }



    /* ############################### METODO PARA GUARDAR EL PATH DEL ARCHIVO Y ACTUALIZAR EL CONTENEDOR ############################### */


    /// <summary> #####################################################################################
    ///         Metodo que se ejecuta una vez se hace la peticion del tipo imagen, y que recibe una respuesta en la variable response.message
    ///         del path que tendra el archivo almacenado en la nube en la base de datos, con este path se procede a asociar ese path al
    ///         contenedor de la imagen para que cuando sea la hora de borrarlo se puede borrar no solo en la escena sino tambien del sistema
    ///         de archivos en la nube
    /// </summary>  
    private int SetMediaContentPathToGameObject(string answer, GameObject mediaImage)
    {
        Response response = JsonUtility.FromJson<Response>(answer);
        if (response.success)
        {
            var netController = this.gameController.GetNetworkController();
            int index = netController.GetPersistentObjects().GetUser().mediaContentsImagesPathsOrder.Count();
            mediaImage.GetComponent<MediaContent>().SetAssociatedPathAndIndex(response.message,index);
            netController.GetPersistentObjects().SetMediaContentsImagePathOrder(response.message);
        }
            
        return 1;
    }



    /* ########################### METODO PARA GUARDAR EL PATH DEL ARCHIVO Y ACTUALIZAR EL CONTENEDOR DE VIDEO ########################### */

        
    /// <summary> #####################################################################################
    ///        Este metodo hace casi lo mismo que el de arriba, obtiene el path del archivo en la nube y lo asocia al contenedor del 
    ///        componente de contenido media de video, luego accede al primer hijo y prepara su componente VideoPlayer asignandole la url
    ///        a la url del host al cual hara la peticion del video guardado, y por ultimo guarda su referencia en la lista de objetos
    ///        persistentes MediaContentsVideo
    /// </summary>  
    private int SetMediaContentVideoPathToGameObject(string answer, GameObject mediaVideo)
    {
        Response response = JsonUtility.FromJson<Response>(answer);
        if (response.success)
        {
            var netController = this.gameController.GetNetworkController();
            var netURLS = netController.GetUrls();
            int index = netController.GetPersistentObjects().GetUser().mediaContentsVideos.Count();
            UserResponse user = netController.GetPersistentObjects().GetUser();
            mediaVideo.GetComponent<MediaContent>().SetAssociatedPathAndIndex(response.message,index);
            mediaVideo.transform.GetChild(0).GetComponent<VideoMediaContent>().ConfigureVideoPlayer(netURLS.GetMainDomain() + netURLS.GET_USER + user.id + netURLS.GET_USER_MEDIA_CONTENTS + "?path=" + response.message + "?token=" + user.token);
            netController.GetPersistentObjects().SetMediaContentsVideo(mediaVideo);
        }

        return 1;
    }



    /* ################################ METODO PARA GENERAR UNA TEXTURA APARTIR DE LOS BYTES DE UNA IMAGEN ################################ */

    private Texture2D GenerateImage(byte[] img)
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(img);
        return texture;
    }
}

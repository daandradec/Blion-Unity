using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ChangeImageButton : MonoBehaviour {

    /* VARIABLE ATRIBUTO REFERENCIA DEL GAMECONTROLLER DE LA ESCENA */
    public GameController04 gameController;



    /* ################################### METODO DEL EVENTO DE CLICK AL BOTON EN LA IMAGEN DE PERFIL ################################### */

    /// <summary> #####################################################################################
    ///         Este metodo pregunta si la aplicacion es el editor o si ya es una aplicacion creada para alguna plataforma de sistema operativo
    ///         segun el caso utiliza el panel de selector de archivos por defecto para el editor de unity o el FileBrowserAndroid, ambos
    ///         con funcionalidad de obtener el path de un archivo seleccionado y ejecutar un metodo con ese path
    /// </summary>  
    public void ModifyCurrentImageUser()
    {
        if (Application.isEditor)
        {
            string path = gameController.GetFilePanel().SearchFile();
            BuildImage(path);            
        }
        else
        {
            gameController.GetFilePanel().SearchFileWithFileManager(BuildImage);
        }                
    }





    /* ################################### METODO PARA CONSTRUIR UNA IMAGEN DE PERFIL APARTIR DE SU PATH ################################### */


    /// <summary> #####################################################################################
    ///         Metodo que es llamado desde cualquiera de los FileBrowsers, que recibe un path y si no es nulo entonces construye un sprite
    ///         para asignarselo a la imagen de perfil y guardarlo en el servidor con un request.
    /// </summary>  
    private int BuildImage(string path)
    {
        if (path.Length > 0)
        {
            byte[] img = File.ReadAllBytes(path);
            var netController = gameController.GetNetworkController();
            var netURLS = netController.GetUrls();

            Texture2D texture = GenerateImage(img);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero, 1f);
            netController.GetPersistentObjects().SetImageToCurrentUser(sprite);
            gameController.userImage.sprite = sprite;

            netController.PostRequestImageBytes(img, 
                netURLS.GetMainDomain() + netURLS.POST_USER + netController.GetPersistentObjects().GetUser().id + netURLS.POST_USER_IMAGE,
                Path.GetExtension(path).Substring(1));

        }
        return 0;
    }



    /* ################################ METODO PARA GENERAR UNA TEXTURA APARTIR DE LOS BYTES DE UNA IMAGEN ################################ */

    private Texture2D GenerateImage(byte[] img)
    {        
        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(img);
        return texture;
    }
}

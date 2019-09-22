using Assets.Scripts.Networking;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class DestroyContentButton : MonoBehaviour {

    /* VARIABLE ATRIBUTO REFERENCIA DEL GAMECONTROLLER DE LA ESCENA */
    private GameController05 gameController;



    /* ################################### INICIALIZACIÓN ################################### */

    private void Awake()
    {
        FindAndSetGameController();
    }


    /* ################################### METODO PARA ENCONTRAR EL GAMECONTROLLER05 ################################### */
    public void FindAndSetGameController()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController05>();
    }





    /* ################################### METODO PARA DESTRUIR EL MEDIA CONTENT SELECCIONADO ################################### */

    public void DestroyContent()
    {
        var netController = gameController.GetNetworkController();
        var netURLS = netController.GetUrls();
        UserResponse user = netController.GetPersistentObjects().GetUser();
        string file_path = this.transform.parent.gameObject.GetComponent<MediaContent>().GetAsociatedPath();

        string[] keys = { "path"};
        string[] values = { file_path };
        string url = netURLS.GetMainDomain() + netURLS.POST_USER + user.id + netURLS.POST_USER_DESTROY_MEDIA_CONTENT;


        string[] image_extensions = { ".png", ".jpg", ".jpeg" };
        int index = this.transform.parent.gameObject.GetComponent<MediaContent>().GetIndexOnList();

        if (image_extensions.Contains(Path.GetExtension(file_path)))
        {
            netController.GetPersistentObjects().RemoveItemMediaContentsImage(index);
            netController.GetPersistentObjects().RemoveItemMediaContentsImagePathOrder(index);
        }
        else
        {
            GameObject video = netController.GetPersistentObjects().GetMediaContentsVideo(index);
            netController.GetPersistentObjects().RemoveItemMediaContentsVideo(index);
            Destroy(video.gameObject);
        }


        gameController.ClearMediaContent();
        gameController.FillMediaContent();

        netController.PostRequest(keys,values,url);
    }
}

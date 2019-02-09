using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class DestroyContentButton : MonoBehaviour {

    private GameController05 gameController;

    private void Awake()
    {
        FindAndSetGameController();
    }

    public void FindAndSetGameController()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController05>();
    }

    public void DestroyContent()
    {
        var netController = gameController.GetNetworkController();
        var netURLS = netController.GetUrls();
        string file_path = this.transform.parent.gameObject.GetComponent<MediaContent>().GetAsociatedPath();

        string[] keys = { "path"};
        string[] values = { file_path };
        string url = netURLS.GetMainDomain() + netURLS.POST_USER + gameController.GetNetworkController().GetPersistentObjects().GetUser().id + 
            netURLS.POST_USER_DESTROY_MEDIA_CONTENT;


        string[] image_extensions = { ".png", ".jpg", ".jpeg" };
        if (image_extensions.Contains(Path.GetExtension(file_path)))
        {
            //int index = netController.GetPersistentObjects().GetIndexMediaContentImage(file_path);
            int index = this.transform.parent.gameObject.GetComponent<MediaContent>().GetIndexOnList();

            netController.GetPersistentObjects().RemoveItemMediaContentsImage(index);
            netController.GetPersistentObjects().RemoveItemMediaContentsImagePathOrder(index);
        }
        else
        {
            //netController.GetPersistentObjects().RemoveItemMediaContentsVideo(file_path);
            int index = this.transform.parent.gameObject.GetComponent<MediaContent>().GetIndexOnList();
            GameObject video = netController.GetPersistentObjects().GetMediaContentsVideo(index);
            netController.GetPersistentObjects().RemoveItemMediaContentsVideo(index);
            Destroy(video.gameObject);
        }


        gameController.ClearMediaContent();
        gameController.FillMediaContent();

        netController.PostRequest(keys,values,url);
    }
}

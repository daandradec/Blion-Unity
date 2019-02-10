using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GameController05 : MonoBehaviour {

    /* VARIABLES ATRIBUTOS QUE VIENEN DEL INSPECTOR */
    public Canvas canvas;
    public GameObject mediaContentsBody;
    public GameObject imageMediaContent;
    public GameObject videoMediaContent;

    /* VARIABLES ATRIBUTOS DE CALCULOS */
    private float width;
    private float x_count;
    private float y_count;
    private float MAX_LIMIT_WIDTH;

    /* VARIABLES ATRIBUTOS COMPONENTES OBJETOS */
    private NetworkController networkController;
    private FilePanel filePanel;


    /* ################################### INICIALIZACIÓN ################################### */

    private void Awake()
    {
        this.networkController = GameObject.FindGameObjectWithTag("NetworkController").GetComponent<NetworkController>();
        this.filePanel = this.GetComponent<FilePanel>();
        this.MAX_LIMIT_WIDTH = canvas.GetComponent<RectTransform>().rect.width;
    }

    private void Start()
    {        
        width = (imageMediaContent.GetComponent<RectTransform>().rect.width + 10f) * canvas.scaleFactor;
        x_count = 0f;
        y_count = 0f;

        foreach (GameObject video in this.networkController.GetPersistentObjects().GetUser().mediaContentsVideos)
        {
            video.transform.GetChild(1).GetComponent<DestroyContentButton>().FindAndSetGameController();
        }

        FillMediaContent();
    }






    /* ########################## ITERAR TODOS LOS CONTENIDOS MEDIA DE IMAGENES Y VIDEO Y AÑADIRLOS A LA ESCENA ########################## */

    public void FillMediaContent()
    {
        int index = 0;

        foreach (Sprite image in this.networkController.GetPersistentObjects().GetUser().mediaContentsImages)
        {
            GameObject mediaImage = InstantianteMediaContentComponent(imageMediaContent, (width * x_count), (width * y_count));
            mediaImage.GetComponent<MediaContent>().SetAssociatedPathAndIndex(this.networkController.GetPersistentObjects().GetMediaContentsImagePathOrder(index),index);
            ConfigureMediaContentImage(mediaImage, image);
            UpdateMediaContentCoordinates(mediaImage);
            ++index;
        }
        
        index = 0;

        foreach (GameObject video in this.networkController.GetPersistentObjects().GetUser().mediaContentsVideos)
        {            
            video.GetComponent<MediaContent>().SetAssociatedIndex(index);
            ConfigureMediaVideo(video, (width * x_count), (width * y_count));
            ++index;
        }
    }




    /* ####################   METODOS PARA INSTANCIAR COMPONENTES MEDIA DE IMAGEN O VIDEO   ####################*/

    public GameObject InstantianteMediaContentComponent(GameObject prefab, float offsetx, float offsety)
    {
        GameObject mediaObject = Instantiate(prefab, mediaContentsBody.transform, false);
        RectTransform imageRect = mediaObject.GetComponent<RectTransform>();
        imageRect.anchoredPosition3D = new Vector3(imageRect.anchoredPosition3D.x + offsetx, imageRect.anchoredPosition3D.y - offsety, imageRect.anchoredPosition3D.z); // anchored position es el vector de coordenadas3D en el RectTransform
        return mediaObject;
    }

    public GameObject InstantianteMediaVideoComponent(GameObject prefab, float offsetx, float offsety)
    {
        GameObject mediaObject = Instantiate(prefab, mediaContentsBody.transform, false);
        mediaObject.GetComponent<RectTransform>().anchoredPosition3D = imageMediaContent.GetComponent<RectTransform>().anchoredPosition3D; // como al volverlo hijo del mediaBody del canvas sus coordenadas cambian entonces se las reinicio a las que utiliza el contenedor de imagenes
        RectTransform videoRect = mediaObject.GetComponent<RectTransform>();
        videoRect.anchoredPosition3D = new Vector3(videoRect.anchoredPosition3D.x + offsetx, videoRect.anchoredPosition3D.y - offsety, videoRect.anchoredPosition3D.z);
        videoRect.localRotation = Quaternion.identity; // reiniciar rotacion (se modificaba por causa de volverlo hijo del mediaBody)
        videoRect.localScale = Vector3.one; // reiniciar escala (se modificaba por causa de volverlo hijo del mediaBody)
        return mediaObject;
    }






    /* ####################  METODOS PARA CONFIGURAR LAS IMAGENES O VIDEOS QUE ESTARAN EN LA ESCENA ####################*/

    public void ConfigureMediaVideo(GameObject video, float offsetx, float offsety)
    {        
        video.transform.SetParent(mediaContentsBody.transform);
        video.GetComponent<RectTransform>().anchoredPosition3D = imageMediaContent.GetComponent<RectTransform>().anchoredPosition3D; // como al volverlo hijo del mediaBody del canvas sus coordenadas cambian entonces se las reinicio a las que utiliza el contenedor de imagenes
        RectTransform videoRect = video.GetComponent<RectTransform>();        
        videoRect.anchoredPosition3D = new Vector3(videoRect.anchoredPosition3D.x + offsetx, videoRect.anchoredPosition3D.y - offsety, videoRect.anchoredPosition3D.z);
        UpdateMediaContentCoordinates(video);
    }

    public void ConfigureMediaContentImage(GameObject mediaImage, Sprite image)
    {
        Image imageObject = mediaImage.transform.GetChild(0).GetComponent<Image>();
        imageObject.preserveAspect = true;
        imageObject.sprite = image;
    }







    /* #################### ACTUALIZADO Y BORRADO DE LAS VARIABLES QUE MANEJAN LA DISPOSICION DE REJILLA EN LA ESCENA #################### */

    public void UpdateMediaContentCoordinates(GameObject mediaImage)
    {
        ++x_count;

        if (mediaImage.GetComponent<RectTransform>().anchoredPosition3D.x + (width * 2f) > MAX_LIMIT_WIDTH)
        {
            x_count = 0;
            ++y_count;
            mediaContentsBody.GetComponent<RectTransform>().sizeDelta = new Vector2(MAX_LIMIT_WIDTH, (width + 10f)*(y_count+1f) );
        }
    }

    public void ClearMediaContent()
    {
        x_count = 0f;
        y_count = 0f;

        foreach (Transform child in mediaContentsBody.transform)
        {
            if (child.gameObject.tag == "Video")
                continue;
            GameObject.Destroy(child.gameObject);
        }
    }




    /* ################################### Metodos exclusivos del GameController05 ################################### */

    public float GetMedConOffsetX()
    {
        return (width * x_count);
    }

    public float GetMedConOffsetY()
    {
        return (width * y_count);
    }

    public GameObject GetPrefabImageMediaContent()
    {
        return this.imageMediaContent;
    }

    public GameObject GetPrefabVideoMediaContent()
    {
        return this.videoMediaContent;
    }

    public FilePanel GetFilePanel()
    {
        return this.filePanel;
    }

    public NetworkController GetNetworkController()
    {
        return this.networkController;
    }

    public void LoadSceneByName(string scene)
    {
        TransformVideosInPersistents();
        SceneManager.LoadScene(scene);
    }
    
    private void TransformVideosInPersistents()
    {
        foreach(GameObject video in this.networkController.GetPersistentObjects().GetUser().mediaContentsVideos)
        {
            video.transform.SetParent(null);
            DontDestroyOnLoad(video);
        }
    }
}

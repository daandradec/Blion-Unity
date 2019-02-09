using Assets.Scripts.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentObjects : MonoBehaviour {

    private UserResponse user;



    /* ###################################### USER SETTERS ###################################### */

    public void SetCurrentUser(string user)
    {
        this.user = JsonUtility.FromJson<UserResponse>(user);
        this.user.mediaContentsImages = new List<Sprite>();
    }

    public void SetCurrentUser(UserResponse user)
    {
        this.user = user;
    }






    /* ###################################### USER GETTERS ###################################### */

    public UserResponse GetUser()
    {
        return user;
    }








    /* ###################################### USER METHODS ###################################### */
    public void SetImageToCurrentUser(Sprite image)
    {
        this.user.image = image;
    }











    /* ###################################### USER ATTRIBUTE : MEDIACONTENTSURLS ###################################### */

    public void SetMediaContentsUserURLS(string[] urls)
    {
        this.user.mediaContentsURLS = urls;
    }








    /* ############################### USER ATTRIBUTE : MEDIACONTENTS IMAGES GETTERS-SETTERS-METHODS ################################ */

    public void SetNewMediaContentsImage(Sprite image)
    {
        this.user.mediaContentsImages.Add(image);
    }

    public void RemoveItemMediaContentsImage(int index)
    {
        this.user.mediaContentsImages.RemoveAt(index);
    }








    /* ############################ USER ATTRIBUTE : MEDIACONTENTSIMAGES PATH ORDER GETTERS-SETTERS-METHODS ############################ */

    public string GetMediaContentsImagePathOrder(int index)
    {
        return this.user.mediaContentsImagesPathsOrder[index];
    }

    public int GetIndexMediaContentImage(string path)
    {
        return this.user.mediaContentsImagesPathsOrder.IndexOf(path);
    }


    public void SetMediaContentsImagePathOrder(string path)
    {
        this.user.mediaContentsImagesPathsOrder.Add(path);
    }

    public void RemoveItemMediaContentsImagePathOrder(int index)
    {
        this.user.mediaContentsImagesPathsOrder.RemoveAt(index);
    }












    /* ############################## USER ATTRIBUTE : MEDIACONTENTSVIDEOS GETTERS-SETTERS-METHODS ############################## */

    public GameObject GetMediaContentsVideo(int index)
    {
        return this.user.mediaContentsVideos[index];
    }
    public void SetMediaContentsVideo(GameObject video)
    {
        this.user.mediaContentsVideos.Add(video);
    }
    public void RemoveItemMediaContentsVideo(int index)
    {
        this.user.mediaContentsVideos.RemoveAt(index);
    }




}


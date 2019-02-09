using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Networking
{
    [System.Serializable]
    public class Response
    {
        public bool success;
        public string message;
    }

    [System.Serializable]
    public class ResponseList
    {
        public bool success;
        public string[] message;
    }

    [System.Serializable]
    public class UserResponse
    {
        public string id;
        public string email;
        public string name;
        public string email_verified_at;
        public Sprite image;
        public string[] mediaContentsURLS;
        public List<Sprite> mediaContentsImages;
        public List<string> mediaContentsImagesPathsOrder;
        public List<GameObject> mediaContentsVideos;
    }
}
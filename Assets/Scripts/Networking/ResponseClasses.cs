﻿using System.Collections;
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
    public class UserResponse
    {
        public string id;
        public string email;
        public string name;
        public Sprite image;
    }
}
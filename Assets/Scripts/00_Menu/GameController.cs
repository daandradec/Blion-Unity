using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    /* VARIABLES REFERENCIA GAMEOBJECTS */
    public GameObject playerShip_1;
    public GameObject playerShip_2;

    /* ################################### INICIALIZACIÓN ################################### */

    private void Start()
    {
        GameObject netObject = GameObject.FindGameObjectWithTag("NetworkController");
        if (netObject != null)
        {
            Destroy(netObject);
        }
    }


    /* ################################### Metodos exclusivos del GameController ################################### */

    public void LoadSceneByName(string scene)
    {
        SceneManager.LoadScene(scene);
    }

}

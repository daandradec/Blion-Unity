using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController02 : MonoBehaviour {


    /* VARIABLE ATRIBUTO CONTROLADOR DE REDES */
    private NetworkController networkController;



    /* ################################### INICIALIZACIÓN ################################### */

    private void Awake()
    {
        this.networkController = GameObject.FindGameObjectWithTag("NetworkController").GetComponent<NetworkController>();
    }


    /* ################################### Metodos exclusivos del GameController02 ################################### */

    public NetworkController GetNetworkController()
    {
        return this.networkController;
    }

    public void LoadSceneByName(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}

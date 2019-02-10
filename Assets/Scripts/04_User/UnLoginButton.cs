using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnLoginButton : MonoBehaviour {

    /* VARIABLE ATRIBUTO REFERENCIA DEL GAMECONTROLLER DE LA ESCENA */
    public GameController04 gameController;




    /* ################################### METODO DEL EVENTO DE CLICK DEL BUTON DE DESLOGUEARSE ################################### */

    public void UnLogin()
    {
        DestroyPersistentVideos();
        gameController.LoadSceneByName("01_Login");
    }

    private void DestroyPersistentVideos()
    {
        foreach (GameObject video in this.gameController.GetNetworkController().GetPersistentObjects().GetUser().mediaContentsVideos)
        {
            Destroy(video);
        }
    }

}

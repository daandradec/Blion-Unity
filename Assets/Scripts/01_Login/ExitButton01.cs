using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton01 : MonoBehaviour {

    /* VARIABLE ATRIBUTO REFERENCIA DEL GAMECONTROLLER DE LA ESCENA */
    public GameController01 gameController;


    /* ################################### METODO PARA SALIR DE LA ESCENA ACTUAL HACIA LA 00 ################################### */

    public void Exit()
    {
        gameController.LoadSceneByName("00_Menu");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour {

    /* VARIABLE ATRIBUTO REFERENCIA DEL GAMECONTROLLER DE LA ESCENA */
    public GameController gameController;



    /* ################################### METODO DEL EVENTO DE CLICK DEL BUTON DE LOGIN ################################### */
    public void ButtonEventLogin()
    {
        gameController.playerShip_1.GetComponent<PlayerMovement>().MakeAnimation("01_Login");
    }


    /* ################################### METODO DEL EVENTO DE CLICK DEL BUTON DE REGISTER ################################### */
    public void ButtonEventRegister()
    {
        gameController.playerShip_2.GetComponent<PlayerMovement>().MakeAnimation("02_Registration");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    /* VARIABLE ATRIBUTO REFERENCIA DEL GAMECONTROLLER DE LA ESCENA */
    public GameController gameController;



    /* ################################### METODO QUE AL LLAMARSE EJECUTARA LA CORRUTINA MOVERIGHT ################################### */
    public void MakeAnimation(string sceneToChangeOnFinished)
    {
        StartCoroutine(MoveRight(sceneToChangeOnFinished));        
    }



    /* ##################### CORRUTINA QUE HACE LA ANIMACION DE MOVIMIENTO DE LA NAVE, Y AL TERMINAR CAMBIA LA ESCENA ##################### */
    private IEnumerator MoveRight(string sceneToChangeOnFinished)
    {
        bool flag = true;
        while (flag)
        {
            //float X = transform.position.x + 1f;
            transform.Translate(Vector3.forward * 6.4f * Time.deltaTime);
            if (transform.position.x >= 6f)
                flag = false;
            yield return new WaitForEndOfFrame();//new WaitForSeconds(0.5f);
        }
        
        gameController.LoadSceneByName(sceneToChangeOnFinished);
    }
}

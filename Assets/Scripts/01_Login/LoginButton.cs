using Assets.Scripts.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginButton : MonoBehaviour {

    /* VARIABLE ATRIBUTO REFERENCIA DEL GAMECONTROLLER DE LA ESCENA */
    public GameController01 gameController;



    /* ################################### METODO DEL EVENTO DE CLICK DEL BUTON DE LOGIN ################################### */


    /// <summary> #####################################################################################
    ///     Este metodo se encarga de obtener los strings de los campos de textos y hacer una peticion al servidor a la API de logueo de usuario
    ///     que recibe un email y un password
    /// </summary>
    public void Login()
    {
        string email = GameObject.Find("InputField Email").GetComponent<InputField>().text;
        string password = GameObject.Find("InputField Password").GetComponent<InputField>().text;

        var netController = gameController.GetNetworkController();
        var netURLS = netController.GetUrls();

        string url = netURLS.GetMainDomain() + netURLS.POST_LOGIN_API;
        string[] keys = {"email","password"};
        string[] values = {email,password};

        netController.PostRequestMethod(LoginUser, keys, values,  url);
    }



    /* ############################### METODO PARA DAR ACCESO A LA SIGUIENTE ESCENA DESPUES DE LOGUEARSE ############################### */



    /// <summary> #####################################################################################
    ///         Metodo que es llamado una vez se hace el request de login, y que obtiene la información del usuario y la convierte
    ///         en un objeto persistente de tipo UserResponse el cual se guarda en los objetos persistentes del NetoworkController, y
    ///         se pasa a la siguiente escena
    /// </summary>
    private int LoginUser(string answer)
    {
        Response response = JsonUtility.FromJson<Response>(answer);
        if (response.success)
        {            
            UserResponse user = JsonUtility.FromJson<UserResponse>(response.message);
            var netController = gameController.GetNetworkController();
            netController.GetPersistentObjects().SetCurrentUser(user);

            gameController.LoadSceneByName("03_Loading");
        }
        else
            gameController.GetNetworkController().LogRequestErrorMessage(response.message);    
            
        
        return 1;
    }
}

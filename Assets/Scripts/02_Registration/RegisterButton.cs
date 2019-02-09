using Assets.Scripts.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterButton : MonoBehaviour {

    /* VARIABLE ATRIBUTO REFERENCIA DEL GAMECONTROLLER DE LA ESCENA */
    public GameController02 gameController;


    /* ################################### METODO DEL EVENTO DE CLICK DEL BUTON DE REGISTRO ################################### */


    /// <summary> #####################################################################################
    ///     Este metodo se encarga de obtener los strings de los campos de textos y hacer una peticion al servidor a la API de registro de
    ///     usuario que recibe un nombre, email, un password, y una confirmación de password
    /// </summary>
    public void Register()
    {
        string name = GameObject.Find("InputField Name").GetComponent<InputField>().text;
        string email = GameObject.Find("InputField Email").GetComponent<InputField>().text;
        string password = GameObject.Find("InputField Password").GetComponent<InputField>().text;
        string password_confirmation = GameObject.Find("InputField PasswordConfirm").GetComponent<InputField>().text;

        var netController = gameController.GetNetworkController();
        var netURLS = netController.GetUrls();

        string url = netURLS.GetMainDomain() + netURLS.POST_REGISTER_API;
        string[] keys = { "name","email", "password" , "password_confirmation" };
        string[] values = { name, email, password, password_confirmation };

        netController.PostRequestMethod(RegisterUser, keys, values, url);
    }





    /* ############################### METODO PARA DAR ACCESO A LA SIGUIENTE ESCENA DESPUES DE REGISTRARSE ############################### */



    /// <summary> #####################################################################################
    ///         Metodo que es llamado una vez se hace el request de registro, y que obtiene la información del usuario y la convierte
    ///         en un objeto persistente de tipo UserResponse el cual se guarda en los objetos persistentes del NetoworkController, y
    ///         se pasa a la siguiente escena
    /// </summary>
    public int RegisterUser(string answer)
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

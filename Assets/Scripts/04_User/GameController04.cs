using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController04 : MonoBehaviour {

    /* VARIABLE ATRIBUTO CONTROLADOR DE REDES */
    private NetworkController networkController;

    /* VARIABLE ATRIBUTO PANEL DE ARCHIVOS */
    private FilePanel filePanel;

    /* VARIABLE ATRIBUTO REFERENCIA A LA IMAGEN EN LA ESCENA DE LA FOTO DE PERFIL DEL USUARIO */
    public Image userImage;

    /* VARIABLE ATRIBUTO REFERENCIA DEL TEXTO QUE MUESTRA EL EMAIL (TEXTO DE TIPO TEXTMESHPRO) */
    public TextMeshProUGUI textField;




    /* ################################### INICIALIZACIÓN ################################### */

    private void Awake()
    {
        this.networkController = GameObject.FindGameObjectWithTag("NetworkController").GetComponent<NetworkController>();
        this.filePanel = this.GetComponent<FilePanel>();       
    }

    private void Start()
    {
        textField.text = "email: " + networkController.GetPersistentObjects().GetUser().email;
        userImage.sprite = networkController.GetPersistentObjects().GetUser().image;
        userImage.preserveAspect = true;
    }




    /* ################################### Metodos exclusivos del GameController04 ################################### */

    public NetworkController GetNetworkController()
    {
        return this.networkController;
    }

    public FilePanel GetFilePanel()
    {
        return this.filePanel;
    }

    public void LoadSceneByName(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}

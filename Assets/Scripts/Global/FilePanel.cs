using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilePanel : MonoBehaviour {

    /* VARIABLE ATRIBUTO REFERENCIA DEL COMPONENTE DEL FILEBROWSERANDROID */
    public OpenFileBrowserAndroid openFileManager;

    /* VARIABLE ATRIBUTO REFERENCIA DEL FILEMANAGER EN LA ESCENA */
    public FileManager fileManager;



    /* ################## METODO PARA BUSCAR UN ARCHIVO USANDO EL PANEL DE BUSQUEDA DE ARCHIVOS POR DEFECTO DE UNITY ################## */
    public string SearchImage()
    {
        #if UNITY_EDITOR
        return UnityEditor.EditorUtility.OpenFilePanel("Select an image", "", "png,jpg,jpeg,mp4");
#else
        return "C:\\Users\\USUARIO\\Pictures\\";
#endif
    }

    /* ################## METODO PARA BUSCAR UN ARCHIVO USANDO EL ASSET DEL FILEBROWSERANDROID (PROPIO) ################## */
    public void SearchImageWithFileManager(Func<string, int> Method)
    {
        fileManager.SetExecuteFunction(Method);
        openFileManager.OpenFileBrowser();
    }
}

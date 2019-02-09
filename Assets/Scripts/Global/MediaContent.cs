using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediaContent : MonoBehaviour {

    /* VARIABLES ATRIBUTOS DE PATH E INDICE */
    private string path;
    private int index;

    /* GETTER DEL PATH */
    public string GetAsociatedPath()
    {
        return this.path;
    }
    /* GETTER DEL INDICE */
    public int GetIndexOnList()
    {
        return index;
    }

    /* SETTER DEL PATH */
    public void SetAssociatedPath(string path)
    {
        this.path = path;
    }

    /* SETTER DE PATH E INDICE*/
    public void SetAssociatedPathAndIndex( string path, int index )
    {
        this.path = path;
        this.index = index;
    }

    /* SETTER DEL INDICE */
    public void SetAssociatedIndex(int index)
    {
        this.index = index;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Persistent : MonoBehaviour {

    /* ############################### AÑADIR SCRIPT A UN OBJETO PARA HACERLO PERSISTENTE DURANTE EL JUEGO ############################### */

    /// <summary> #####################################################################################
    ///         Al inicializarse usa el DontDestroyOnLoad para que sea un objeto persistente en todas las escenas, y para que no se dupliquen
    ///         estos objetos al volver a una escena donde se instancia un objeto que tiene este script entonces pregunta si el tipo de objeto
    ///         (que al tener este script su getType es persistent) al buscar otros mas la cantidad es mayor a 1 entonces destruyalo. Cosa que
    ///         no se ejecutara en el que ya es persistente, puesto que ya se inicializo
    /// </summary>  
    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }

    }
}

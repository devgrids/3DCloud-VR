using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class dalDocente : MonoBehaviour
{
    public static dalDocente sharedInstance;
    public Docente docente;

    #region UNITY Methods

    private void Awake()
    {
        sharedInstance = this;
    }

    void Start()
    {

    }

    void Update()
    {

    }

    #endregion

    #region UI Callback Methods

    public void guardarDocente()
    {
        Docente obj = new Docente();
        

        string json = JsonUtility.ToJson(obj);
    }

    public void obtenerDocente()
    {
        //StartCoroutine(IE_obtenerCuenta());
    }

    #endregion


    #region IEnumerator Callback Methods


    #endregion

}

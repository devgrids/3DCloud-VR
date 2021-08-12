using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
   
    public static LoginManager sharedInstance;

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

    

    #endregion

    #region IEnumerator Callback Methods

    

    

    #endregion

    #region Photon Callback Methods


    #endregion

}

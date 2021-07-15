using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
//using TMPro;

public class LoginManager : MonoBehaviourPunCallbacks
{
    //public TMP_InputField PlayerName_InputField;
    public InputField PlayerName_InputField;
    [SerializeField] private bool VR;

    [SerializeField] private GameObject GUI_VR;
    [SerializeField] private GameObject GUI_PC;

    [SerializeField] private GameObject GUI;

    #region UNITY Methods

    void Start()
    {
        //PhotonNetwork.ConnectUsingSettings();

        if(VR)
        {
            GUI.transform.SetParent(GUI_VR.transform);
        }
        else
        {
            GUI.transform.SetParent(GUI_PC.transform);
        }

    }

    void Update()
    {
    }

    #endregion

    #region UI Callback Methods

    public void ConnectAnonymously()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public void ConnectToPhotonServer()
    {
        if (PlayerName_InputField != null)
        {
            PhotonNetwork.NickName = PlayerName_InputField.text;
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    #endregion

    #region Photon Callback Methods
    public override void OnConnected()
    {
        Debug.Log("OnConnected is called. The server is available.");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to the Master Server with player name: " + PhotonNetwork.NickName);
        PhotonNetwork.LoadLevel("Home");
    }

    #endregion
}

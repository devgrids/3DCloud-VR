using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ConectionManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private float versionJuego;

    void Start()
    {
        PhotonNetwork.GameVersion = versionJuego.ToString();
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Conectado al servidor " + PhotonNetwork.CloudRegion);
    }
}

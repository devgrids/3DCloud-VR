using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeManager : MonoBehaviourPunCallbacks
{
    // Como pueden haber varios jugadores creamos un gameobject serializable
    [SerializeField]
    GameObject genericPlayer;

    [SerializeField]
    GameObject cube;

    [SerializeField]
    Vector3 spawnPlayer;

    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (genericPlayer != null)
            {
                // Instanciar al jugador para todos los jugadores en la sala en una posición al azar al inicio de la escena
                PhotonNetwork.Instantiate(genericPlayer.name, spawnPlayer, Quaternion.identity);
            }
        }

        //if (!PhotonNetwork.IsMasterClient)
        //{
        //    PhotonNetwork.Instantiate(cube.name, spawnPlayer + new Vector3(2, 0, 2), Quaternion.identity);
        //}
    }

    void Update()
    {

    }

    public override void OnJoinedRoom()
    {
        
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        //PhotonNetwork.PlayerList;
        Debug.Log("Se ha unido un jugador");
        //listaDeJugadores.
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) 
        {
            Debug.Log(PhotonNetwork.PlayerList[i].IsMasterClient);
        }

        Debug.Log("Cantidad: " + PhotonNetwork.PlayerList.Length);

        //PhotonNetwork.

        //Actualizarse al jugador p :v

    }

    public void debugGUI()
    {
        Debug.Log("Evento Debug Activado");
    }

}

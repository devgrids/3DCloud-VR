using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

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
       
            if (genericPlayer != null)
            {
                // Instanciar al jugador para todos los jugadores en la sala en una posición al azar al inicio de la escena
                //PhotonNetwork.Instantiate(genericPlayer.name, spawnPlayer, Quaternion.identity);

                GameObject player = PhotonNetwork.Instantiate(genericPlayer.name, spawnPlayer, Quaternion.identity, 0);
                BNG.NetworkPlayer np = player.GetComponent<BNG.NetworkPlayer>();
                if (np)
                {
                    Debug.Log("Instanciando DEL PUN VRIF :V VVVV");
                    np.transform.name = "MyRemotePlayer";
                    np.AssignPlayerObjects();
                }

            }
        
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

using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomSelect : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Text textNombre;
    [SerializeField]
    private Text textCapacidad;
    [SerializeField]
    private Image imagen;

    private string roomName;
    private int roomSize;
    private int playerCount;
    private int indexImagen;

    // Método enlazado con el botón que une al jugador a una sala
    public void JoinRoomOnClick()
    {

        LobbyManager.roomNameContenedor = roomName;
        LobbyManager.roomSizeContenedor = roomSize;
        LobbyManager.playerCountContenedor = playerCount;
        LobbyManager.indexImagenContenedor = indexImagen;

        LobbyManager.sharedInstance.CreateRoom();

        Debug.Log("ROOM NAME: " + roomName);
        Debug.Log("ROOM SIZE: " + roomSize);
        Debug.Log("PLAYER COUNT: " + playerCount);
        Debug.Log("INDEX IMAGEN: " + indexImagen);
    }

    // Se llama por el controlador de lobbys para cada nueva sala que se añade a la lista
    public void SetRoom(string nombreIngresado, int capacidad, int cantidad, int indexImagen)
    {
        this.roomName = nombreIngresado;
        roomSize = capacidad;
        playerCount = cantidad;
        this.indexImagen = indexImagen;
        textNombre.text = nombreIngresado;
        textCapacidad.text = cantidad + "/" + capacidad;
        imagen.sprite = LobbyManager.sharedInstance.imageList[indexImagen];
    }

    

}

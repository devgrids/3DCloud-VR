using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public static LobbyManager sharedInstance;

    [SerializeField] Transform contenedorRoomDocente;
    [SerializeField] Transform contenedorRoomEstudiante;

    private List<RoomInfo> roomLista;
    public GameObject prefabRoomList;

    [SerializeField] InputField inputNombreRoom;
    [SerializeField] InputField inputCapacidadRoom;

    public List<Sprite> imageList;
    public static int indexImagen = 0;

    public static string roomNameContenedor;
    public static int roomSizeContenedor;
    public static int playerCountContenedor;
    public static int indexImagenContenedor;

    #region UNITY Methods
    private void Awake()
    {
        sharedInstance = this;
    }

    private void Start()
    {
        
    }
    #endregion

    #region Photon Callback Methods

    // Si la conexión fue establecida
    public override void OnConnectedToMaster()
    {
        // Esto indica que todos los jugadores de la sala usarán la misma el jugador master (el que crea la sala) o Master Client
        PhotonNetwork.AutomaticallySyncScene = true;
        roomLista = new List<RoomInfo>();
        Debug.Log("Conectando master");
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Curso creado correctamente");
    }
    // Si no se puede unir a la sala al azar
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Error en Join Random Falider");
       
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Error en unirse a ROOM");
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Lobby conectado");
    }

    public override void OnCreateRoomFailed(short returnCode, string message) //si la sala existe
    {
        Debug.Log("Fallo en crear una nueva sala, seguramente ya existe una sala con ese nombre.");
        CreateRoom();
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("Te uniste a la Sala: " + PhotonNetwork.CurrentRoom.Name);
            Debug.Log("La sala cuenta con: " + PhotonNetwork.CurrentRoom.PlayerCount + " jugador(es).");
            Debug.Log("INDEX: " + (indexImagenContenedor + 1));
            PhotonNetwork.LoadLevel((indexImagenContenedor + 1));
            Debug.Log("Escena creada");
        }
    }

    #endregion

    #region UI Callback Methods

    // Predicate, es un método C# que contiene un set de criterios de búsqueda y devuelve un boolean
    // Está definido en el System namespace 
    // Método que buscan una sala en la lista de salas
    static System.Predicate<RoomInfo> ByName(string name)
    {
        return delegate (RoomInfo room)
        {
            return room.Name == name;
        };
    }

    public void CreateRoom()
    {
        Debug.Log("Creando nueva sala: " + roomNameContenedor);
        RoomOptions roomOptions = new RoomOptions()
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = (byte)roomSizeContenedor
        };

        PhotonNetwork.JoinOrCreateRoom(roomNameContenedor, roomOptions, TypedLobby.Default); // Creación de una nueva sala

    }

    void BorrarSalasdeLista()
    {
        Transform contenedorRoom = dalCuenta.isDocente ? contenedorRoomDocente : contenedorRoomEstudiante;
        for (int i = contenedorRoom.childCount - 1; i >= 0; i--)
        {
            Destroy(contenedorRoom.GetChild(i).gameObject);
        }
    }

    void ListRoom(RoomInfo room) //Muestra la nueva lista de salas para la sala actual
    {
        if (room.IsOpen && room.IsVisible)
        {
            Transform contenedorRoom = dalCuenta.isDocente ? contenedorRoomDocente : contenedorRoomEstudiante;
            GameObject card = Instantiate(prefabRoomList, contenedorRoom);
            RoomSelect scriptRoomSelect = card.GetComponent<RoomSelect>();
            scriptRoomSelect.SetRoom(room.Name, room.MaxPlayers, room.PlayerCount, indexImagen);
        }
    }

    public void ContenedorListaCursos()
    {
        Transform contenedorRoom = dalCuenta.isDocente ? contenedorRoomDocente : contenedorRoomEstudiante;
        GameObject card = Instantiate(prefabRoomList, contenedorRoom);
        RoomSelect scriptRoomSelect = card.GetComponent<RoomSelect>();
        scriptRoomSelect.SetRoom(inputNombreRoom.text, int.Parse(inputCapacidadRoom.text), 0, indexImagen);
    }

    public void AddContenedorListaCurso(string nombre, int capacidad, int index)
    {
        Transform contenedorRoom = dalCuenta.isDocente ? contenedorRoomDocente : contenedorRoomEstudiante;
        GameObject card = Instantiate(prefabRoomList, contenedorRoom);
        RoomSelect scriptRoomSelect = card.GetComponent<RoomSelect>();
        scriptRoomSelect.SetRoom(nombre, capacidad, 0, index);
    }

    #endregion

    

    

}

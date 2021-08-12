using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace HurricaneVR.Framework.Integrations.PUN
{
    public class HVRPUNManager : MonoBehaviourPunCallbacks
    {
        public GameObject Prefab;
        public GameObject Player;

        void Start()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log($"Connected To Server.");
            base.OnConnectedToMaster();
            PhotonNetwork.JoinOrCreateRoom("test", new RoomOptions()
            {
                MaxPlayers = 5,
                IsVisible = true,
                IsOpen = true
            }, TypedLobby.Default);
        }

        public override void OnJoinedRoom()
        {
            Debug.Log($"spawning");
            Player = PhotonNetwork.Instantiate(Prefab.name, transform.position, transform.rotation);
            base.OnJoinedRoom();
        }

        public override void OnLeftRoom()
        {
            Debug.Log($"destroying");
            base.OnLeftRoom();
            PhotonNetwork.Destroy(Player);
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.Log($"OnPlayerLeftRoom");
            base.OnPlayerLeftRoom(otherPlayer);
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log($"OnDisconnected");
            base.OnDisconnected(cause);
        }
    }
}

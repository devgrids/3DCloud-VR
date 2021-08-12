using System;
using UnityEngine;
using Photon.Pun;

namespace Player
{
    public class PlayerNetwork : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;
        
        private PhotonView _photonView;

        private void Awake()
        {
            _photonView = GetComponent<PhotonView>();
        }

        void Update()
        {
            if (_photonView.IsMine)
            {
                playerController.Enabled();
            }
            else
            {
                playerController.Disabled();
            }
        }
        
    }
}
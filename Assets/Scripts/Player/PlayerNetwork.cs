using System;
using UnityEngine;
using Photon.Pun;


namespace Player
{
    [RequireComponent(typeof(PhotonView))]
    public class PlayerNetwork : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;


        private PhotonView _photonView;

        private void Awake()
        {
            _photonView = GetComponent<PhotonView>();

        }

        private void Start()
        {

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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using BNG;

public class NetworkPlayerController : MonoBehaviourPunCallbacks
{
    private GameObject player;
    private SmoothLocomotion _smoothLocomotion;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
        {

            if (_smoothLocomotion)
            {
                _smoothLocomotion.enabled = false;
                Debug.Log("Movimineto desactivado");
            }

            player.transform.position = ChairPosition.sharedInstance.GetPositionPlayer();
        }
    }

    private void Update()
    {
        
    }

    public void AssignPlayerControllers()
    {
        _smoothLocomotion = player.GetComponent<SmoothLocomotion>();
    }

}

using System;
using Photon.Pun.Demo.PunBasics;
using UnityEngine;
using UnityEngine.SpatialTracking;
using HurricaneVR.Framework.Core.Player;

namespace Player
{
    // [DefaultExecutionOrder(-100)]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private HVRPlayerController _hvrPlayerController;

        [SerializeField] private TrackedPoseDriver _trackedPoseDriver_Left;
        [SerializeField] private TrackedPoseDriver _trackedPoseDriver_Right;

        private void Awake()
        {

        }

        public void Enabled()
        {
            _camera.enabled = true;
            _hvrPlayerController.enabled = true;
            _trackedPoseDriver_Left.enabled = true;
            _trackedPoseDriver_Right.enabled = true;
        }

        public void Disabled()
        {
            _camera.enabled = false;
            _hvrPlayerController.enabled = false;
            _trackedPoseDriver_Left.enabled = false;
            _trackedPoseDriver_Right.enabled = false;
        }



    }
}
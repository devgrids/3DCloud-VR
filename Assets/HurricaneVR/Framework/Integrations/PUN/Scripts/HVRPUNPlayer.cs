using System;
using System.Linq;
using HurricaneVR.Framework.Core;
using HurricaneVR.Framework.Core.Grabbers;
using HurricaneVR.Framework.Core.Player;
using HurricaneVR.Framework.Shared.HandPoser;
using Photon.Pun;
using UnityEngine;

namespace HurricaneVR.Framework.Integrations.PUN
{
    [RequireComponent(typeof(PhotonView))]
    public class HVRPUNPlayer : MonoBehaviour, IPunObservable
    {
        public PhotonView PhotonView { get; private set; }

        public HVRPlayerController PlayerController;
        public HVRPUNPlayerInputs PunInputs;

        private void Awake()
        {

        }

        void Start()
        {
            PlayerController = GetComponentInChildren<HVRPlayerController>();
            PhotonView = GetComponent<PhotonView>();

            PunInputs = GetComponent<HVRPUNPlayerInputs>();
            PunInputs.PlayerInputs = PlayerController.Inputs;
            //PhotonView.observableSearch = PhotonView.ObservableSearch.Manual;
            //PhotonView.ObservedComponents.Add(PunInputs);

            var handGrabbers = GetComponentsInChildren<HVRHandGrabber>().ToList();
            var handAnimators = GetComponentsInChildren<HVRHandAnimator>().ToList();
            var sockets = GetComponentsInChildren<HVRSocket>().ToList();
            var cameraRig = GetComponentInChildren<HVRCameraRig>();


            if (!PhotonView.IsMine)
            {
                PlayerController.RemoveMultiplayerComponents();
                PlayerController.Inputs.UpdateInputs = false;
                handAnimators.ForEach(e => e.IsMine = false);
                handGrabbers.ForEach(e =>
                {
                    e.IsMine = false;
                    e.PerformUpdate = false;
                });
                sockets.ForEach(e =>
                {
                    e.IsMine = false;
                    e.PerformUpdate = false;
                    e.CanInteract = false;
                });

                if (cameraRig)
                    cameraRig.IsMine = false;
            }
            else
            {
                HVRManager.Instance.PlayerController = GetComponentInChildren<HVRPlayerController>();
                HVRManager.Instance.Camera = GetComponentInChildren<HVRCamera>()?.transform;

                PlayerController.Inputs.UpdateInputs = true;

                foreach (var handGrabber in handGrabbers)
                {
                    var jointHand = handGrabber.gameObject.GetComponent<HVRJointHand>();
                    if (jointHand)
                    {
                        jointHand.MaxDistanceReached.AddListener(handGrabber.ForceRelease);
                    }
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(PlayerController.CameraRig.CameraYOffset);
                stream.SendNext(PlayerController.CameraRig.PlayerControllerYOffset);
            }
            else
            {
                PlayerController.CameraRig.CameraYOffset = (float)stream.ReceiveNext();
                PlayerController.CameraRig.PlayerControllerYOffset = (float)stream.ReceiveNext();
            }
        }
    }
}


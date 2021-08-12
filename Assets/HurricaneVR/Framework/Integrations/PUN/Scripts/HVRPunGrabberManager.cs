using System;
using ExitGames.Client.Photon;
using HurricaneVR.Framework.Components;
using HurricaneVR.Framework.Core;
using HurricaneVR.Framework.Core.Grabbers;
using HurricaneVR.Framework.Shared;
using HurricaneVR.Framework.Shared.HandPoser.Data;
using Photon.Pun;
//using Smooth;
using UnityEngine;

namespace HurricaneVR.Framework.Integrations.PUN
{
    [RequireComponent(typeof(PhotonView))]
    public class HVRPunGrabberManager : HVRGrabberManager
    {
        public PhotonView PhotonView { get; private set; }


        protected override void Awake()
        {
            base.Awake();

            PhotonView = GetComponent<PhotonView>();
        }

        protected override void OnBeforeGrabberGrabbed(HVRGrabberBase grabber, HVRGrabbable grabbable)
        {
            base.OnBeforeGrabberGrabbed(grabber, grabbable);


        }

        protected override void OnGrabberGrabbed(HVRGrabberBase grabber, HVRGrabbable grabbable)
        {
            base.OnGrabberGrabbed(grabber, grabbable);

            if (!PhotonNetwork.InRoom)
                return;


            var grabbableView = grabbable.GetComponent<PhotonView>();
            var grabberView = grabber.GetComponent<PhotonView>();

            if (!grabberView || !grabbableView)
            {
                return;
            }

            if (grabber is HVRHandGrabber handGrabber && grabberView.IsMine)
            {
                if (!grabbableView.IsMine)
                    grabbableView.RequestOwnership();
                OnHandGrabberGrabbed(handGrabber, grabbable, grabberView, grabbableView);
            }
            else if (grabber is HVRSocket socket && socket.CanInteract && grabbableView.IsMine)
            {
                OnSocketGrabbed(socket, grabbable, grabberView, grabbableView);
            }
            else if (grabber is HVRForceGrabber forceGrabber && grabberView.IsMine)
            {
                if (!grabbableView.IsMine)
                    grabbableView.RequestOwnership();
                OnForceGrabbed(forceGrabber, grabbable, grabberView, grabbableView);
            }
        }

        private void OnForceGrabbed(HVRForceGrabber forceGrabber, HVRGrabbable grabbable, PhotonView grabberView, PhotonView grabbableView)
        {
            //PhotonView.RPC("PunForceGrab", RpcTarget.Others, grabberView.ViewID, grabbableView.ViewID);
        }

        [PunRPC]
        private void PunForceGrab(int grabberId, int grabbableId)
        {
            Debug.Log($"PunForceGrab");
            var grabberView = PhotonView.Find(grabberId);
            var grabbableView = PhotonView.Find(grabbableId);

            if (!grabberView || !grabbableView)
                return;

            var grabber = grabberView.gameObject.GetComponent<HVRForceGrabber>();
            var grabbable = grabbableView.gameObject.GetComponent<HVRGrabbable>();

            if (!grabbable || !grabber)
                return;

            EnableDisableView(grabbable, false);

            StartCoroutine(grabber.ForceGrab(grabbable));
            //HVRGrabberBase.GrabGrabbable(grabber, grabbable, false);
        }

        private void OnSocketGrabbed(HVRSocket socket, HVRGrabbable grabbable, PhotonView grabberView, PhotonView grabbableView)
        {
            PhotonView.RPC("PunSocketGrab", RpcTarget.Others, grabberView.ViewID, grabbableView.ViewID);
        }

        [PunRPC]
        private void PunSocketGrab(int grabberId, int grabbableId)
        {
            Debug.Log($"PunSocketGrab");
            var grabberView = PhotonView.Find(grabberId);
            var grabbableView = PhotonView.Find(grabbableId);

            if (!grabberView || !grabbableView)
                return;

            var grabber = grabberView.gameObject.GetComponent<HVRSocket>();
            var grabbable = grabbableView.gameObject.GetComponent<HVRGrabbable>();

            if (!grabbable || !grabber)
                return;

            //sockets aren't owned by anyone so their logic is already firing - get out if already grabbed 
            if (grabber.GrabbedTarget == grabbable)
                return;

            EnableDisableView(grabbable, false);

            grabbable.PrimaryGrabber?.CheckForceRelease(grabbable);

            HVRGrabberBase.GrabGrabbable(grabber, grabbable, false);
        }

        private void OnHandGrabberGrabbed(HVRHandGrabber grabber, HVRGrabbable grabbable, PhotonView grabberView, PhotonView grabbableView)
        {
            int grabPointIndex = -1;
            if (grabber.GrabPoint)
                grabPointIndex = grabbable.GrabPoints.IndexOf(grabber.GrabPoint);

            if (grabber.IsPhysicsPose)
            {
                PhotonView.RPC("PunPhysicsHandGrab", RpcTarget.Others,
                    grabberView.ViewID,
                    grabbableView.ViewID,
                    grabber.GetPoseData(),
                    grabber.TempGrabPoint.transform.localPosition,
                    grabber.PhysicsHandRotation,
                    grabber.PhysicsHandPosition
                    );
            }
            else
            {
                PhotonView.RPC("PunHandGrab", RpcTarget.Others, grabberView.ViewID, grabbableView.ViewID, grabPointIndex);
            }

        }


        [PunRPC]
        private void PunPhysicsHandGrab(int grabberId, int grabbableId, byte[] poseData, Vector3 grabPointPosition, Quaternion poseRotation, Vector3 posePosition)
        {
            Debug.Log($"PunPhysicsHandGrab");
            var grabberView = PhotonView.Find(grabberId);
            var grabbableView = PhotonView.Find(grabbableId);

            if (!grabberView || !grabbableView)
                return;

            var grabber = grabberView.gameObject.GetComponent<HVRHandGrabber>();
            var grabbable = grabbableView.gameObject.GetComponent<HVRGrabbable>();

            if (!grabbable || !grabber)
                return;


            EnableDisableView(grabbable, false);

            grabbable.PrimaryGrabber?.CheckForceRelease(grabbable);

            var grabPoint = new GameObject(name + " PhysicsGrabPoint");
            grabPoint.transform.parent = grabbable.transform;
            grabPoint.transform.localRotation = Quaternion.identity;
            grabPoint.transform.localPosition = grabPointPosition;
            grabber.TempGrabPoint = grabPoint; //used to track deleting temp grab points on release
            grabber.GrabPoint = grabPoint.transform;


            grabbable.InternalOnBeforeGrabbed(grabber);
            grabber.GrabbedTarget = grabbable;
            grabber.PhysicsHandRotation = poseRotation;
            grabber.PhysicsHandPosition = posePosition;

            grabber.NetworkPhysicsGrab(grabbable);
            grabbable.InternalOnGrabbed(grabber);
            grabber.InternalOnAfterGrabbed(grabbable);

            try
            {
                grabber.PoseHand(poseData);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured while deserializing hand pose.");
                Debug.LogException(e);
            }
        }

        [PunRPC]
        private void PunHandGrab(int grabberId, int grabbableId, int grabPointIndex)
        {
            Debug.Log($"PunHandGrab");
            var grabberView = PhotonView.Find(grabberId);
            var grabbableView = PhotonView.Find(grabbableId);

            if (!grabberView || !grabbableView)
                return;

            var grabber = grabberView.gameObject.GetComponent<HVRHandGrabber>();
            var grabbable = grabbableView.gameObject.GetComponent<HVRGrabbable>();

            if (!grabbable || !grabber)
                return;


            EnableDisableView(grabbable, false);

            grabbable.PrimaryGrabber?.CheckForceRelease(grabbable);

            var grabPoint = grabPointIndex >= 0 && grabPointIndex < grabbable.GrabPoints.Count ? grabbable.GrabPoints[grabPointIndex] : grabbable.transform;
            grabber.GrabPoint = grabPoint;
            grabbable.InternalOnBeforeGrabbed(grabber);
            grabber.GrabbedTarget = grabbable;
            grabber.NetworkGrab(grabbable);
            grabbable.InternalOnGrabbed(grabber);
            grabber.InternalOnAfterGrabbed(grabbable);
        }

        //todo make sure this is handled if someone disconnects while holding something
        protected override void OnGrabberReleased(HVRGrabberBase grabber, HVRGrabbable grabbable)
        {
            base.OnGrabberReleased(grabber, grabbable);

            if (!PhotonNetwork.InRoom)
                return;

            var grabberView = grabber.GetComponent<PhotonView>();
            var grabbableView = grabbable.GetComponent<PhotonView>();

            if (!grabberView || !grabbableView || !grabberView.IsMine)
                return;

            PhotonView.RPC("PunOnGrabberReleased", RpcTarget.Others, grabberView.ViewID, grabbableView.ViewID);
        }



        [PunRPC]
        private void PunOnGrabberReleased(int grabberId, int grabbableId)
        {
            Debug.Log($"PunOnGrabberReleased");
            var grabberView = PhotonView.Find(grabberId);
            var grabbableView = PhotonView.Find(grabbableId);

            if (!grabberView || !grabbableView)
                return;

            var grabber = grabberView.gameObject.GetComponent<HVRGrabberBase>();
            var grabbable = grabbableView.gameObject.GetComponent<HVRGrabbable>();

            

            if (grabber && grabbable && grabber.GrabbedTarget == grabbable)
            {
                HVRGrabberBase.ReleaseGrabbable(grabber, grabbable, false);
            }

            if (grabbable && !grabbable.IsBeingHeld)
            {
                EnableDisableView(grabbable, true);
            }
        }

        private static void EnableDisableView(HVRGrabbable grabbable, bool enable)
        {
            var rigidView = grabbable.GetComponent<PhotonRigidbodyView>();
            if (rigidView)
            {
                rigidView.enabled = enable;
            }

            var transformView = grabbable.GetComponent<PhotonTransformView>();
            if (transformView)
            {
                transformView.enabled = enable;
            }

            //var smooth = grabbable.GetComponent<SmoothSyncPUN2>();
            //if (smooth)
            //{
            //    if (enable)
            //    {
            //        smooth.syncRotation = SyncMode.XYZ;
            //        smooth.syncAngularVelocity = SyncMode.XYZ;
            //        smooth.syncPosition = SyncMode.XYZ;
            //        smooth.syncScale = SyncMode.XYZ;
            //        smooth.syncVelocity = SyncMode.XYZ;
            //    }
            //    else
            //    {
            //        smooth.syncRotation = SyncMode.NONE;
            //        smooth.syncAngularVelocity = SyncMode.NONE;
            //        smooth.syncPosition = SyncMode.NONE;
            //        smooth.syncScale = SyncMode.NONE;
            //        smooth.syncVelocity = SyncMode.NONE;
            //    }
            //}
        }
    }
}
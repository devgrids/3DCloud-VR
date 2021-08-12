﻿using HurricaneVR.Framework.Components;
using Photon.Pun;
using UnityEngine;

namespace HurricaneVR.Framework.Integrations.PUN
{
    public class HVRPunLever : HVRLever, IPunObservable
    {

        public bool OthersRaiseEvents = true;

        public PhotonView PhotonView { get; private set; }


        private void Awake()
        {
            PhotonView = GetComponent<PhotonView>();
        }

        protected override void FixedUpdate()
        {
            if (PhotonView.IsMine)
                base.FixedUpdate();
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                if (RotationTarget)
                {
                    stream.SendNext(RotationTarget.localRotation);
                }

                stream.SendNext(transform.localRotation);
                stream.SendNext(CurrentAngle);
                stream.SendNext(CurrentStep);
            }
            else
            {
                if (RotationTarget)
                {
                    RotationTarget.localRotation = (Quaternion)stream.ReceiveNext();
                }

                transform.localRotation = (Quaternion)stream.ReceiveNext();
                CurrentAngle = (float)stream.ReceiveNext();
                CurrentStep = (int)stream.ReceiveNext();
            }
        }

        protected override void OnStepChanged(int step, bool raiseEvents = true)
        {
            if (PhotonView.IsMine)
            {
                base.OnStepChanged(step, true);
                PhotonView.RPC("PunOnStepChanged", RpcTarget.Others, step, OthersRaiseEvents);
            }
        }

        protected override void OnAngleChanged(float angle, float delta, float percent, bool raiseEvents = true)
        {
            if (PhotonView.IsMine)
            {
                base.OnAngleChanged(angle, delta, percent, raiseEvents);
                PhotonView.RPC("PunOnAngleChanged", RpcTarget.Others, angle, delta, percent, OthersRaiseEvents);
            }
        }

        [PunRPC]
        protected virtual void PunOnStepChanged(int step, bool raiseEvents)
        {
            base.OnStepChanged(step, raiseEvents);
        }

        [PunRPC]
        protected virtual void PunOnAngleChanged(float angle, float delta, float percent, bool raiseEvents)
        {
            base.OnAngleChanged(angle, delta, percent, raiseEvents);
        }
    }
}
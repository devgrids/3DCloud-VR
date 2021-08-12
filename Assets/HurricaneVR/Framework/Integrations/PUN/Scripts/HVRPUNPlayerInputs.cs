using HurricaneVR.Framework.ControllerInput;
using Photon.Pun;
using UnityEngine;

namespace HurricaneVR.Framework.Integrations.PUN
{
    public class HVRPUNPlayerInputs : MonoBehaviourPunCallbacks, IPunObservable
    {
        public HVRPlayerInputs PlayerInputs;

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (!PlayerInputs)
                return;

            //if (stream.IsWriting)
            //{
            //    stream.SendNext(PlayerInputs.MovementAxis);
            //    stream.SendNext(PlayerInputs.TurnAxis);
            
            //    stream.SendNext(PlayerInputs.IsTeleportActivated);
            //    stream.SendNext(PlayerInputs.IsTeleportDeactivated);
            //    stream.SendNext(PlayerInputs.IsSprintingActivated);
            //    stream.SendNext(PlayerInputs.SprintRequiresDoubleClick);
            //    stream.SendNext(PlayerInputs.IsCrouchActivated);
            //}
            //else
            //{
            //    PlayerInputs.MovementAxis = (Vector2) stream.ReceiveNext();
            //    PlayerInputs.TurnAxis = (Vector2) stream.ReceiveNext();

            //    PlayerInputs.IsTeleportActivated = (bool) stream.ReceiveNext();
            //    PlayerInputs.IsTeleportDeactivated = (bool)stream.ReceiveNext();
            //    PlayerInputs.IsSprintingActivated = (bool)stream.ReceiveNext();
            //    PlayerInputs.SprintRequiresDoubleClick = (bool)stream.ReceiveNext();
            //    PlayerInputs.IsCrouchActivated = (bool)stream.ReceiveNext();
            //}
        }
    }
}

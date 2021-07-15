using System.Collections;
using UnityEngine;

namespace HurricaneVR.Framework.Core.Player
{
    public class HVRCameraRig : MonoBehaviour
    {
        [Header("Required Transforms")]
        public Transform Camera;
        public Transform FloorOffset;
        public Transform CameraScale;

        [Header("Manual Camera Offsetting")]
        [Tooltip("Manually modify the camera height if needed")]
        public float CameraYOffset;

        [Tooltip("Height of the virtual player")]
        public float PlayerHeight = 1.66f;

        [Tooltip("Sitting or standing mode")]
        public HVRSitStand SitStanding = HVRSitStand.PlayerHeight;

        [Header("Debugging")]

        [Tooltip("If true, use up and down arrow to change YOffset to help with testing.")]
        public bool DebugKeyboardOffset;

        [Header("For Debugging Display only")]
        public float PlayerControllerYOffset = 0f;
        public float AdjustedCameraHeight;
        public float SittingOffset;
        public bool CalibrateHeightOnStart;

        public bool IsMine { get; set; } = true;

        private Vector3 _cameraStartingPosition;

        void Start()
        {
            Setup();

            if (CalibrateHeightOnStart)
            {
                StartCoroutine(CalibrateHeightOnStartRoutine());
            }
        }

        private IEnumerator CalibrateHeightOnStartRoutine()
        {
            yield return null;
            _cameraStartingPosition = Camera.localPosition;

            while (Vector3.Distance(_cameraStartingPosition, Camera.transform.localPosition) < .05f)
            {
                yield return null;
            }

            Debug.Log($"Camera movement detected, calibrating height.");
            CalibrateHeight(Camera.localPosition.y);
        }

        void Update()
        {
            if (FloorOffset)
            {
                var pos = FloorOffset.transform.localPosition;
                var intendedOffset = SittingOffset + CameraYOffset + PlayerControllerYOffset;
                FloorOffset.transform.localPosition = new Vector3(pos.x, intendedOffset, pos.z);
            }

            AdjustedCameraHeight = FloorOffset.transform.localPosition.y + Camera.localPosition.y * _scale;

            if (IsMine)
            {
#if ENABLE_LEGACY_INPUT_MANAGER
                if (DebugKeyboardOffset && UnityEngine.Input.GetKeyDown(KeyCode.DownArrow))
                {
                    CameraYOffset += -.25f;
                }
                else if (DebugKeyboardOffset && UnityEngine.Input.GetKeyDown(KeyCode.UpArrow))
                {
                    CameraYOffset += .25f;
                }
#endif
            }
        }

        private void Setup()
        {
            var offset = CameraYOffset;

            if (FloorOffset)
            {
                var pos = FloorOffset.transform.localPosition;
                FloorOffset.transform.localPosition = new Vector3(pos.x, offset, pos.z);
            }
        }

        private float _scale = 1f;
        public void CalibrateHeight(float height)
        {
            //CalibratedHeight = height;

            if (SitStanding == HVRSitStand.Standing)
            {
                SittingOffset = 0f;
                _scale = PlayerHeight / height;
            }
            else if (SitStanding == HVRSitStand.Sitting)
            {
                SittingOffset = PlayerHeight - height;
                _scale = 1f;
            }
            else if (SitStanding == HVRSitStand.PlayerHeight)
            {
                SittingOffset = 0f;
                _scale = 1f;
            }

            if (CameraScale)
            {
                CameraScale.localScale = new Vector3(_scale, _scale, _scale);
            }
        }

        public void Calibrate()
        {
            CalibrateHeight(Camera.localPosition.y);
        }

        public void SetSitStandMode(HVRSitStand sitStand)
        {
            if (sitStand == HVRSitStand.Standing && !CameraScale)
            {
                Debug.LogWarning($"Standing mode cannot be set without the CameraScale transform assigned and setup properly.");
                sitStand = HVRSitStand.PlayerHeight;
            }

            SitStanding = sitStand;
            Calibrate();
        }
    }

    public enum HVRSitStand
    {
        Sitting,
        Standing,
        PlayerHeight
    }
}
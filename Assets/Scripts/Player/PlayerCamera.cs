using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Player
{
    [RequireComponent(typeof(PlayerComponent))]
    public class PlayerCamera : MonoBehaviour, IPlayerCamera
    {
        [SerializeField] private PlayerCameraConfiguration configuration;
        private PlayerComponent playerComponent;

        private float yaw = 0.0f;
        private float pitch = 0.0f;

        public PlayerCameraConfiguration Configuration { get => configuration; set => configuration = value; }

        private void Awake()
        {
            playerComponent = PlayerComponent.sharedInstance;
        }

        private void Start()
        {
            playerComponent.Camera.fieldOfView = Configuration.Fov;
        }

        private void Update()
        {
            Move();   
        }

        public void Move()
        {
            if(Configuration.CameraCanMove)
            {
                yaw = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * Configuration.MouseSensitivity;
        
                if (!Configuration.InvertCamera)
                {
                    pitch -= Configuration.MouseSensitivity * Input.GetAxis("Mouse Y");
                }
                else
                {
                    pitch += Configuration.MouseSensitivity * Input.GetAxis("Mouse Y");
                }
        
                pitch = Mathf.Clamp(pitch, -Configuration.MaxLookAngle, Configuration.MaxLookAngle);
        
                transform.localEulerAngles = new Vector3(0, yaw, 0);
                playerComponent.Camera.transform.localEulerAngles = new Vector3(pitch, 0, 0); 
            }
        }
    }
}
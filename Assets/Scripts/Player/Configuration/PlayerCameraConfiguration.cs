using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(menuName = "Player Custom/Camera Configuration")]
    public class PlayerCameraConfiguration: ScriptableObject
    {

        [SerializeField] private float _fov = 60f;
        [SerializeField] private bool _invertCamera = false;
        [SerializeField] private bool _cameraCanMove = true;
        [SerializeField] private float _mouseSensitivity = 2f;
        [SerializeField] private float _maxLookAngle = 50f;
        
        public float Fov { get => _fov; set => _fov = value; }
        public bool InvertCamera { get => _invertCamera; set => _invertCamera = value; }
        public bool CameraCanMove { get => _cameraCanMove; set => _cameraCanMove = value; }
        public float MouseSensitivity { get => _mouseSensitivity; set => _mouseSensitivity = value; }
        public float MaxLookAngle { get => _maxLookAngle; set => _maxLookAngle = value; }
        
    }
}
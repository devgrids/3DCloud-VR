using UnityEngine;

namespace Player
{
    public class PlayerComponent : MonoBehaviour
    {
        public static PlayerComponent sharedInstance;

        [SerializeField] private Camera _camera;
        [SerializeField] private Rigidbody _rigidbody;

        public Camera Camera { get => _camera; set => _camera = value; }
        public Rigidbody Rigidbody { get => _rigidbody; set => _rigidbody = value; }

        private void Awake()
        {
            sharedInstance = this;
        }

    }
}
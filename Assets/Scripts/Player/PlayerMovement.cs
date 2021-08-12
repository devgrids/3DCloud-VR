using System;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    [RequireComponent(typeof(PlayerComponent))]
    public class PlayerMovement : MonoBehaviour, IPlayerMovement
    {
        [SerializeField] private PlayerMovementConfiguration configuration;
        private PlayerComponent playerComponent;

        public PlayerMovementConfiguration Configuration { get => configuration; set => configuration = value; }

        private void Awake()
        {
            playerComponent = PlayerComponent.sharedInstance;
        }

        private void FixedUpdate()
        {
            Move();
        }

        public void Move()
        {
            if (configuration.PlayerCanMove)
            {
                Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                targetVelocity = transform.TransformDirection(targetVelocity) * configuration.WalkSpeed;
                
                Vector3 velocity = playerComponent.Rigidbody.velocity;
                Vector3 velocityChange = (targetVelocity - velocity);
                velocityChange.x = Mathf.Clamp(velocityChange.x, -configuration.MaxVelocityChange, configuration.MaxVelocityChange);
                velocityChange.z = Mathf.Clamp(velocityChange.z, -configuration.MaxVelocityChange, configuration.MaxVelocityChange);
                velocityChange.y = 0;

                playerComponent.Rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
            }
        }
    }
}
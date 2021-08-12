using System;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerComponent))]
    public class PlayerJump : MonoBehaviour, IPlayerJump
    {
        [SerializeField] private PlayerJumpConfiguration configuration;
        private PlayerComponent playerComponent;

        public PlayerJumpConfiguration Configuration { get => configuration; set => configuration = value; }

        private void Awake()
        {
            playerComponent = PlayerComponent.sharedInstance;
        }

        private void Update()
        {
            if( Input.GetKeyDown(Configuration.JumpKey))
            {
                Jump();
            }
        }

        public void Jump()
        {
            if (Util.CheckGround(transform, 0.75f)) 
            {
                playerComponent.Rigidbody.AddForce(0f, Configuration.JumpPower, 0f, ForceMode.Impulse);
            }
        }
    }
}
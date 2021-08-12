using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    [CreateAssetMenu(menuName = "Player Custom/Movement Configuration")]
    public class PlayerMovementConfiguration : ScriptableObject
    {
        [SerializeField] private bool playerCanMove = true;
        [SerializeField] private float walkSpeed = 5f;
        [SerializeField] private float maxVelocityChange = 10f;
        

        public bool PlayerCanMove { get => playerCanMove; set => playerCanMove = value; }
        public float WalkSpeed { get => walkSpeed; set => walkSpeed = value; }
        public float MaxVelocityChange { get => maxVelocityChange; set => maxVelocityChange = value; }

    }
}
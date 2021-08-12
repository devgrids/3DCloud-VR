using UnityEngine;

namespace Player
{
    [CreateAssetMenu(menuName = "Player Custom/Jump Configuration")]
    public class PlayerJumpConfiguration : ScriptableObject
    {
        [SerializeField] KeyCode _jumpKey = KeyCode.Space;
        [SerializeField] float _jumpPower = 5f;

        public KeyCode JumpKey { get => _jumpKey; set => _jumpKey = value; }
        public float JumpPower { get => _jumpPower; set => _jumpPower = value; }
    }
}
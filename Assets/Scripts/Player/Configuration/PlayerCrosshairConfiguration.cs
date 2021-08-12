using UnityEngine;

namespace Player
{
    [CreateAssetMenu(menuName = "Player Custom/Crosshair Configuration")]
    public class PlayerCrosshairConfiguration: ScriptableObject
    {
        [SerializeField] private bool _lockCursor = true;
        [SerializeField] private Sprite _crosshairImage;
        [SerializeField] private Color _crosshairColor = Color.white;
        
        public bool LockCursor { get => _lockCursor; set => _lockCursor = value; }
        public Sprite CrosshairImage { get => _crosshairImage; set => _crosshairImage = value; }
        public Color CrosshairColor { get => _crosshairColor; set => _crosshairColor = value; }

    }
}
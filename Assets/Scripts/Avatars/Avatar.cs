using UnityEngine;

namespace Avatars
{
    public abstract class Avatar : MonoBehaviour
    {
        [SerializeField] protected string id;
        public string Id => id;

        // public abstract  void inter();
    }
}
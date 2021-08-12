using System;
using System.Collections.Generic;
using UnityEngine;

namespace Avatars
{
    [CreateAssetMenu(menuName = "Custom/Avatars configuration")]
    public class AvatarsConfiguration: ScriptableObject{
        
        [SerializeField] private Avatar[] avatars;
        private Dictionary<string, Avatar> idToAvatar;

        private void Awake()
        {
            idToAvatar = new Dictionary<string, Avatar>();
            foreach (var avatar in avatars)
            {
                idToAvatar.Add(avatar.Id, avatar);
            }
        }

        public Avatar GetAvatarPrefabById(string id)
        {
            if (!idToAvatar.TryGetValue(id, out var avatar))
            {
                throw new Exception($"Avatar with id {id} does not exit");
            }
            return avatar;
        }
    }
}
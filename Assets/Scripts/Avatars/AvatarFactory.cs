using Object = UnityEngine.Object;

namespace Avatars
{
    public class AvatarFactory
    {
        private readonly AvatarsConfiguration avatarsConfiguration;

        public AvatarFactory(AvatarsConfiguration avatarsConfiguration)
        {
            this.avatarsConfiguration = avatarsConfiguration;
        }

        public Avatar Create(string id)
        {
            var avatar = avatarsConfiguration.GetAvatarPrefabById(id);
            return Object.Instantiate(avatar);
        }
    }
}
using UnityEngine;

internal interface IPlayerJump
{
    void Jump();
}
internal interface IPlayerMovement
{
    void Move();
}
internal interface IPlayerCrosshair
{
    void ExecuteEventsFromGameObject(ref GameObject gameObject);
    bool GetGameObjectFromRaycast(int layerMask, float maxDistance, out GameObject gameObject);
}
internal interface IPlayerCamera
{
    void Move();
}
internal interface IPlayerAnimation
{
    void Move(float x, float y);
}

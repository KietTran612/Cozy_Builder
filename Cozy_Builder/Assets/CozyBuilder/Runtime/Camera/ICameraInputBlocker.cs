using UnityEngine;

namespace CozyBuilder.Camera
{
    public interface ICameraInputBlocker
    {
        bool IsPointerOverUI(Vector2 screenPosition);
    }
}

using UnityEngine;

namespace CozyBuilder.Town.Rendering
{
    [DisallowMultipleComponent]
    public sealed class BlockColorAdapter : MonoBehaviour
    {
        [SerializeField] private Renderer[] renderers;

        public Renderer[] Renderers
        {
            get
            {
                // Fallback nếu chưa kéo thả sẵn trong Inspector
                if (renderers == null || renderers.Length == 0)
                {
                    renderers = GetComponentsInChildren<Renderer>(true);
                }
                return renderers;
            }
        }

        private void Reset()
        {
            // Tự động tìm kiếm khi gắn component trong Editor
            renderers = GetComponentsInChildren<Renderer>(true);
        }
    }
}

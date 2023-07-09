using UnityEngine;

namespace Assets.Platforming_script_one
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Controller2D : MonoBehaviour
    {
        public LayerMask collisionMask;

        public const float SkinWidth = .015f;
        public int horizontalRayCount = 4;
        public int verticalRayCount = 4;

        [HideInInspector]
        public float horizontalRaySpacing;
        [HideInInspector]
        public float verticalRaySpacing;

        [HideInInspector]
        public BoxCollider2D bCollider;
        public RaycastOrigins RaycastOrigin;

        public virtual void Start()
        {
            bCollider = GetComponent<BoxCollider2D>();
            CalculateRaySpacing();
        }

        public void UpdateRaycastOrigins()
        {
            Bounds bounds = bCollider.bounds;
            bounds.Expand(SkinWidth * -2);

            RaycastOrigin.BottomLeft = new Vector2(bounds.min.x, bounds.min.y);
            RaycastOrigin.BottomRight = new Vector2(bounds.max.x, bounds.min.y);
            RaycastOrigin.TopLeft = new Vector2(bounds.min.x, bounds.max.y);
            RaycastOrigin.TopRight = new Vector2(bounds.max.x, bounds.max.y);
        }

        public void CalculateRaySpacing()
        {
            Bounds bounds = bCollider.bounds;
            bounds.Expand(SkinWidth * -2);

            horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
            verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

            horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
            verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
        }

        public struct RaycastOrigins
        {
            public Vector2 TopLeft, TopRight;
            public Vector2 BottomLeft, BottomRight;
        }
    }
}


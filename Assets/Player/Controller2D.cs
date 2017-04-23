using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Controller2D : MonoBehaviour {

    new BoxCollider2D collider;

    public LayerMask layerMask;
    public int rayCastCount = 4;
    public float buffer = 0.015f;

    private RaySpacing spacing;
    private Corners corners;

    struct RaySpacing
    {
        public float horizontal, vertical;
    }

    struct Corners
    {
        public Vector2 topLeft, topRight,
                       bottomLeft, bottomRight;
    }

    void Start () {
        collider = GetComponent<BoxCollider2D>();
        spacing = new RaySpacing();
        corners = new Corners();
        UpdateSpacing();
	}

    internal void Move(Vector3 velocity)
    {
        UpdateColliderCorners();
        if ( velocity.x != 0 )
        {
            CheckHorizontalCollisions(ref velocity);
        }


        if (velocity.y != 0)
        {
            CheckVerticalCollisions(ref velocity);
        }

        transform.Translate(velocity);
    }

    private void CheckHorizontalCollisions(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + buffer;
        Vector2 rayCastOrigin;
        
        for(int i = 0; i < rayCastCount; i++ )
        {
            rayCastOrigin = GoingLeft(directionX) ? corners.bottomLeft : corners.bottomRight;
            rayCastOrigin += Vector2.up * (spacing.horizontal * i);

            RaycastHit2D hit = Physics2D.Raycast(rayCastOrigin, Vector2.right * directionX, rayLength, layerMask);

            Debug.DrawRay(rayCastOrigin, Vector2.right * directionX * rayLength, Color.red);

            
            if ( hit )
            {
                velocity.x = (hit.distance - buffer) * directionX;
                rayLength = hit.distance;
            }
        }
    }

    private bool GoingLeft(float directionX)
    {
        return directionX < 0;
    }

    private void CheckVerticalCollisions(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + buffer;
        Vector2 rayCastOrigin;

        for (int i = 0; i < rayCastCount; i++)
        {
            rayCastOrigin = GoingUp(directionY) ? corners.bottomLeft : corners.topLeft;
            rayCastOrigin += Vector2.right * (spacing.vertical * i);

            RaycastHit2D hit = Physics2D.Raycast(rayCastOrigin, Vector2.up * directionY, rayLength, layerMask);

            Debug.DrawRay(rayCastOrigin, Vector2.up * directionY * rayLength, Color.red);

            
            if (hit)
            {
                velocity.y = (hit.distance - buffer) * directionY;
                rayLength = hit.distance;
            }
        }
    }

    private bool GoingUp(float directionY)
    {
        return directionY < 0;
    }

    void UpdateSpacing()
    {
        Bounds bounds = GetBoundsWithSkin();

        rayCastCount = Mathf.Clamp(rayCastCount, 2, 10);

        spacing.horizontal = bounds.size.y / (rayCastCount - 1);
        spacing.vertical = bounds.size.x / (rayCastCount - 1);
    }

    private Bounds GetBoundsWithSkin()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(buffer);
        return bounds;
    }

    private void UpdateColliderCorners()
    {
        Bounds bounds = GetBoundsWithSkin();

        corners.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        corners.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        corners.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        corners.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }
}

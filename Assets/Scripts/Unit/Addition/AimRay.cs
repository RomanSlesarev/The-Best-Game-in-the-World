using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Unit.Addition
{
    public class AimRay
    {
        public static bool Shot(Vector2 origin, Vector2 direction, float distance, int layerMask, string targetTag, out AimRayHit aimRayHit)
        {
            aimRayHit = new AimRayHit
            {
                LinePath = new List<Vector2> { origin },
                Direction = direction,
                Distance = distance
            };

            var hit = false;
            while (aimRayHit.Distance > 0 && !hit)
            {
                var raycastHit = Physics2D.Raycast(origin + direction, direction, aimRayHit.Distance, layerMask);
                if (raycastHit.collider != null)
                {
                    hit = raycastHit.collider.tag == targetTag;
                    aimRayHit.Distance -= raycastHit.distance;

                    origin = raycastHit.point;
                    direction = Vector2.Reflect(direction, raycastHit.normal);

                    aimRayHit.LinePath.Add(raycastHit.point);
                }
                else
                {
                    aimRayHit.LinePath.Add(origin + direction.normalized * distance);
                    aimRayHit.Distance = distance;
                    break;
                }
            }

            return hit;
        }
    }

    public struct AimRayHit
    {
        public float Distance;
        public List<Vector2> LinePath;
        public Vector2 Direction;
    }
}

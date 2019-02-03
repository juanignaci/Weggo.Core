using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weggo.Core2D
{
    public class Bullet2D : Core3D.Bullet
    {
        private void Update()
        {
            if (spd < 0)
                return;

            ProcessCollisions(transform.position, transform.position + transform.forward * spd * localDeltaTime);

            if (tShot + dist / spd <= Time.timeSinceLevelLoad)
                gameObject.SetActive(false);
        }

        protected override void ProcessCollisions(Vector3 from, Vector3 to)
        {
            RaycastHit2D col = radius >= 0 ?
                Physics2D.CircleCast(from, radius, (to - from).normalized, (to - from).magnitude, layerMask) :
                Physics2D.Raycast(from, (to - from).normalized, (to - from).magnitude, layerMask);

            if (col)
            {
                var c = col.transform.GetComponent<Character2D>();

                if (c != null)
                    c.OnBulletHit(this);

                OnHit(col);
            }
        }

        protected override void ProcessCollisions(Ray ray)
        {
            RaycastHit2D col = radius >= 0 ?
                Physics2D.CircleCast(ray.origin, radius, ray.direction, layerMask) :
                Physics2D.Raycast(ray.origin, ray.direction, layerMask);

            if (col)
            {
                var c = col.transform.GetComponent<Character2D>();

                if (c != null)
                    c.OnBulletHit(this);

                OnHit(col);
            }
        }

        protected void OnHit(RaycastHit2D hit) { gameObject.SetActive(false); }
        private void OnDrawGizmos() { Gizmos.DrawWireSphere(transform.position, radius); }
    }
}
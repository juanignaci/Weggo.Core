using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weggo.Core;

namespace Weggo.Core3D
{
    public class Bullet : PoolObject
    {
        public int dmg;
        public float radius;
        public LayerMask layerMask;

        protected float spd, dist, tShot;

        public override void Reset() { base.Reset(); spd = -1; dist = 0; }

        public void Shot(float direction, float distance, float speed, int damage = 0)
        {
            dmg = damage;
            spd = speed;
            dist = distance;
            transform.eulerAngles = Vector3.up * direction;
            tShot = Time.timeSinceLevelLoad;
        }

        public void Shot(Vector3 direction, float distance, float speed, int damage = 0)
        {
            dmg = damage;
            spd = speed;
            dist = distance;
            transform.LookAt(transform.position + direction);
            tShot = Time.timeSinceLevelLoad;
        }

        public void Shot(Vector3 direction, Vector3 worldUp, float distance, float speed, int damage = 0)
        {
            dmg = damage;
            spd = speed;
            dist = distance;
            transform.LookAt(transform.position + direction, worldUp);
            tShot = Time.timeSinceLevelLoad;
        }

        private void Update()
        {
            if (spd < 0)
                return;

            ProcessCollisions(transform.position, transform.position + transform.forward * spd * localDeltaTime);

            if (tShot + dist / spd <= Time.timeSinceLevelLoad)
                gameObject.SetActive(false);
        }

        protected virtual void ProcessCollisions(Vector3 from, Vector3 to)
        {
            RaycastHit col;

            if(radius >= 0 ?
                Physics.SphereCast(from, radius, (to - from).normalized, out col, (to - from).magnitude, layerMask) :
                Physics.Raycast(from, (to - from).normalized, out col, (to - from).magnitude, layerMask)
            ) {
                var c = col.transform.GetComponent<Character>();

                if (c != null)
                    c.OnBulletHit(this);

                OnHit(col);
            }
        }

        protected virtual void ProcessCollisions(Ray ray)
        {
            RaycastHit col;

            if(radius >= 0 ?
                Physics.SphereCast(ray, radius, out col, layerMask) :
                Physics.Raycast(ray, out col, layerMask)
            ) {
                var c = col.transform.GetComponent<Character>();

                if (c != null)
                    c.OnBulletHit(this);

                OnHit(col);
            }
        }

        protected void OnHit(RaycastHit hit) { gameObject.SetActive(false); }
        private void OnDrawGizmos() { Gizmos.DrawWireSphere(transform.position, radius); }
        public float GetSpeed() { return spd; }
    }
}
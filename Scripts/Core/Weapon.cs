using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weggo.Core3D;

namespace Weggo.Core
{
    public class Weapon : MonoBehaviour
    {
        public WeaponBehaviour behaviour;
        public Transform weaponTip;
        public bool isFiring = false;
        public float lShot = -1;
        public ObjectPool<Bullet> pool;
        public Transform aimPoint;

        public Entity chr;

        // Use this for initialization
        public virtual void Init(Entity c)
        {
            chr = c;
            behaviour.Init(this);
            if (aimPoint == default(Transform)) {
                aimPoint = new GameObject("AimPoint").transform;
                aimPoint.SetParent(transform);
            }
        }

        protected virtual void Update()
        {
            behaviour.Update();
        }

        public virtual void OnFire() { lShot = Time.timeSinceLevelLoad; }

        public Vector3 GetAimPoint() { return aimPoint.position; }
        public Vector3 GetAimDirection() { return (aimPoint.position - weaponTip.position).normalized; }
    } 
}



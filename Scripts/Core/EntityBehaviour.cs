using UnityEngine;
using Weggo.Core2D;
using Weggo.Core3D;

namespace Weggo.Core
{
    public abstract class CharacterBehaviour : MonoBehaviour {
        protected Character c;
        protected Animator anim;
        public void Init(Character c) { this.c = c; anim = c.mesh.GetComponent<Animator>(); OnInit(); }
        public abstract void OnInit();
        public abstract void UpdateBehaviour();
        public virtual void ResetBehaviour()
        {
            c.movement.velocity.Set(0, 0, 0);
            c.movement.force.Set(0, 0, 0);
        }
    }

    public abstract class Character2DBehaviour : MonoBehaviour
    {
        protected Character2D c;
        protected Animator anim;
        public void Init(Character2D c) { this.c = c; anim = c.sprite.GetComponent<Animator>(); OnInit(); }
        public abstract void OnInit();
        public abstract void Update();
        public virtual void Reset()
        {
            c.movement.velocity.Set(0, 0);
            c.movement.force.Set(0, 0);
        }
    }

    [System.Serializable]
    public class WeaponBehaviour {
        protected Weapon weapon;
        protected Animator weaponAnim;

        public bool isAuto = true;
        public float delay;
        public Bullet bulletPrefab;
        public float maxDistance = 10, bulletSpeed = 50;

        public void Init(Weapon w) { weapon = w; weaponAnim = weapon.GetComponent<Animator>(); weapon.pool = new ObjectPool<Bullet>(bulletPrefab); OnInit(); }
        public virtual void OnInit() {  }
        public virtual void Reset() { weapon.lShot = -1; }
        public virtual bool CanFire() {
            return weapon.lShot + delay <= Time.timeSinceLevelLoad;
        }
        public virtual void OnFireDown()
        {
            weapon.isFiring = true;

            if (!isAuto)
                Fire();
        }

        public virtual void OnFireUp()
        {
            weapon.isFiring = false;
        }

        public virtual void Update()
        {
            if (isAuto && weapon.isFiring)
                Fire();
        }

        public virtual void Fire()
        {
            if (!CanFire())
                return;

            weapon.OnFire();

            if (bulletPrefab == default(Bullet))
                return;

            Bullet bb = weapon.pool.GetNext(weapon.weaponTip.position);
            bb.Shot((weapon.aimPoint.position - weapon.weaponTip.position).normalized, maxDistance, bulletSpeed);
        }
    }
}
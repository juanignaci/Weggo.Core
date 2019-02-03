using UnityEngine;
using Weggo.Core;

namespace Weggo.Core2D
{
    public class Character2D : Entity
    {
        public int health, maxHealth = 1;

        public EntitySprite sprite;

        public Rigidbody2D rBody { get; private set; }
        public Movement movement;
        public Weapon weapon;
        public Character2DBehaviour behaviour;

        private void OnEnable()
        {
            Reset();

            if (GetComponent<Rigidbody2D>() == null)
                rBody = gameObject.AddComponent<Rigidbody2D>();
            else rBody = GetComponent<Rigidbody2D>();

            sprite.sRen.sortingLayerName = "Characters";

            if (weapon)
                weapon.Init(this);

            movement.Init(this);
            behaviour.Init(this);

            behaviour.Reset();
        }

        private void Reset()
        {
            behaviour = GetComponent<Character2DBehaviour>();

            health = maxHealth;
            movement.velocity.Set(0, 0);
            movement.force.Set(0, 0);
        }

        // Update is called once per frame
        private void Update()
        {
            sprite.UpdateSprite(transform);

            behaviour.Update();
            movement.Update();

            rBody.velocity = movement.GetVelocity();
        }

        public void OnBulletHit(Bullet2D bullet)
        {
            movement.AddForce(bullet.transform.forward * bullet.GetSpeed());
            TakeDamage(bullet.dmg);
        }

        protected virtual void TakeDamage(int amount)
        {
            health -= amount;
            if (health <= 0)
                Die();
        }

        protected virtual void Die()
        {
            gameObject.SetActive(false);
        }
    }

    [System.Serializable]
    public class Movement
    {
        Character2D chr;
        public float acceleration = 42, maxSpeed = 10;
        [Range(0, 1)]
        public float friction = 0.8f;
        public bool lockVelocity = false, lockForce = false;
        public Vector2 velocity = new Vector2(), force = new Vector2();

        public void Init(Character2D c) { chr = c; }

        public void Update()
        {
            if (!lockVelocity)
                velocity *= 1 - friction;
            if (!lockForce)
                force *= 1 - friction;
        }

        public Vector3 GetVelocity() { return velocity + force; }

        public void Accelerate(Vector2 direction, float multiiplier = 1)
        {
            velocity = Vector2.ClampMagnitude(direction.normalized * acceleration
                            * chr.localDeltaTime * multiiplier + velocity, maxSpeed);
        }

        public void AddForce(Vector2 force, bool overrideForces = false)
        {
            if (overrideForces)
                this.force = force;
            else
                this.force += force;
        }
    }

}
using UnityEngine;
using Weggo.Core;

namespace Weggo.Core3D
{
    public class Character : Entity
    {
        public int health, maxHealth = 1;
        public Transform mesh;
        public Rigidbody rBody { get; private set; }
        public Movement movement;
        public Weapon weapon;
        public CharacterBehaviour behaviour;

        private void OnEnable()
        {
            Reset();

            if (GetComponent<Rigidbody>() == null) {
                Debug.LogWarning("No RigidBody found! adding default component, this could lead to undesired behaviour!");
                rBody = gameObject.AddComponent<Rigidbody>();
            }
            else rBody = GetComponent<Rigidbody>();

            movement.Init(this);

            weapon?.Init(this);
            behaviour?.Init(this);
            behaviour?.ResetBehaviour();
        }

        protected virtual void Reset()
        {
            behaviour = GetComponent<CharacterBehaviour>();

            health = maxHealth;
            movement.velocity.Set(0,0,0);
            movement.force.Set(0,0,0);
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            behaviour.UpdateBehaviour();
            movement.Update();

            rBody.velocity = movement.GetVelocity();
        }

        public virtual void OnBulletHit(Bullet bullet)
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
        Character chr;
        public float acceleration = 42, maxSpeed = 10;
        [Range(0, 1)]
        public float friction = 0.8f;
        public bool lockVelocity = false, lockForce = false;
        public Vector3 velocity = new Vector3(), force = new Vector3();

        public void Init(Character c) { chr = c; }

        public void Update()
        {
            if (!lockVelocity)
                velocity *= 1 - friction;
            if (!lockForce)
                force *= 1 - friction;
        }

        public Vector3 GetVelocity() { return velocity + force; }

        public void Accelerate(Vector3 direction, float multiiplier = 1)
        {
            velocity = Vector3.ClampMagnitude(direction.normalized * acceleration
                            * chr.localDeltaTime * multiiplier + velocity, maxSpeed);
        }

        public void AddForce(Vector3 force, bool overrideForces = false)
        {
            if (overrideForces)
                this.force = force;
            else
                this.force += force;
        }
    }
}
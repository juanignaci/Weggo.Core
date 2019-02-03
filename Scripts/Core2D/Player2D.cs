using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weggo.Core;

namespace Weggo.Core2D
{
    public class Player2D : Character2DBehaviour
    {

        public string xAxis = "Horizontal", yAxis = "Vertical", shoot = "mouse 0";

        public override void OnInit() { }

        public override void Update()
        {
            Movement();
            WeaponInput();
        }

        private void WeaponInput()
        {
            if (Input.GetButtonDown(shoot))
                c.weapon.behaviour.OnFireDown();
            else if (Input.GetButtonUp(shoot) || (!Input.GetButton(shoot) && c.weapon.isFiring))
                c.weapon.behaviour.OnFireUp();
        }

        private void Movement()
        {
            var inputVector = new Vector2(Input.GetAxisRaw(xAxis), Input.GetAxisRaw(yAxis));

            if (inputVector.magnitude == 0)
            {
                c.movement.lockVelocity = false;
                return;
            }

            c.movement.lockVelocity = true;

            if (Vector2.Angle(inputVector, c.movement.velocity) > 130)
                c.movement.velocity *= 0.7f;

            c.movement.Accelerate(inputVector);
        }
    } 
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weggo.Core3D
{
    public class MousePoint : Core2D.MousePoint2D
    {
        Plane plane = new Plane();
        public float planeHeigth = 0;

        protected override void Update()
        {
            ray = (default(Camera) ? Camera.main : cam).ScreenPointToRay(Input.mousePosition);
            plane.SetNormalAndPosition(Vector3.up, Vector3.up * planeHeigth);
            plane.Raycast(ray, out output);

            transform.position = ray.GetPoint(output);
        }
    } 
}

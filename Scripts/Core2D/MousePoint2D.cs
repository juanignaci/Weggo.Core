using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weggo.Core2D
{
    public class MousePoint2D : MonoBehaviour
    {
        public Camera cam;
        readonly Plane plane = new Plane(-Vector3.forward, Vector3.zero);
        protected Ray ray;
        protected float output;

        // Update is called once per frame
        protected virtual void Update()
        {
            ray = (default(Camera) ? Camera.main : cam).ScreenPointToRay(Input.mousePosition);
            plane.Raycast(ray, out output);
            transform.position = ray.GetPoint(output);
        }
    } 
}

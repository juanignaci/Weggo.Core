using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weggo.Core
{
    public class EntitySprite : MonoBehaviour
    {

        public SpriteRenderer sRen;
        public float height;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        public void UpdateSprite(Transform tt)
        {
            sRen.sortingOrder = Mathf.FloorToInt(tt.position.y * 1000);
            transform.position = tt.position + Vector3.up * height;
        }
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weggo.Core
{
    public class Entity : MonoBehaviour
    {
        public float timescale = 1;
        public float localTimescale { get { return timescale * Time.timeScale; } }
        public float localDeltaTime { get { return localTimescale * Time.deltaTime; } }
    } 
}

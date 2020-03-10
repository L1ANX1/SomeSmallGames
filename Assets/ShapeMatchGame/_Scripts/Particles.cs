using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShapeMatchGame
{

    public class Particles : MonoBehaviour
    {
        public float destoryDelay;
        // Use this for initialization
        void Start()
        {
            Destroy(this.gameObject, destoryDelay);
        }

    }
}
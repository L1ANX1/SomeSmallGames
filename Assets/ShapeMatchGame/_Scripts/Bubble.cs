using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShapeMatchGame
{
    public enum BubbleGenDir { up, right, left, bottom };
    public class Bubble : MonoBehaviour
    {
        Rigidbody2D rigidbody2D;
        Vector3 vecDir;
        public BubbleGenDir GenDir { get; set; }
        public float Speed { get; set; }
        public Vector3 VecDir
        {
            get
            {
                return vecDir;
            }
            set
            {
                vecDir = value;
                Move();
            }
        }

        public Bubble()
        {
            // Debug.Log("this is a Bubble" + " " + Speed + " " + VecDir);
        }

        void Awake()
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
        }

        public void Move()
        {
            // Debug.Log("Move" + " " + Speed + " " + VecDir);
            rigidbody2D.velocity = VecDir * Speed * (Random.Range(0.5f, 1.3f));
        }


    }
}
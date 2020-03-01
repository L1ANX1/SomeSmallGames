using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    public float destoryDelay;
    // Use this for initialization
    void Start()
    {
        Destroy(this.gameObject, destoryDelay);
    }

    // Update is called once per frame
    void Update()
    {

    }
}

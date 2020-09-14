using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleRemoval : MonoBehaviour
{
    private ParticleSystem ps;

    public void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    public void Update()
    {
        if (ps && !ps.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] int _damageAmount = 1;
    [SerializeField] ParticleSystem _impactParticles;
    [SerializeField] AudioClip _impactSound;

    Rigidbody _rb;

    private void Awake()
    {
        GetComponent<Health>();

        _rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Health player = other.gameObject.GetComponent<Health>();
        if (player != null)
        {
            PlayerImpact(player);
            ImpactFeedback();
        }
    }

    private void PlayerImpact(Health player)
    {
        player.DecreaseHealth(_damageAmount);
    }

    private void ImpactFeedback()
    {
        // particles
        if (_impactParticles != null)
        {
            _impactParticles = Instantiate(_impactParticles,
                transform.position, Quaternion.Euler(-90,0,0));
        }
        // audio. TODO - consider Object Pooling for performance
        if (_impactSound != null)
        {
            AudioHelper.PlayClip2D(_impactSound, 1f);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityChange : Ability
{
    [SerializeField] Material objectTelekinesis;
    [SerializeField] ParticleSystem telekinesisCastFeedback;
    [SerializeField] ParticleSystem telekinesisHitFeedback;
    [SerializeField] AudioClip telekinesisCastSound;
    [SerializeField] AudioClip telekinesisHitSound;

    int forceAmount = 100;

    public override void Use(Transform origin, Transform target)
    {
        int layerMask = LayerMask.GetMask("Gravity");
        RaycastHit hit;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            CastActivateFeedback();
        }

        if (Physics.Raycast(origin.position, origin.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            if (hit.rigidbody != null)
            {
                Debug.Log("Gravity detect working");
                hit.rigidbody.AddForce(Vector3.up * forceAmount);

                hit.collider.GetComponent<Renderer>().material = objectTelekinesis;

                CastHitFeedback();
            }
        }
    }

    private void CastActivateFeedback()
    {
        Instantiate(telekinesisCastFeedback, transform.position, Quaternion.identity);

        if (telekinesisCastSound != null)
        {
            AudioHelper.PlayClip2D(telekinesisCastSound, 1f);
        }
    }

    private void CastHitFeedback()
    {
        Instantiate(telekinesisHitFeedback, transform.position, Quaternion.identity);

        if (telekinesisHitSound != null)
        {
            AudioHelper.PlayClip2D(telekinesisHitSound, 1f);
        }
    }
}

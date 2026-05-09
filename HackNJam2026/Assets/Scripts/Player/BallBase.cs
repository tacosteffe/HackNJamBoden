using System;
using UnityEngine;

public class BallBase : MonoBehaviour
{
    [SerializeField]
    protected  Rigidbody RB;

    [SerializeField]
    protected  float LifeTime = 10f;

    
    public void Fire(Vector3 position, Vector3 direction, float force)
    {
        transform.position = position;
        RB.AddForce(direction * force, ForceMode.Impulse);
    }


    protected virtual void Update()
    {
        if ((LifeTime -= Time.deltaTime) < 0f)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        var p = collision.GetContact(0).point;
        EffectGenerator.Instance.SpawnDust(p, (p - transform.position).normalized);
        Destroy(gameObject);
    }
}

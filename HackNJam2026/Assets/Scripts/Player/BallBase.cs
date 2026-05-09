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


    private void Update()
    {
        if ((LifeTime -= Time.deltaTime) < 0f)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        
    }
}

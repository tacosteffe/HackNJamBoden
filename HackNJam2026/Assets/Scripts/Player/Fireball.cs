using UnityEngine;

public class Fireball : BallBase
{
    
    protected override void Update()
    {
        base.Update();
    }


    protected override void OnCollisionEnter(Collision collision)
    {
        var p = collision.GetContact(0).point;
        EffectGenerator.Instance.SpawnExplosion(p, (p - transform.position).normalized);
        Destroy(this.gameObject);
    }
}

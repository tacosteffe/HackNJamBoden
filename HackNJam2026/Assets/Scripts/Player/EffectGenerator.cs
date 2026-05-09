using System.Collections.Generic;
using UnityEngine;

public class EffectGenerator : Singleton<EffectGenerator>
{
    public GameObject DustEffect;
    public GameObject ExplosionEffect;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Implement(this, out var _);
    }

    public void SpawnDust(Vector3 location, Vector3 direction)
    {
        var go = Instantiate(DustEffect, location, Quaternion.LookRotation(direction));
        var eff = go.GetComponent<Effect>();
        eff.Spawn();
    }
    public void SpawnExplosion(Vector3 location, Vector3 direction)
    {
        var go = Instantiate(ExplosionEffect, location, Quaternion.LookRotation(direction));
        var eff = go.GetComponent<FireEffect>();
        eff.Spawn();
    }
}

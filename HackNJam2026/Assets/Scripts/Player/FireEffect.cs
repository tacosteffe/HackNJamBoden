using UnityEngine;

public class FireEffect : MonoBehaviour
{
    public float Lifetime = 2f;
    public ParticleSystem Particles;
    public ParticleSystem Particles2;
    
    public void Spawn()
    {
        Particles.Play();    
        Particles2.Play();    
    }

    // Update is called once per frame
    void Update()
    {
        if ((Lifetime - Time.deltaTime) <= 0f)
        {
            Destroy(this.gameObject);
        }
    }
}

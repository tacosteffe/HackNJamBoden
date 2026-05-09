using UnityEngine;

public class Effect : MonoBehaviour
{
    public float Lifetime = 2f;
    public ParticleSystem Particles;
    
    public void Spawn()
    {
        Particles.Play();    
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

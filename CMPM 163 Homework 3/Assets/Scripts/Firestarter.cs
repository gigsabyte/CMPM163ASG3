using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firestarter : MonoBehaviour
{
    [SerializeField]
    ParticleSystem fire;
    [SerializeField]
    ParticleSystem smoke;

    bool fireOn = false;

    // Start is called before the first frame update
    void Start()
    {
        ParticleSystem[] ps = transform.GetComponentsInChildren<ParticleSystem>();
        fire = ps[0];
        smoke = ps[1];

        if (fire.isPlaying) fire.Stop();
        if (smoke.isPlaying) smoke.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartFire()
    {
        if (!fireOn)
        {
            fire.Play();
            smoke.Play();
        }
        else
        {
            fire.Stop();
            fire.Clear();
            smoke.Stop();
            smoke.Clear();
        }
        fireOn = !fireOn;
    }
}

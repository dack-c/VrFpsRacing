using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSoundStarter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioSource explosionSound = GetComponent<AudioSource>();
        explosionSound.Play();
        Destroy(gameObject, 5.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

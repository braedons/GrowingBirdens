using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public PlayerController playerController;
    public AudioSource walking;
    public AudioSource flap;
    public AudioSource hurt;
    public AudioSource pickup;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.IsWalking() && !walking.isPlaying) {
            // walking.volume = Random.Range(0.8f, 1f);
            walking.pitch = Random.Range(0.8f, 1.1f);
            walking.Play();
        }
        else if (!playerController.IsWalking() && walking.isPlaying) {
            walking.Stop();
        }
    }

    public void Flap() {
        // flap.volume = Random.Range(0.8f, 1f);
        flap.pitch = Random.Range(0.8f, 1.1f);
        flap.Play();
    }

    public void Hurt() {
        hurt.Play();
    }

    public void Pickup() {
        pickup.Play();
    }
}

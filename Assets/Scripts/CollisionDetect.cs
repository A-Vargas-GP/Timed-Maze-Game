using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetect : MonoBehaviour
{
    //Script is read by the TileMap's BarriersMaze

    //Audio variables
    private AudioSource audio;
    public AudioClip hitPlay;
    
    // Start is called before the first frame update
    void Start()
    {
        //Initializes the variables
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //If collision is detected, set its boolean variable to detect collisions to true
    //Decrease the number of chances by 1 and play an audio clip
    private void OnCollisionEnter2D(Collision2D collision)
    {
        audio.PlayOneShot(hitPlay,0.5F);
        levelManager.chances--;
        levelManager.detectCollide = true;
    }
}

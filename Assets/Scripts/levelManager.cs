using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class levelManager : MonoBehaviour
{
    //Script is read by levelManager game object in the first scene
    
    //Variable to see if Player can move or not, is updated in the MovementPlayer script
    public static bool goMove = false;

    //Variable to represent number of chances a Player has, is updated in the MovementPlayer script
    public static int chances = 5;

    //Time variables
    //Used in the MovementPlayer script to print the time in 'minutes:seconds' and to detect if the Player has run out of time
    public static float timeRemaining = 500;
    public static bool timerHasRunOut = false;

    //Current level variable
    public static int currLevel = 1;

    //Variable to detect if collision occurs
    public static bool detectCollide = false;
    
    private void Awake()
    {
        //Allows levelManager to carry throughout scenes
        DontDestroyOnLoad(transform.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Decreases the time up to 0
        //Once 0 is achieved (meaning time has run out), set the boolean timer object to true (read in the MovementPlayer script)
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            timerHasRunOut = true;
            timeRemaining = 0;
        }
    }
}

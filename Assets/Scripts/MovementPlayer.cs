using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MovementPlayer : MonoBehaviour
{
    //Script is read by the Player object

    //Player variables
    private PlayerInput player;
    private Rigidbody2D rb;

    //Player movement variable
    private Vector2 movement;

    //Player's animator variable
    private Animator animatorPlayer;

    //Player speed variable
    private float speed = 3f;

    //Text variables
    public Text titleText;
    public Text descText;
    public Text chancesBeforeRestart;
    public Text timerText;
    public Text hintTitleText;
    public Text hintText;
    public Text currLevelText;

    //Button variable
    public Button startBtn;

    //Player's original position variable
    private Vector2 originalPosition;

    //Variables to represent minutes and seconds
    private float minutes;
    private float seconds;

    //Audio variables
    private AudioSource audio;
    public AudioClip walking;

    //Variable to represent game ending
    private bool gameEnd = false;

    //Variable that will be used to change the player color
    SpriteRenderer sprite;

    //Variable to see how much time has passed since collision occurs
    private float timer = 0.0f;

    //Initializes the variables
    private void Awake()
    {
        player = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
        animatorPlayer = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Saves the starting position once the game is started
        originalPosition = transform.position;
    }

    void OnMove(InputValue inputVal)
    {
        //Captures the Player's Vector2D movement once the button disappears
        if (levelManager.goMove == true)
        {
            movement = inputVal.Get<Vector2>();
        }
    }

    void Update()
    {
        //Uses the Player's movement values to trigger the walkUp, walkDown, walkLeft, and walkRight animations
        animatorPlayer.SetFloat("Horizontal", movement.x);
        animatorPlayer.SetFloat("Vertical", movement.y);
        animatorPlayer.SetFloat("Speed", movement.sqrMagnitude);

        //Uses the levelManager's time and convert into minutes and seconds
        minutes = Mathf.FloorToInt(levelManager.timeRemaining / 60);
        seconds = Mathf.FloorToInt(levelManager.timeRemaining % 60);

        //If the button disappears, update the UI (tells the hints, remaining lives, current level, and time remaining) and remove
        //the unnecessary texts and button
        if (levelManager.goMove == true)
        {
            startBtn.gameObject.SetActive(false);
            titleText.text = "";
            descText.text = "";
            hintTitleText.text = "Hints:";
            hintText.text = "Find the dark grass patches to escape!";
            chancesBeforeRestart.text = "Lives: " + levelManager.chances;
            timerText.text = "Time remaining: " + string.Format("{0:00}:{1:00}", minutes, seconds);
            currLevelText.text = "Level: " + levelManager.currLevel;
        }

        //If the timer has run out, update the UI with a losing screen and freeze the character so it does not move
        if (levelManager.timerHasRunOut == true)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
            gameObject.GetComponent<Animator>().enabled = false;
            titleText.text = "You have run out of time!";
            descText.text = "Try again next time!";
            hintTitleText.text = "";
            hintText.text = "";
            timerText.text = "";
            chancesBeforeRestart.text = "";
            currLevelText.text = "";
        }

        //If the number of chances has reached 0, move the character back to the starting position and give the player 5 chances again
        if (levelManager.chances <= 0)
        {
            levelManager.chances = 5;
            transform.position = originalPosition;
        }

        //If the Player reaches the end successfully, update the UI with a "Congrats" screen
        if (gameEnd == true)
        {
            titleText.color = Color.red;
            titleText.text = "Congrats!";
            descText.text = "You made it to the end of the maze!";
            timerText.text = "";
            chancesBeforeRestart.text = "";
            currLevelText.text = "";
            hintTitleText.text = "";
            hintText.text = "";
        }

        //Detect if collision occurs. If so, increment the time up to 0.3 seconds while changing the Player's sprite color to red
        //Once 0.3 seconds has passed, set the boolean variable to false
        if (levelManager.detectCollide == true)
        {
            sprite.color = new Color (1, 0, 0, 1);
            timer += Time.deltaTime;
            if (timer > 0.3f)
            {
                levelManager.detectCollide = false;
            }
        }

        //If a collision does not occur, change the Player's color to white and reset the timer to 0
        if (levelManager.detectCollide == false)
        {
            sprite.color = new Color (1, 1, 1, 1);
            timer = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Update the character's velocity
        rb.velocity = speed * movement;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If the Player reaches the first grass patch, send to the next Scene and increment the currentLevel by 1
        if (collision.tag == "Area1")
        {
            SceneManager.LoadScene("SecondScene");
            levelManager.currLevel++;
        }
        //If the Player reaches the second grass patch, send to the next Scene and increment the currentLevel by 1
        if (collision.tag == "Area2")
        {
            SceneManager.LoadScene("ThirdScene");
            levelManager.currLevel++;
        }
        //If the Player reaches the third grass patch, send to the next Scene and increment the currentLevel by 1
        if (collision.tag == "Area3")
        {
            SceneManager.LoadScene("FourthScene(WIN)");
            levelManager.currLevel++;
        }
        //If the Player reaches the end, freeze the character and its animations, and set the boolean variable 'gameEnd' to true
        if (collision.tag == "Area4")
        {
            gameEnd = true;
            gameObject.GetComponent<Animator>().enabled = false;
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        }

    }

    //Button event that when pressed, it allows the Player to move
    public void Pressed()
    {
        levelManager.goMove = true;
    }

    //Audio Sound for walking, used as an event for the Player's animated walking
    public void WalkSound()
    {
        audio.PlayOneShot(walking, 0.8F);
    }
}

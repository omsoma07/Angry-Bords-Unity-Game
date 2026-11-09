using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour{
    public float speed = 0; 
    public float jumpForce = 5;

    private static bool canJump = false;
    private static bool canDash = false;

    public GameObject boss;
    
    public TextMeshProUGUI countText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI dialogue;


    public TextMeshProUGUI door1Counter;
    public TextMeshProUGUI door2Counter;
    public TextMeshProUGUI door3Counter;
    public TextMeshProUGUI exitCounter;


    public GameObject door1;
    public GameObject door2;
    public GameObject door3;
    public GameObject door0;

    public GameObject exit;

    private Rigidbody rb;
    private int count;
    private int lives;
    private float movementX;
    private float movementY;

    public float dashForce = 15.0f; 
    public float dashCooldown = 1.5f; 
    private float nextDashTime = 0.0f; 

    private int dialogueCounter = 0;

    private bool isGrounded = true;

    public GameObject pauseScreen;
    public GameObject exitText;

    private bool isPaused = false;
    private bool canInteract = false;

    public GameObject restartScreen;
    public GameObject startScreen;
    public GameObject winScreen;
    public GameObject dialogueScreen;


    private bool noPausing = true;
    private static bool hasStarted = false;

    public AudioClip pickupSound;
    public AudioClip pauseSound;

    public AudioClip resumeSound;

    public AudioClip winSound;
    public AudioClip hitSound;


    public AudioClip loseSound;
    public AudioClip tntSound;

    public AudioClip heartupSound;
    public AudioClip jumpSound;
    public AudioClip dashSound;



    private AudioSource playerAudio;
    public GameObject music;

    private bool currentlyInDialogue = false;


    void Start(){
        rb = GetComponent<Rigidbody>();
        count = 0;
        lives = 3;
        playerAudio = GetComponent<AudioSource>();

        if(!hasStarted){
            Time.timeScale = 0f; 
            startScreen.SetActive(true);
        }
        else{
            startScreen.SetActive(false);
            Time.timeScale = 1f;
            SetText();
            countText.gameObject.SetActive(true);
            livesText.gameObject.SetActive(true);
            noPausing = false;
        }
    }

    void Update(){    
        if(Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame){
            if(canInteract && !currentlyInDialogue){
                noPausing = true;
                currentlyInDialogue = true;
                dialogueScreen.SetActive(true);
                dialogueText.gameObject.SetActive(false);
                Time.timeScale = 0f; 
                dialogueCounter = 0;
                dialogue.text = "Hey there Red! Quick story, I was taking a power nap and the pigs came in and took all the eggs. (Press E to continue)";
                playerAudio.PlayOneShot(pauseSound);
            }
            else if(currentlyInDialogue){
                dialogueCounter++;
                if(dialogueCounter == 1){
                    playerAudio.PlayOneShot(pauseSound);
                    dialogue.text = "You and the Blues wouldn't mind getting them back right? Perfect! I'm actually really tired from stressing over the eggs, so I'm going to take a quick power nap. (Press E to continue)";
                }
                else if(dialogueCounter == 2){
                    playerAudio.PlayOneShot(pauseSound);
                    dialogue.text = "Make sure to get the 12 they've got here, and then use the slingshot to get the rest. Good luck! (Press E to continue)";
                }
                else if(dialogueCounter > 2){
                    playerAudio.PlayOneShot(pauseSound);
                    dialogueScreen.SetActive(false);
                    Time.timeScale = 1f;
                    currentlyInDialogue = false;
                    dialogueCounter = 0;
                    door0.gameObject.SetActive(false);
                    dialogueText.gameObject.SetActive(true);
                    noPausing = false;
                }
            }
    }
}

    void OnMove(InputValue movementValue){
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void OnJump(InputValue value){
        if(isGrounded && canJump){
            playerAudio.PlayOneShot(jumpSound);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void OnDash(InputValue value){
        if(Time.time >= nextDashTime && canDash){
            Vector3 dashDirection = new Vector3(movementX, 0.0f, movementY).normalized;

            if(dashDirection != Vector3.zero){
                rb.AddForce(dashDirection * dashForce, ForceMode.Impulse);
                playerAudio.PlayOneShot(dashSound);
                nextDashTime = Time.time + dashCooldown; 
            }
        }
    }

    void OnPause(InputValue value){
        if(isPaused){
            ResumeGame();
        }
        else{
            PauseGame();
        }
    }

    public void StartGame(){
        hasStarted = true;
        startScreen.SetActive(false); 
        Time.timeScale = 1f;
        SetText();
        countText.gameObject.SetActive(true);
        livesText.gameObject.SetActive(true);
        noPausing = false;
    }

    public void PauseGame(){
        if(!noPausing){
            pauseScreen.SetActive(true);
            playerAudio.PlayOneShot(pauseSound); 
            Time.timeScale = 0f; 
            isPaused = true;
            canInteract = false;
        }
    }

    public void ResumeGame(){
        pauseScreen.SetActive(false); 
        playerAudio.PlayOneShot(resumeSound);
        Time.timeScale = 1f; 
        isPaused = false;
        canInteract = true;
    }

    public void RestartScreen(){
        restartScreen.SetActive(true); 
        noPausing = true;
        Time.timeScale = 0f; 
    }

    public void RetryGame(){
        restartScreen.SetActive(false);
        Time.timeScale = 1f; 
        SceneManager.LoadScene("Minigame");
        noPausing = false;
    }

    public void RestartGame(){
        restartScreen.SetActive(false);
        Time.timeScale = 1f; 
        if(MusicManager.instance != null){
            Destroy(MusicManager.instance.gameObject);
        }
        SceneManager.LoadScene("Minigame");
        noPausing = false;
        hasStarted = false;
        canJump = false; 
        canDash = false;
    }

    private void WinGame(){
        Time.timeScale = 0f; 
        winScreen.SetActive(true);
        noPausing = true;
    }

    public void NextScene(){
        SceneManager.LoadScene("WinScene");
    }

    IEnumerator TextTimer(){
        dialogueText.gameObject.SetActive(true);                   
        yield return new WaitForSeconds(3f); 
        dialogueText.gameObject.SetActive(false);                  
    }

    void SetText(){
        countText.text = "Count: " + count.ToString();
        livesText.text = "Lives: " + lives.ToString();
        if(door1Counter != null){
            door1Counter.text = "x" + (2-count).ToString();
        }
        if(door2Counter != null){
            door2Counter.text = "x" + (6-count).ToString();
        }
        if(door3Counter != null){
            door3Counter.text = "x" + (9-count).ToString();
        }
        if(exitCounter != null){
            exitCounter.text = "x" + (12-count).ToString();
        }
    }

    void FixedUpdate(){
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);

        if(gameObject.transform.position.y < -3){
            RestartScreen();
        }

        if(count >= 2){
            door1.gameObject.SetActive(false);
        }

        if(count >= 6){
            door2.gameObject.SetActive(false);
        }

        if(count >= 9){
            door3.gameObject.SetActive(false);
        }

        if(count >= 12){
            exit.gameObject.SetActive(true);
            exitText.SetActive(false);
        }

        if(lives == 0){
            playerAudio.PlayOneShot(loseSound);
            RestartScreen();
        }
    }

    private void OnCollisionEnter(Collision collision){
        if(collision.gameObject.CompareTag("Ground")){
            isGrounded = true;
        }

        if(collision.gameObject.CompareTag("Spawn")){
            dialogueText.gameObject.SetActive(true);
            canInteract = true;
        }

        if(collision.gameObject.CompareTag("Enemy")){
            lives--;
            SetText();
            gameObject.transform.position = new Vector3(0, 1, 0);
            playerAudio.PlayOneShot(hitSound);
        }
    }

    private void OnCollisionExit(Collision collision){
        if(collision.gameObject.CompareTag("Spawn")){
            dialogueText.gameObject.SetActive(false);
            canInteract = false;
        }
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Pickup")){
            other.gameObject.SetActive(false);
            count++;
            SetText();
            playerAudio.PlayOneShot(pickupSound);
        }

        if(other.gameObject.CompareTag("Exit")){
            NextScene();
        }

        if(other.gameObject.CompareTag("Nest")){
            WinGame();
            playerAudio.PlayOneShot(winSound);
        }

        if(other.gameObject.CompareTag("Jump")){
            other.gameObject.SetActive(false);
            canJump = true;
            dialogueText.text = "You can now jump by pressing SPACE!";
            playerAudio.PlayOneShot(jumpSound);
            StartCoroutine(TextTimer());
        }

        if(other.gameObject.CompareTag("Dash")){
            other.gameObject.SetActive(false);
            canDash = true;
            dialogueText.text = "You can now dash by pressing SHIFT!";
            playerAudio.PlayOneShot(dashSound);
            StartCoroutine(TextTimer());
        }

        if(other.gameObject.CompareTag("Heart")){
            other.gameObject.SetActive(false);
            lives++;
            playerAudio.PlayOneShot(heartupSound);
            SetText();
        }

        if(other.gameObject.CompareTag("TNT")){
            other.gameObject.SetActive(false);
            lives--;
            playerAudio.PlayOneShot(tntSound);
            SetText();
        }
    }
}

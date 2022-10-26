using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting;
using UnityEngine;
//using UnityEngine.Rendering;
//using UnityEngine.Android;

public class PlayerController : MonoBehaviour
{
    /******************/
    // Public variables
    /******************/
    public int speed;
    public int jumpForce;
    public int lives;
    public float levelTime; // In seconds
    public Canvas canvas;

    /*******************/
    // Private variables
    /*******************/
    private Rigidbody2D rb;
    private GameObject foot;
    private SpriteRenderer sprite;
    private Animator playerAnimation;
    private GameManager gameManager;
    private HUDController hud;
    private bool hasJumped;
    private float initialPosX, updatedPosX;

    private void Start()
    {
        GameManager.instance.ResumeGame();
        rb = GetComponent<Rigidbody2D>();
        foot = transform.Find("Foot").gameObject;

        // Get the sprite component of the child sprite object
        sprite = gameObject.transform.Find("player-idle-1").GetComponent<SpriteRenderer>();

        initialPosX = transform.position.x;
        // Get the animator controller of the sprite child object
        playerAnimation = gameObject.transform.Find("player-idle-1").GetComponent<Animator>();

        // Get the HUD Controller
        hud = canvas.GetComponent<HUDController>();
        hud.SetLivesTxt(lives);
        hud.SetPowerUpsTxt(GameObject.FindGameObjectsWithTag("PowerUp").Length);
    }

    private void FixedUpdate()
    {
        // Get the value from -1 to 1 of the horizontal move
        float inputX = Input.GetAxis("Horizontal");

        // Apply physic velocity to the object with the move value * speed, the Y coordenate is the same
        rb.velocity = new Vector2(inputX * speed, rb.velocity.y);

        // Calculate if the time is finnish
        if(levelTime <= 0f)
        {
            // TODO: To the HUD
            WinLevel(false);
            GameManager.instance.PauseGame();
            hud.SetTimesUpBox();
            //canvas.transform.Find("TimesUpBox").gameObject.SetActive(true);
        }
        else
        {
            levelTime -= Time.deltaTime;
            hud.SetTimeTxt((int)levelTime);
        }

        if (levelTime <= 0)
        {
            levelTime -= Time.deltaTime;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && TouchGround())
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            hasJumped = true;
            GameManager.instance.TotalJumps += 1; // Count how many jumps the character takes
        }

        // Changing the sprite
        if (rb.velocity.x > 0)
        {
            sprite.flipX = false;
        }
        else if (rb.velocity.x < 0)
        {
            sprite.flipX = true;
        }

        // Player animations
        PlayerAnimate();

        // Count how many steps the character takes according to its direction
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            updatedPosX = transform.position.x;
            GameManager.instance.Steps += updatedPosX - initialPosX;
            initialPosX = updatedPosX;
        } else if(Input.GetKeyUp(KeyCode.LeftArrow))
        {
            updatedPosX = transform.position.x;
            GameManager.instance.Steps += initialPosX - updatedPosX;
            initialPosX = updatedPosX;
        }
    }

    private bool TouchGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(foot.transform.position, Vector2.down, 0.2f);

        return hit.collider != null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            hasJumped = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PowerUp")
        {
            Destroy(collision.gameObject);
            Invoke(nameof(InfoPowerUp), 0.1f);
            GameManager.instance.Collectibles += 1;
        }
    }

    private void InfoPowerUp()
    {
        int powerupsNum = GameObject.FindGameObjectsWithTag("PowerUp").Length;
        hud.SetPowerUpsTxt(powerupsNum);

        if(powerupsNum == 0)
        {
            WinLevel(true);
            GameManager.instance.PauseGame();
            hud.SetWinBox();
            //canvas.transform.Find("WinBox").gameObject.SetActive(true);
        }
    }


    public void TakeDamage(int damage)
    {
        lives -= damage;
        hud.SetLivesTxt(lives);
        StartCoroutine(Damaged());

        if (lives == 0)
        {
            WinLevel(false);
            hud.SetLivesTxt(lives);
            GameManager.instance.PauseGame();
            hud.SetLoseLivesBox();
            //canvas.transform.Find("LoseLivesBox").gameObject.SetActive(true);
        }
    }

    
    private void PlayerAnimate()
    {
        // Player Jumping
        if (!TouchGround())
        {
            playerAnimation.Play("playerJump");
        }
        // Player Running
        else if (TouchGround() && Input.GetAxisRaw("Horizontal") != 0)
        {
            playerAnimation.Play("playerRunning");
        }
        // Player Idle
        else if (TouchGround() && Input.GetAxisRaw("Horizontal") == 0)
        {
            playerAnimation.Play("playerIdle");
        }
    }

    private void WinLevel(bool win)
    {
        GameManager.instance.Win = win;
        GameManager.instance.Score = (lives * 1000) + ((int)levelTime * 100);
    }

    IEnumerator Damaged()
    {
        sprite.color = new Color(255, 0, 0, 1);
        yield return new WaitForSeconds(0.5f);
        sprite.color = new Color(255, 255, 255, 1);
    }
}

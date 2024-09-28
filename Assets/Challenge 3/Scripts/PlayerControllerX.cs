using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver;

    public float floatForce;
    private float gravityModifier = 1.5f;
    private Rigidbody playerRb;

    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    private AudioSource playerAudio;
    public AudioClip moneySound;
    public AudioClip explodeSound;
    public AudioClip bounceSound;

    private bool LowEnough = true;
    public float maxHeight = 10.0f;
    public float bounceForce = 2.0f;


    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();

        // Apply a small upward force at the start of the game
        playerRb.AddForce(Vector3.up * 5, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        // While space is pressed and player is low enough, float up
        if (Input.GetKey(KeyCode.Space) && !gameOver && LowEnough)
        {
            playerRb.AddForce(Vector3.up * floatForce);
        }

        // Check if the balloon is above the max height
        if (transform.position.y > maxHeight)
        {
            LowEnough = false;
        }
        else
        {
            LowEnough = true;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // If player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle.Play();
            playerAudio.PlayOneShot(explodeSound, 1.0f);
            gameOver = true;
            Debug.Log("Game Over!");
            Destroy(other.gameObject);
        }

        // If player collides with money, trigger fireworks
        else if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play();
            playerAudio.PlayOneShot(moneySound, 1.0f);
            Destroy(other.gameObject);
        }

        // If player collides with the ground, apply bounce force
        if (other.gameObject.CompareTag("Ground"))
        {
            // Apply bounce force only once when the player hits the ground
            playerRb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);

            // Play the bounce sound effect
            if (playerAudio != null && bounceSound != null)
            {
                playerAudio.PlayOneShot(bounceSound);
            }
        }
    }
}

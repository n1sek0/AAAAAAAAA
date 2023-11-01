using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movimento : MonoBehaviour
{
    private CharacterController character;
    private Animator animator;
    private Vector3 inputs;
    private float directionY;
    [SerializeField] private float gravityForce = 8;
    [SerializeField] private float jumpForce = 8;
    [SerializeField] private float jumpAttackDamage = 10f;
    [SerializeField] private float knockbackForce = 10f; // Força do knockback
    private bool isJumping;

    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource jewelSound;
    [SerializeField] private AudioSource walkingSound;

    private float velocity = 3f;
    private int playerHealth = 20;

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        backgroundMusic.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth <= 0)
        {
            ResetScene();
            return;
        }

        Vector3 movement = Vector3.zero;

        if (!character.isGrounded)
        {
            directionY -= gravityForce * Time.deltaTime;
        }

        if (character.isGrounded && Input.GetButtonDown("Jump"))
        {
            directionY = jumpForce;
            animator.SetBool("jumping", true);
            isJumping = true;
            jumpSound.Play();
        }
        else if (character.isGrounded)
        {
            isJumping = false;
            animator.SetBool("jumping", false);
        }

        movement.y = directionY;
        movement *= Time.deltaTime;
        character.Move(movement);

        inputs.Set(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        character.Move(inputs * Time.deltaTime * velocity);

        if (inputs != Vector3.zero)
        {
            animator.SetBool("running", true);
            transform.forward = inputs;
            if (!walkingSound.isPlaying && !isJumping)
            {
                walkingSound.Play();
            }
        }
        else
        {
            animator.SetBool("running", false);
            walkingSound.Stop();
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (isJumping && hit.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = hit.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(jumpAttackDamage);
                playerHealth -= 10; // Reduz a vida do player ao atacar o inimigo

                // Aplica o knockback no jogador
                Rigidbody rb = GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Vector3 knockbackDirection = (transform.position - hit.gameObject.transform.position).normalized;
                    rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Joia"))
        {
            jewelSound.Play();
        }
    }

    private void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

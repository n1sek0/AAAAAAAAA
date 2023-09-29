using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimento : MonoBehaviour
{
    private CharacterController character;
    private Animator animator;
    private Vector3 inputs;
    private float directionY;
    [SerializeField] private float gravityForce = 8;
    [SerializeField] private float jumpForce = 8;
    private bool isJumping;

    private float velocidade = 3f;

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
        } else if (character.isGrounded)
        {
            isJumping = false;
            animator.SetBool("jumping", false);
        }

        movement.y = directionY;
        movement *= Time.deltaTime;
        character.Move(movement);
        
        inputs.Set(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        character.Move(inputs * Time.deltaTime * velocidade);
        Debug.Log(character.isGrounded);
        
        if (inputs != Vector3.zero)
        {
            animator.SetBool("running", true);
            transform.forward = inputs;
        }
        else
        {
            animator.SetBool("running", false);
        }

        //isJumping = false;
    }
}
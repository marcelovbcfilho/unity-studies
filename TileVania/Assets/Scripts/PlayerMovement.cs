using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float normalSpeed = 5f;
    [SerializeField] float jumpSpeed = 10f;
    [SerializeField] float climbingSpeed = 3f;
    [SerializeField] ParticleSystem myParticleSystem;
    [SerializeField] GameObject myGun;
    [SerializeField] GameObject bullet;
    float playerGravity = 4f;
    Vector2 inputValue;

    Rigidbody2D myRigidbody2D;
    CapsuleCollider2D myBodyCollider2D;
    BoxCollider2D myFeetCollider2D;
    Animator myAnimator;
    [SerializeField] bool isAlive = true;

    private void Awake()
    {
        this.myRigidbody2D = GetComponent<Rigidbody2D>();
        this.myAnimator = GetComponent<Animator>();
        this.myBodyCollider2D = GetComponent<CapsuleCollider2D>();
        this.myFeetCollider2D = GetComponent<BoxCollider2D>();
        this.playerGravity = this.myRigidbody2D.gravityScale;
    }

    void Start()
    {

    }

    void Update()
    {
        if (this.isAlive)
        {
            Run();
            FlipSprite();
            ClimbLadder();
        }
    }

    void Run()
    {
        Vector2 velocity = new Vector2(this.inputValue.x * this.normalSpeed, this.myRigidbody2D.velocity.y);
        this.myRigidbody2D.velocity = velocity;

        this.myAnimator.SetBool("isRunning", this.IsPlayerRunning());
    }

    void FlipSprite()
    {
        if (this.IsPlayerRunning())
            this.transform.localScale = new Vector2(Mathf.Sign(this.myRigidbody2D.velocity.x), 1f);
    }

    void ClimbLadder()
    {

        if (this.myBodyCollider2D.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            Vector2 velocity = new Vector2(this.myRigidbody2D.velocity.x, this.inputValue.y * this.climbingSpeed);
            this.myRigidbody2D.velocity = velocity;
            this.myRigidbody2D.gravityScale = 0;
        }
        else
        {
            this.myRigidbody2D.gravityScale = this.playerGravity;
        }



        this.myAnimator.SetBool("isClimbing", this.IsPlayerClimbing());
    }

    void OnMove(InputValue inputValue)
    {
        this.inputValue = inputValue.Get<Vector2>();
    }

    void OnJump(InputValue inputValue)
    {
        if (inputValue.isPressed && this.myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground")) && this.isAlive)
        {
            this.myRigidbody2D.velocity += new Vector2(0f, this.jumpSpeed);
        }
    }

    void OnFire(InputValue inputValue)
    {
        if (this.isAlive)
        {
            GameObject bullet = Instantiate(this.bullet, this.myGun.transform.position, this.myGun.transform.rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(this.transform.localScale.x * 20f, 2f);
            Destroy(bullet, 1f);
        }
    }

    bool IsPlayerRunning()
    {
        return Mathf.Abs(this.myRigidbody2D.velocity.x) > Mathf.Epsilon;
    }

    bool IsPlayerClimbing()
    {
        return this.myBodyCollider2D.IsTouchingLayers(LayerMask.GetMask("Ladder")) && Mathf.Abs(this.myRigidbody2D.velocity.y) > Mathf.Epsilon;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Spike")
        {
            Die();
        }
    }

    void Die()
    {
        this.isAlive = false;
        this.myAnimator.SetTrigger("Dead");
        this.myParticleSystem.Play();
        FindObjectOfType<GameSession>().ProccessPlayerDeath();
    }
}

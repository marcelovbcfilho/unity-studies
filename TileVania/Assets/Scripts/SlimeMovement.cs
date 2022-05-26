using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMovement : MonoBehaviour
{
    [SerializeField] float walkingSpeed = 1f;
    [SerializeField] ParticleSystem myParticleSystem;
    Rigidbody2D myRigidbody2D;
    CapsuleCollider2D myCapsuleCollider;
    Animator myAnimator;
    SpriteRenderer mySpriteRenderer;
    bool isAlive = true;

    void Awake()
    {
        this.myRigidbody2D = this.GetComponent<Rigidbody2D>();
        this.myCapsuleCollider = this.GetComponent<CapsuleCollider2D>();
        this.myAnimator = GetComponent<Animator>();
        this.mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        if (Random.value > 0.5f)
        {
            this.transform.localScale = new Vector2(-(Mathf.Sign(this.myRigidbody2D.velocity.x)), 1f);
            this.walkingSpeed = -this.walkingSpeed;
        }
    }

    void Update()
    {
        if (isAlive)
        {
            this.myAnimator.SetBool("isWalking", true);
            this.myRigidbody2D.velocity = new Vector2(this.walkingSpeed, 0);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        this.transform.localScale = new Vector2(-(Mathf.Sign(this.myRigidbody2D.velocity.x)), 1f);
        this.walkingSpeed = -this.walkingSpeed;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            Die();
        }
    }

    void Die()
    {
        this.isAlive = false;
        this.myParticleSystem.Play();
        this.myCapsuleCollider.enabled = false;
        this.myRigidbody2D.simulated = false;
        this.mySpriteRenderer.enabled = false;
        Invoke("DisableSlime", 1f);
    }

    void DisableSlime()
    {
        this.myParticleSystem.Stop();
        Destroy(this.gameObject, 2f);
    }
}

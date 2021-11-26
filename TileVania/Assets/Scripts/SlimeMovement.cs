using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMovement : MonoBehaviour
{
    [SerializeField] float walkingSpeed = 1f;
    Rigidbody2D myRigidbody2D;
    CapsuleCollider2D myCapsuleCollider;
    Animator myAnimator;

    void Awake()
    {
        this.myRigidbody2D = this.GetComponent<Rigidbody2D>();
        this.myCapsuleCollider = this.GetComponent<CapsuleCollider2D>();
        this.myAnimator = GetComponent<Animator>();
    }

    void Start()
    {

    }

    void Update()
    {
        this.myAnimator.SetBool("isWalking", true);
        this.myRigidbody2D.velocity = new Vector2(this.walkingSpeed, 0);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        this.transform.localScale = new Vector2(-(Mathf.Sign(this.myRigidbody2D.velocity.x)), 1f);
        this.walkingSpeed = -this.walkingSpeed;
    }
}

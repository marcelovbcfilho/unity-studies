using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float torqueAmount = 30f;
    [SerializeField] private float boostSpeed = 40f;
    [SerializeField] private float normalSpeed = 20f;

    private SurfaceEffector2D surfaceEffector2D;
    private Rigidbody2D rigidbody2D;
    private bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        this.rigidbody2D = this.GetComponent<Rigidbody2D>();
        this.surfaceEffector2D = FindObjectOfType<SurfaceEffector2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.canMove)
        {
            RotatePlayer();
            RespondToBoost();
        }
    }

    private void RespondToBoost()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            this.surfaceEffector2D.speed = this.boostSpeed;
        }
        else
        {
            this.surfaceEffector2D.speed = this.normalSpeed;
        }
    }

    private void RotatePlayer()
    {
        if (Input.GetKey(KeyCode.A))
        {
            this.rigidbody2D.AddTorque(this.torqueAmount);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            this.rigidbody2D.AddTorque(-this.torqueAmount);
        }
    }

    public void DisableControls()
    {
        this.canMove = false;
    }
}

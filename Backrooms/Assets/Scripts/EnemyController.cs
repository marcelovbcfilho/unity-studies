using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy objects")]
    [Tooltip("Blood prefab particle that will be displayed when enemy dies")]
    public GameObject bloodEffect;

    [Tooltip("Target to follow")]
    private Transform target;

    [Tooltip("Animator controller")]
    public Animator animator;

    [Tooltip("Current colliding controller")]
    public PlayerController playerController;

    [Header("Enemy status")]
    [Tooltip("Enemy health")]
    public float health = 100;

    [Tooltip("Enemy speed")]
    public float speed = 4f;

    [Tooltip("Is alive ?")]
    public bool alive = true;

    [Tooltip("Touching player")]
    public bool touchingPlayer = false;

    [Header("Enemy UI")]
    [Tooltip("Enemy health bar")]
    public Image healthBar;

    [Tooltip("Enemy canvas UI")]
    public Canvas ui;

    private bool playerFound = false;

    private void Awake()
    {
        this.target = GameObject.FindGameObjectWithTag("Player").transform;
        this.animator = this.GetComponentInChildren<Animator>();
    }

    private void FixedUpdate()
    {
        if (this.alive)
        {
            if (Mathf.Abs(this.transform.position.x - this.target.position.x) < 6 && Mathf.Abs(this.transform.position.z - this.target.position.z) < 6)
            {
                this.playerFound = true;
            }

            if (this.playerFound)
            {
                transform.position = Vector3.MoveTowards(this.transform.position, this.target.position, this.speed * Time.deltaTime);
                this.animator.SetBool("running", true);
            }
        }
    }

    private void LateUpdate()
    {
        if (this.alive)
        {
            this.ui.transform.LookAt(this.target);
            this.transform.LookAt(new Vector3(this.target.position.x, this.transform.position.y, this.target.position.z));
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            this.touchingPlayer = true;
            InvokeRepeating("DamagePlayer", 0f, 1f);
            this.playerController = other.gameObject.GetComponent<PlayerController>();
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            this.touchingPlayer = false;
            CancelInvoke("DamagePlayer");
        }
    }

    public void DamagePlayer()
    {
        this.playerController.TakeDamage(20);
    }

    public void TakeDamage(RaycastHit hit, float damageAmount)
    {
        this.health = Mathf.Max(0, this.health - damageAmount);
        this.healthBar.fillAmount = this.health / 100;

        if (this.alive)
            Instantiate(this.bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));

        if (this.health == 0)
            this.Die();
    }

    public void Die()
    {
        if (this.alive)
        {
            this.alive = false;
            this.gameObject.GetComponent<Rigidbody>().freezeRotation = true;
            this.gameObject.GetComponent<BoxCollider>().center = new Vector3(0, 0.25f, 0);
            this.gameObject.GetComponent<BoxCollider>().size = new Vector3(0.6f, 0.5f, 0.3f);
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().AddKill();
            this.animator.SetBool("falling", true);
            Destroy(this.gameObject, 3f);
        }
    }
}

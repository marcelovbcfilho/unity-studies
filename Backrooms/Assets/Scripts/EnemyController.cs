using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    private static readonly int Running = Animator.StringToHash("running");

    [Header("Enemy objects")] [Tooltip("Blood prefab particle that will be displayed when enemy dies")]
    public GameObject bloodEffect;

    [Tooltip("Animator controller")] public Animator animator;

    [Tooltip("Current colliding controller")]
    public PlayerController playerController;

    [Header("Enemy status")] [Tooltip("Enemy health")]
    public float health = 100;

    [Tooltip("Enemy speed")] public float speed = 4f;

    [Tooltip("Is alive ?")] public bool alive = true;

    [Tooltip("Touching player")] public bool touchingPlayer;

    [Header("Enemy UI")] [Tooltip("Enemy health bar")]
    public Image healthBar;

    [Tooltip("Enemy canvas UI")] public Canvas ui;

    private bool _playerFound;

    [Tooltip("Target to follow")] private Transform _target;

    private void Awake()
    {
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponentInChildren<Animator>();
    }

    private void FixedUpdate()
    {
        if (alive)
        {
            if (Mathf.Abs(transform.position.x - _target.position.x) < 6 &&
                Mathf.Abs(transform.position.z - _target.position.z) < 6) _playerFound = true;

            if (_playerFound)
            {
                transform.position = Vector3.MoveTowards(transform.position, _target.position, speed * Time.deltaTime);
                animator.SetBool(Running, true);
            }
        }
    }

    private void LateUpdate()
    {
        if (alive)
        {
            ui.transform.LookAt(_target);
            transform.LookAt(new Vector3(_target.position.x, transform.position.y, _target.position.z));
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            touchingPlayer = true;
            InvokeRepeating(nameof(DamagePlayer), 0f, 1f);
            playerController = other.gameObject.GetComponent<PlayerController>();
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            touchingPlayer = false;
            CancelInvoke(nameof(DamagePlayer));
        }
    }

    public void DamagePlayer()
    {
        playerController.TakeDamage(20);
    }

    public void TakeDamage(RaycastHit hit, float damageAmount)
    {
        health = Mathf.Max(0, health - damageAmount);
        healthBar.fillAmount = health / 100;

        if (alive)
            Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));

        if (health == 0)
            Die();
    }

    private void Die()
    {
        if (alive)
        {
            alive = false;
            gameObject.GetComponent<Rigidbody>().freezeRotation = true;
            gameObject.GetComponent<BoxCollider>().center = new Vector3(0, 0.25f, 0);
            gameObject.GetComponent<BoxCollider>().size = new Vector3(0.6f, 0.5f, 0.3f);
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().AddKill();
            animator.SetBool("falling", true);
            Destroy(gameObject, 3f);
        }
    }
}
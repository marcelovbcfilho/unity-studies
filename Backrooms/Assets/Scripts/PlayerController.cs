using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Tooltip("Player kill count")] public int killCount;

    [Tooltip("Player health")] public float health = 100;

    [Tooltip("Player is alive")] public bool alive = true;

    [Tooltip("Player health bar")] public Image healthBar;

    [Tooltip("HUD kill count")] private TextMeshProUGUI HudKillCount;

    private void Awake()
    {
        HudKillCount = GameObject.Find("HUD_kills").GetComponent<TextMeshProUGUI>();
    }

    private void FixedUpdate()
    {
        if (!alive)
            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(transform.position.x, transform.position.y - 0.007f, transform.position.z),
                1 * Time.deltaTime);
    }

    public void AddKill()
    {
        killCount++;
        HudKillCount.text = killCount + "";
    }

    public void TakeDamage(float damageAmount)
    {
        health = Mathf.Max(0, health - damageAmount);
        healthBar.fillAmount = health / 100;

        if (health == 0 && alive)
            StartCoroutine("Die");
    }

    public IEnumerator Die()
    {
        alive = false;
        transform.GetComponent<Rigidbody>().isKinematic = true;
        transform.GetComponent<CapsuleCollider>().enabled = false;
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Procedural");
    }
}
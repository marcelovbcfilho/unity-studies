using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Tooltip("Player kill count")]
    public int killCount = 0;

    [Tooltip("HUD kill count")]
    TextMeshProUGUI HudKillCount;

    [Tooltip("Player health")]
    public float health = 100;

    [Tooltip("Player is alive")]
    public bool alive = true;

    [Tooltip("Player health bar")]
    public Image healthBar;

    private void Awake()
    {
        this.HudKillCount = GameObject.Find("HUD_kills").GetComponent<TextMeshProUGUI>();
    }

    private void FixedUpdate()
    {
        if (!this.alive)
            this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(this.transform.position.x, this.transform.position.y - 0.007f, this.transform.position.z), 1 * Time.deltaTime);
    }

    public void AddKill()
    {
        this.killCount++;
        this.HudKillCount.text = this.killCount + "";
    }

    public void TakeDamage(float damageAmount)
    {
        this.health = Mathf.Max(0, this.health - damageAmount);
        this.healthBar.fillAmount = this.health / 100;

        if (this.health == 0 && this.alive)
            StartCoroutine("Die");
    }

    public IEnumerator Die()
    {
        this.alive = false;
        this.transform.GetComponent<Rigidbody>().isKinematic = true;
        this.transform.GetComponent<CapsuleCollider>().enabled = false;
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Procedural");
    }
}

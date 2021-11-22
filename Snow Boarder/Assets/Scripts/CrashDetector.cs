using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrashDetector : MonoBehaviour
{
    [SerializeField] private float reloadSceneTimout = 0.5f;
    [SerializeField] private ParticleSystem crashEffect;
    [SerializeField] private AudioClip crashSFX;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Ground")
        {
            FindObjectOfType<PlayerController>().DisableControls();
            this.crashEffect.Play();
            this.GetComponent<AudioSource>().PlayOneShot(this.crashSFX);
            Invoke("ReloadScene", this.reloadSceneTimout);
        }
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }
}

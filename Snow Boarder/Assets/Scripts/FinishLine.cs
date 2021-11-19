using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    [SerializeField] private float reloadSceneTimout = 0.5f;
    [SerializeField] private ParticleSystem finishEffect;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            this.finishEffect.Play();
            Invoke("ReloadScene", this.reloadSceneTimout);
        }
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }
}

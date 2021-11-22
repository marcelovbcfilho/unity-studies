using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustTrail : MonoBehaviour
{
    [SerializeField] private ParticleSystem dustTrail;

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Ground" && this.dustTrail.isStopped) {
            this.dustTrail.Play();
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
        if (other.gameObject.tag == "Ground" && this.dustTrail.isPlaying) {
            this.dustTrail.Stop();
        }
    }
}

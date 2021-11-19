using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delivery : MonoBehaviour
{
    [SerializeField] private Color32 withPackage = new Color32(1, 1, 1, 1);
    [SerializeField] private Color32 withoutPackage = new Color32(1, 1, 1, 1);
    private bool hasPackage = false;
    [SerializeField] private float destroyDelay = 0.5f;
    private SpriteRenderer spriteRenderer;

    private void Start() {
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Package" && !this.hasPackage)
        {
            Debug.Log("Package");
            this.hasPackage = true;
            Destroy(other.gameObject, this.destroyDelay);
            this.spriteRenderer.color = this.withPackage;
            this.transform.GetChild(0).gameObject.SetActive(true);
        }
        else if (other.tag == "Customer" && this.hasPackage)
        {
            Debug.Log("Customer");
            this.hasPackage = false;
            this.transform.GetChild(0).gameObject.SetActive(false);
            this.spriteRenderer.color = this.withoutPackage;
        }
    }
}

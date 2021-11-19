using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Driver : MonoBehaviour
{

    [SerializeField] float steerSpeed = 100f;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float normalSpeed = 5f;
    [SerializeField] float boostSpeed = 12f;
    [SerializeField] float roadBoostSpeed = 30f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && this.moveSpeed == this.normalSpeed)
            this.moveSpeed = this.boostSpeed;
        else if (this.moveSpeed == this.boostSpeed)
            this.moveSpeed = this.normalSpeed;

        float steerAmount = Input.GetAxis("Horizontal") * this.steerSpeed * Time.deltaTime;
        float moveAmount = Input.GetAxis("Vertical") * this.moveSpeed * Time.deltaTime;
        transform.Rotate(0, 0, -steerAmount);
        transform.Translate(0, moveAmount, 0);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        this.moveSpeed = this.normalSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Boost")
            this.moveSpeed = this.roadBoostSpeed;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;
    [SerializeField] private float torqueAmount = 30f;

    // Start is called before the first frame update
    void Start()
    {
        this.rigidbody2D = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            this.rigidbody2D.AddTorque(this.torqueAmount);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            this.rigidbody2D.AddTorque(-this.torqueAmount);
        }
    }
}

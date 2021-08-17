using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPlayerMovement : MonoBehaviour
{
    public float speed = 1.0f;
    public float jumpForce = 10.0f;
    public bool onFloor = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        if (Input.GetKey(KeyCode.W))
        {
            transform.position = transform.position + forward * speed;
        }
        if (Input.GetKey(KeyCode.S)) {
            transform.position = transform.position + forward * -speed;
        }

        if (Input.GetKey(KeyCode.Space) && onFloor) {
            Rigidbody r = GetComponent<Rigidbody>();
            Vector3 j = new Vector3(0, jumpForce, 0);
            r.AddForce(j);
        }

    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Terrain") {
            onFloor = true;
        }
    }

    private void OnCollisionExit(Collision collision) {
        if (collision.gameObject.tag == "Terrain") {
            onFloor = false;
        }
    }
}

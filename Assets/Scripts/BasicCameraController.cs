using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCameraController : MonoBehaviour {
    [Range(0.01f, 0.1f)]
    public float speed = 0.05f;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 up = transform.TransformDirection(Vector3.up);
        Vector3 right = transform.TransformDirection(Vector3.right);
        if (Input.GetKey(KeyCode.W)) {
            transform.position = transform.position + forward * speed;
        }
        if (Input.GetKey(KeyCode.S)) {
            transform.position = transform.position + forward * -speed;
        }

        if (Input.GetKey(KeyCode.Space)) {
            transform.position = transform.position + up * speed;
        }
        if (Input.GetKey(KeyCode.LeftControl)) {
            transform.position = transform.position + up * -speed;
        }

        if (Input.GetKey(KeyCode.D)) {
            transform.position = transform.position + right * speed;
        }
        if (Input.GetKey(KeyCode.A)) {
            transform.position = transform.position + right * -speed;
        }
    }

}

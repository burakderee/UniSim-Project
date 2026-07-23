using UnityEngine;

public class VehicleController : MonoBehaviour
{
    Rigidbody rb;
    public float moveForce = 10f;
    public float rotateSpeed = 80f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        rb.AddForce(transform.forward * v * moveForce);
        transform.Rotate(0, h * rotateSpeed * Time.deltaTime, 0);
    }
}
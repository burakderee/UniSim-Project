using UnityEngine;

public class SuAraciController : MonoBehaviour
{
    Rigidbody rb;
    public float motorKuvveti = 500f;
    public float donusHizi = 50f;
    public float maksHiz = 15f;
    public float yuzmeYuksekligi = 0.7f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.drag = 3f;
    }

    void FixedUpdate()
    {
        // Yüzeyde tut
        float yukseklikFarki = yuzmeYuksekligi - transform.position.y;
        rb.AddForce(Vector3.up * yukseklikFarki * 100f);

        // Ýleri geri hareket
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        if (rb.velocity.magnitude < maksHiz)
            rb.AddForce(transform.forward * v * motorKuvveti);

        transform.Rotate(0, h * donusHizi * Time.deltaTime, 0);
    }
}
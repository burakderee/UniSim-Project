using UnityEngine;

public class SuBariyeri : MonoBehaviour
{
    public float itmeKuvveti = 50f;

    void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null && other.gameObject.name == "KaraAraci")
        {
            rb.AddForce(Vector3.up * itmeKuvveti, ForceMode.Impulse);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "KaraAraci")
        {
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
                rb.velocity = new Vector3(rb.velocity.x * -0.5f, rb.velocity.y, rb.velocity.z * -0.5f);
        }
    }
}
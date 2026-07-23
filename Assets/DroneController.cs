using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class DroneController : MonoBehaviour
{
    Rigidbody rb;

    [Header("Uçuþ Ayarlarý")]
    public float thrustForce = 15f;
    public float moveForce = 8f;
    public float rotateSpeed = 60f;

    [Header("Pervane Ayarlarý")]
    public float pervaneDonusHizi = 2500f;

    [Tooltip("Bulunan pervaneler oyun baþladýðýnda buraya otomatik eklenecek.")]
    public List<Transform> pervaneler = new List<Transform>();

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Drone'un havada çok fazla savrulmamasý için ideal sürtünme deðerleri
        rb.drag = 2f;
        rb.angularDrag = 3f;

        // Oyun baþlar baþlamaz modelin altýndaki pervaneleri otomatik tespit et
        PervaneleriOtomatikBul();
    }

    void FixedUpdate()
    {
        // Girdileri (Input) oku
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        // --- Dikey Hareket (Yukarý / Aþaðý) ---
        if (Input.GetKey(KeyCode.Space))
            rb.AddForce(Vector3.up * thrustForce, ForceMode.Acceleration);
        if (Input.GetKey(KeyCode.LeftShift))
            rb.AddForce(Vector3.down * thrustForce, ForceMode.Acceleration);

        // --- Yatay Hareket (Ýleri / Geri / Sað / Sol) ---
        rb.AddForce(transform.forward * v * moveForce, ForceMode.Acceleration);
        rb.AddForce(transform.right * h * moveForce, ForceMode.Acceleration);

        // --- Dönüþ (Q ve E Tuþlarý) ---
        if (Input.GetKey(KeyCode.Q))
            transform.Rotate(0, -rotateSpeed * Time.fixedDeltaTime, 0);
        if (Input.GetKey(KeyCode.E))
            transform.Rotate(0, rotateSpeed * Time.fixedDeltaTime, 0);
    }

    void Update()
    {
        // Taranýp bulunan tüm pervaneleri her karede kendi ekseninde döndür
        foreach (Transform pervane in pervaneler)
        {
            if (pervane != null)
            {
                // Space.Self önemlidir: Drone takla atsa bile pervane hep kendi merkezinde döner
                pervane.Rotate(Vector3.up * pervaneDonusHizi * Time.deltaTime, Space.Self);
            }
        }
    }

    // --- Akýllý Pervane Tarayýcý ---
    public void PervaneleriOtomatikBul()
    {
        pervaneler.Clear();

        // Objenin altýndaki tüm çocuk (child) nesneleri tarar
        Transform[] tumAltObjeler = GetComponentsInChildren<Transform>();

        foreach (Transform obje in tumAltObjeler)
        {
            string isim = obje.name.ToLower();

            // Eðer objenin adýnda bu kelimelerden biri geçiyorsa pervane olarak kabul et
            if (isim.Contains("pervane") || isim.Contains("prop") || isim.Contains("rotor") || isim.Contains("blade"))
            {
                pervaneler.Add(obje);
            }
        }

        Debug.Log("<color=green>Drone Sistemi:</color> " + pervaneler.Count + " adet dönen pervane baþarýyla baðlandý!");
    }
}
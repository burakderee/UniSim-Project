using UnityEngine;
using System.Collections.Generic;

public class KaraAraciController : MonoBehaviour
{
    Rigidbody rb;

    [Header("Motor ve Yokuþ")]
    public float motorKuvveti = 9000f;
    public float maksHiz = 9f;
    public float gercekFrenKuvveti = 30000f;
    public float yokusZorlukCarpani = 0.5f;

    [Header("Direksiyon (Aðaçlardan Kaçmak Ýçin Keskinleþtirildi)")]
    public float maksDonus = 400f;
    public float direksiyonToplanmaHizi = 25.0f;

    [Header("Yol Tutuþu ve Fizik")]
    public float aracAgirligi = 1200f;
    public float yerCekimiKuvveti = 15f;
    public float yanKaymaOrani = 0.05f;

    [Header("Doðal Salýným Ayarlarý")]
    public float bostaSurtunme = 1.5f;
    public float gazdaSurtunme = 0.1f;

    [Header("--- YENÝ: GÖRSEL TEKERLEK AYARLARI ---")]
    public float tekerlekGorselHizi = 800f; // Tekerleklerin kendi etrafýnda dönme hýzý
    public float tekerlekMaksDonus = 35f;   // Ön tekerleklerin direksiyon kýrýlma açýsý
    public List<Transform> onTekerlekler = new List<Transform>();
    public List<Transform> arkaTekerlekler = new List<Transform>();

    float mevcutDonus = 0f;
    float anlikDireksiyonGirdisi = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass = aracAgirligi;
        rb.angularDrag = 5f;

        rb.centerOfMass = new Vector3(0, -0.5f, 0);

        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            PhysicMaterial sifirSurtunme = new PhysicMaterial("SifirSurtunme");
            sifirSurtunme.dynamicFriction = 0f;
            sifirSurtunme.staticFriction = 0f;
            sifirSurtunme.frictionCombine = PhysicMaterialCombine.Minimum;
            col.material = sifirSurtunme;
        }

        // --- ARACI TOPRAKTAN KURTAR ---
        Terrain aktifTerrain = Terrain.activeTerrain;
        if (aktifTerrain != null)
        {
            float yuzeyYuksekligi = aktifTerrain.SampleHeight(transform.position) + aktifTerrain.transform.position.y;
            if (transform.position.y <= yuzeyYuksekligi + 1f)
            {
                transform.position = new Vector3(transform.position.x, yuzeyYuksekligi + 2f, transform.position.z);
            }
        }

        // Kod baþlarken tekerlekleri modelin içinden otomatik bulsun
        TekerlekleriOtomatikBul();
    }

    void FixedUpdate()
    {
        // ... (Senin yazdýðýn fizik kodunun tamamý burada, HÝÇ DEÐÝÞTÝRÝLMEDÝ) ...
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        float lokalIleriHiz = transform.InverseTransformDirection(rb.velocity).z;
        float gercekYatayHiz = Mathf.Abs(lokalIleriHiz);

        float burnunEgimi = Vector3.Dot(transform.forward, Vector3.up);
        float hareketYonu = v != 0 ? Mathf.Sign(v) : (gercekYatayHiz > 0.1f ? Mathf.Sign(lokalIleriHiz) : 1f);
        float efektifEgim = burnunEgimi * hareketYonu;

        float aktifMotorKuvveti = motorKuvveti;
        float aktifMaksHiz = maksHiz;

        if (efektifEgim > 0)
        {
            aktifMotorKuvveti -= (motorKuvveti * efektifEgim * yokusZorlukCarpani);
            aktifMotorKuvveti = Mathf.Max(0, aktifMotorKuvveti);

            aktifMaksHiz -= (maksHiz * efektifEgim * 1.0f);
            aktifMaksHiz = Mathf.Max(5.5f, aktifMaksHiz);
        }
        else if (efektifEgim < 0)
        {
            aktifMaksHiz += (maksHiz * Mathf.Abs(efektifEgim) * 0.8f);
        }

        anlikDireksiyonGirdisi = Mathf.Lerp(anlikDireksiyonGirdisi, h, direksiyonToplanmaHizi * Time.fixedDeltaTime);
        float donusYeterliligi = Mathf.Clamp01(gercekYatayHiz / 0.5f);

        mevcutDonus = anlikDireksiyonGirdisi * maksDonus * donusYeterliligi;

        if (gercekYatayHiz > 0.1f)
        {
            float yon = lokalIleriHiz >= 0 ? 1f : -1f;
            transform.Rotate(0, mevcutDonus * Time.fixedDeltaTime * yon, 0);
        }

        if (v != 0)
        {
            rb.drag = gazdaSurtunme;

            if ((v > 0 && lokalIleriHiz < -1f) || (v < 0 && lokalIleriHiz > 1f))
            {
                rb.AddForce(-transform.forward * Mathf.Sign(lokalIleriHiz) * gercekFrenKuvveti, ForceMode.Force);
            }
            else if (gercekYatayHiz < aktifMaksHiz)
            {
                Vector3 motorYon = transform.forward * v;
                rb.AddForce(motorYon * aktifMotorKuvveti, ForceMode.Force);
            }
        }
        else
        {
            if (efektifEgim < -0.1f)
            {
                rb.drag = Mathf.Lerp(bostaSurtunme, gazdaSurtunme, Mathf.Abs(efektifEgim) * 1.0f);
            }
            else
            {
                rb.drag = bostaSurtunme;
            }
        }

        Vector3 yerCekimiYon = Vector3.down * yerCekimiKuvveti;
        rb.AddForce(yerCekimiYon, ForceMode.Acceleration);

        Vector3 yanHiz = transform.InverseTransformDirection(rb.velocity);
        yanHiz.x *= yanKaymaOrani;
        rb.velocity = transform.TransformDirection(yanHiz);
    }

    // --- YENÝ EKLENEN KISIM: TEKERLEK GÖRSELLÝÐÝ ---
    void Update()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        // 1. Ýleri Geri Dönüþ Efekti (Bütün tekerlekler)
        foreach (Transform t in onTekerlekler)
        {
            if (t != null) t.Rotate(Vector3.right * v * tekerlekGorselHizi * Time.deltaTime, Space.Self);
        }
        foreach (Transform t in arkaTekerlekler)
        {
            if (t != null) t.Rotate(Vector3.right * v * tekerlekGorselHizi * Time.deltaTime, Space.Self);
        }

        // 2. Direksiyon Kýrýlma Efekti (Sadece ön tekerlekler)
        float hedefAci = h * tekerlekMaksDonus;
        foreach (Transform t in onTekerlekler)
        {
            if (t != null)
            {
                Vector3 mevcutAci = t.localEulerAngles;
                // Sadece Y eksenini (sað/sol dönüþünü) deðiþtir, diðer dönüþleri koru
                t.localRotation = Quaternion.Euler(mevcutAci.x, hedefAci, mevcutAci.z);
            }
        }
    }

    void TekerlekleriOtomatikBul()
    {
        onTekerlekler.Clear();
        arkaTekerlekler.Clear();

        Transform[] tumAltObjeler = GetComponentsInChildren<Transform>();

        foreach (Transform obje in tumAltObjeler)
        {
            string isim = obje.name.ToLower();

            if (isim.Contains("wheel") || isim.Contains("teker") || isim.Contains("tyre"))
            {
                if (isim.Contains("front") || isim.Contains("on") || isim.Contains("f_"))
                {
                    onTekerlekler.Add(obje);
                }
                else
                {
                    arkaTekerlekler.Add(obje);
                }
            }
        }
        Debug.Log($"<color=yellow>Araç Sistemi:</color> {onTekerlekler.Count} Ön, {arkaTekerlekler.Count} Arka tekerlek bulundu.");
    }
}
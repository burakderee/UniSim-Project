using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HavaDurumu : MonoBehaviour
{
    [Header("Rüzgar")]
    public float ruzgarHizi = 0f;
    public float ruzgarYonu = 0f;

    [Header("Sis")]
    public float sisDensitesi = 0f;

    [Header("Sýcaklýk")]
    public float sicaklik = 20f;

    [Header("UI")]
    public GameObject havaPanel;
    public Slider ruzgarHiziSlider;
    public Slider sisSlider;
    public Slider sicaklikSlider;
    public TextMeshProUGUI ruzgarText;
    public TextMeshProUGUI sisText;
    public TextMeshProUGUI sicaklikText;

    [Header("Araçlar")]
    public Rigidbody ihaRb;
    public Rigidbody karaAraciRb;
    public Rigidbody suAraciRb;

    void Start()
    {
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.Exponential;
        RenderSettings.fogDensity = 0f;

        ruzgarHiziSlider.minValue = 0; ruzgarHiziSlider.maxValue = 30;
        sisSlider.minValue = 0; sisSlider.maxValue = 1;
        sicaklikSlider.minValue = -20; sicaklikSlider.maxValue = 50;
        sicaklikSlider.value = 20;

        ruzgarHiziSlider.onValueChanged.AddListener(v => { ruzgarHizi = v; GuncelleUI(); });
        sisSlider.onValueChanged.AddListener(v => { sisDensitesi = v; GuncelleUI(); });
        sicaklikSlider.onValueChanged.AddListener(v => { sicaklik = v; GuncelleUI(); });

        GuncelleUI();
    }

    void FixedUpdate()
    {
        RenderSettings.fogDensity = sisDensitesi * 0.05f;

        Vector3 ruzgarVektoru = Quaternion.Euler(0, ruzgarYonu, 0) * Vector3.forward * ruzgarHizi;

        UygulaNesne(ihaRb, ruzgarVektoru);
        UygulaNesne(karaAraciRb, ruzgarVektoru);
        UygulaNesne(suAraciRb, ruzgarVektoru);
    }

    void UygulaNesne(Rigidbody rb, Vector3 kuvvet)
    {
        if (rb != null && rb.gameObject.activeInHierarchy)
            rb.AddForce(kuvvet * 0.1f, ForceMode.Force);
    }

    void GuncelleUI()
    {
        ruzgarText.text = $"Rüzgar: {ruzgarHizi:F0} m/s";
        sisText.text = $"Sis: {sisDensitesi:F2}";
        sicaklikText.text = $"Sýcaklýk: {sicaklik:F0}°C";
    }

    public float BataryaCarpani()
    {
        return sicaklik < 0 ? 1f + Mathf.Abs(sicaklik) * 0.02f : 1f;
    }
}
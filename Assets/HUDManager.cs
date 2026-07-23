using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [Header("Eski HUD (kapat)")]
    public TextMeshProUGUI hudText;

    [Header("Yeni HUD")]
    public Slider hizBar;
    public Slider bataryaBar;
    public TextMeshProUGUI yukseklikDeger;
    public TextMeshProUGUI hizDeger;
    public TextMeshProUGUI bataryaDeger;

    public GameObject iha;
    public GameObject karaAraci;
    public GameObject suAraci;

    float batarya = 100f;
    float bataryaTuketim = 2f;
    float maksHiz = 30f;

    void Start()
    {
        if (hudText != null) hudText.gameObject.SetActive(false);

        if (hizBar != null) { hizBar.minValue = 0; hizBar.maxValue = 1; }
        if (bataryaBar != null) { bataryaBar.minValue = 0; bataryaBar.maxValue = 1; bataryaBar.value = 1; }
    }

    void Update()
    {
        string secilenArac = PlayerPrefs.GetString("SecilenArac", "IHA");
        GameObject aktifArac = secilenArac == "IHA" ? iha :
                               secilenArac == "KaraAraci" ? karaAraci : suAraci;

        if (aktifArac == null) return;

        Rigidbody rb = aktifArac.GetComponent<Rigidbody>();
        if (rb == null) return;

        float hiz = rb.velocity.magnitude * 3.6f;
        float yukseklik = aktifArac.transform.position.y;

        if (hiz > 0.1f)
            batarya -= bataryaTuketim * Time.deltaTime;
        batarya = Mathf.Clamp(batarya, 0, 100);

        if (hizBar != null) hizBar.value = Mathf.Clamp01(hiz / (maksHiz * 3.6f));
        if (bataryaBar != null) bataryaBar.value = batarya / 100f;
        if (yukseklikDeger != null) yukseklikDeger.text = $"{yukseklik:F1} m";
        if (hizDeger != null) hizDeger.text = $"{hiz:F0} km/s";
        if (bataryaDeger != null) bataryaDeger.text = $"%{batarya:F0}";
    }
}
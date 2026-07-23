using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AnaMenuManager : MonoBehaviour
{
    public Button ihaButon;
    public Button karaAraciButon;
    public Button baslatButon;
    public Button haritaAraziButon;
    public Button haritaOrmanButon;

    string secilenArac = "";
    string secilenHarita = "HaritaArazi";

    void Start()
    {
        ihaButon.onClick.AddListener(() => AracSec("IHA"));
        karaAraciButon.onClick.AddListener(() => AracSec("KaraAraci"));
        haritaAraziButon.onClick.AddListener(() => HaritaSec("HaritaArazi"));
        haritaOrmanButon.onClick.AddListener(() => HaritaSec("HaritaOrman"));
        baslatButon.onClick.AddListener(Baslat);
    }

    void AracSec(string arac)
    {
        secilenArac = arac;
        PlayerPrefs.SetString("SecilenArac", arac);
    }

    void HaritaSec(string harita)
    {
        secilenHarita = harita;
        PlayerPrefs.SetString("SecilenHarita", harita);
    }

    void Baslat()
    {
        if (secilenArac == "")
        {
            Debug.Log("Önce bir araç seçin!");
            return;
        }
        SceneManager.LoadScene(secilenHarita);
    }
}
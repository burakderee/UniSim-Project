using UnityEngine;
using System.IO;

public class VeriKaydedici : MonoBehaviour
{
    StreamWriter dosya;
    public GameObject iha;
    public GameObject karaAraci;
    public GameObject suAraci;
    bool kaydediyor = false;

    void Start()
    {
        string dosyaYolu = Application.dataPath + "/simulasyon_verisi.csv";
        Debug.Log("Dosya yolu: " + dosyaYolu);
        dosya = new StreamWriter(dosyaYolu);
        dosya.WriteLine("zaman,arac,x,y,z,hiz");
        kaydediyor = true;
    }

    void FixedUpdate()
    {
        if (!kaydediyor) return;

        string secilenArac = PlayerPrefs.GetString("SecilenArac", "IHA");
        GameObject aktifArac = secilenArac == "IHA" ? iha :
                               secilenArac == "KaraAraci" ? karaAraci : suAraci;

        if (aktifArac == null) return;

        Rigidbody rb = aktifArac.GetComponent<Rigidbody>();
        if (rb == null) return;

        Vector3 pos = aktifArac.transform.position;
        float hiz = rb.velocity.magnitude * 3.6f;

        dosya.WriteLine($"{Time.time:F2},{secilenArac},{pos.x:F2},{pos.y:F2},{pos.z:F2},{hiz:F2}");
    }

    void OnApplicationQuit()
    {
        if (dosya != null)
        {
            dosya.Close();
            Debug.Log("Veri kaydedildi: " + Application.dataPath + "/simulasyon_verisi.csv");
        }
    }
}
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ModelYuklemeUI : MonoBehaviour
{
    public GameObject modelPanel;
    public TMP_InputField dosyaYoluInput;
    public TextMeshProUGUI hataText;
    public Button yukleButon;

    void Start()
    {
        modelPanel.SetActive(false);
        hataText.text = "";
        yukleButon.onClick.AddListener(ModelYukle);
    }

    void ModelYukle()
    {
        string yol = dosyaYoluInput.text.Trim();

        if (string.IsNullOrEmpty(yol))
        {
            hataText.text = "Hata: Dosya yolu boþ olamaz!";
            return;
        }

        Mesh mesh = OBJYukle.OBJOku(yol);

        if (mesh == null)
        {
            hataText.text = "Hata: Dosya bulunamadý veya geçersiz format!";
            return;
        }

        string secilenArac = PlayerPrefs.GetString("SecilenArac", "IHA");
        GameObject arac = secilenArac == "IHA" ? GameObject.Find("Cube") :
                          secilenArac == "KaraAraci" ? GameObject.Find("KaraAraci") :
                          GameObject.Find("SuAraci");

        if (arac == null)
        {
            hataText.text = "Hata: Aktif araç bulunamadý!";
            return;
        }

        // --- 1. GÖRÜNÜMÜ GÜNCELLE ---
        MeshFilter mf = arac.GetComponent<MeshFilter>();
        if (mf != null) mf.mesh = mesh;

        // --- 2. ÇARPIÞMA AÐINI (FÝZÝÐÝ) GÜNCELLE ---
        // Yerin altýna düþmeyi engelleyen zýrhlý fizik güncellemesi
        Collider eskiCollider = arac.GetComponent<Collider>();

        // Eðer araçta MeshCollider harici eski tip (Box/Sphere) bir collider varsa sil
        if (eskiCollider != null && !(eskiCollider is MeshCollider))
        {
            Destroy(eskiCollider);
        }

        // Yeni modele tam oturan bir MeshCollider ekle veya var olaný al
        MeshCollider mc = arac.GetComponent<MeshCollider>();
        if (mc == null)
        {
            mc = arac.gameObject.AddComponent<MeshCollider>();
        }

        mc.sharedMesh = mesh;
        mc.convex = true; // Rigidbody'nin Terrain üzerinde çalýþabilmesi için ZORUNLU ayar

        // --- 3. BOYUTLANDIRMA ---
        Bounds bounds = mesh.bounds;
        float maxBoyut = Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z);
        if (maxBoyut > 0.001f)
        {
            float hedefBoyut = 8f; // Hedef boyut 3 birim
            float olcek = hedefBoyut / maxBoyut;
            arac.transform.localScale = new Vector3(olcek, olcek, olcek);
        }

        // --- 4. FÝZÝK MOTORUNU SIFIRLA VE SAKÝNLEÞTÝR ---
        // Aracýn takla atmasýný ve fýrlamasýný engelleyen bölüm
        Rigidbody rb = arac.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.ResetCenterOfMass();
        }

        // --- 5. TERRAIN ÜSTÜNE GÜVENLÝ YERLEÞTÝRME ---
        Terrain terrain = Terrain.activeTerrain;
        if (terrain != null)
        {
            float yukseklik = terrain.SampleHeight(arac.transform.position);
            arac.transform.position = new Vector3(
                arac.transform.position.x,
                yukseklik + 5f, // Aracý güvenli bir þekilde arazinin biraz üstünden býrak
                arac.transform.position.z
            );
        }

        hataText.text = "Model baþarýyla yüklendi!";
        modelPanel.SetActive(false);
    }
}
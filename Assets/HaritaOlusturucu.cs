using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Terrain))]
public class HaritaOlusturucu : MonoBehaviour
{
    private Terrain terrain;
    private TerrainData td;

    public enum HaritaTipi { Arazi, Orman, Su }

    [Header("Genel Harita Ayarlarý")]
    public HaritaTipi haritaTipi = HaritaTipi.Arazi;
    public int tohum = 42;

    [Header("Aðaç Ayarlarý")]
    [Tooltip("Haritadaki büyük aðaçlarýn sayýsý")]
    public int agacSayisi = 500;
    public float minAgacBoyutu = 0.8f;
    public float maxAgacBoyutu = 1.3f;
    [Tooltip("Terrain'deki ilk kaç modelin AÐAÇ olduðunu belirtin. (Örn: Ýlk 2 model aðaç, sonrakiler bitkiyse buraya 2 yazýn)")]
    public int agacModeliSayisi = 1;

    [Header("Aðaç Çarpýþma (Fizik) Ayarlarý")]
    [Tooltip("Görünmez gövde kalkanýnýn kalýnlýðý (aracýn aðaca ne kadar yaklaþabileceðini belirler)")]
    public float agacGovdeKalinligi = 0.5f;

    [Header("Bitki Ayarlarý")]
    [Tooltip("Haritadaki küçük çalý/ot gibi bitkilerin sayýsý")]
    public int bitkiSayisi = 4000;

    [Header("Çevre Objeleri")]
    public GameObject suObjesi;

    void Start()
    {
        terrain = GetComponent<Terrain>();
        td = terrain.terrainData;

        Random.InitState(tohum);

        switch (haritaTipi)
        {
            case HaritaTipi.Arazi: AraziOlustur(); break;
            case HaritaTipi.Orman: OrmanOlustur(); break;
            case HaritaTipi.Su: SuOlustur(); break;
        }

        bool agacEklenebilirMi = (haritaTipi == HaritaTipi.Orman || haritaTipi == HaritaTipi.Arazi);

        if (agacEklenebilirMi)
        {
            if (td.treePrototypes.Length > 0)
                AgaclariDagit();
            else
                Debug.LogWarning("Terrain'de hiç Tree Prototype yok!");
        }
    }

    void AraziOlustur()
    {
        int coz = td.heightmapResolution;
        float[,] y = new float[coz, coz];

        for (int z = 0; z < coz; z++)
            for (int x = 0; x < coz; x++)
            {
                float nx = (float)x / coz;
                float nz = (float)z / coz;
                float h = 0f;
                h += Mathf.PerlinNoise(nx * 1.5f + tohum, nz * 1.5f + tohum) * 0.2f;
                h += Mathf.PerlinNoise(nx * 3f + tohum, nz * 3f + tohum) * 0.1f;
                h += Mathf.PerlinNoise(nx * 6f + tohum, nz * 6f + tohum) * 0.05f;
                y[z, x] = h;
            }
        td.SetHeights(0, 0, y);
    }

    void OrmanOlustur()
    {
        int coz = td.heightmapResolution;
        float[,] y = new float[coz, coz];

        for (int z = 0; z < coz; z++)
            for (int x = 0; x < coz; x++)
            {
                float nx = (float)x / coz;
                float nz = (float)z / coz;
                float h = 0f;
                h += Mathf.PerlinNoise(nx * 2f + tohum, nz * 2f + tohum) * 0.35f;
                h += Mathf.PerlinNoise(nx * 5f + tohum, nz * 5f + tohum) * 0.2f;
                h += Mathf.PerlinNoise(nx * 10f + tohum, nz * 10f + tohum) * 0.1f;
                y[z, x] = h;
            }
        td.SetHeights(0, 0, y);
    }

    void SuOlustur()
    {
        int coz = td.heightmapResolution;
        float[,] y = new float[coz, coz];

        for (int z = 0; z < coz; z++)
            for (int x = 0; x < coz; x++)
            {
                float nx = (float)x / coz;
                float nz = (float)z / coz;
                float kenarMesafe = Mathf.Min(nx, 1f - nx, nz, 1f - nz);
                float h = 0f;
                h += (1f - Mathf.Clamp01(kenarMesafe * 5f)) * 0.4f;
                h += Mathf.PerlinNoise(nx * 3f + tohum, nz * 3f + tohum) * 0.05f;
                y[z, x] = h;
            }
        td.SetHeights(0, 0, y);

        if (suObjesi != null)
        {
            Vector3 terrainMerkezi = terrain.transform.position;
            float xMerkez = terrainMerkezi.x + (td.size.x / 2f);
            float zMerkez = terrainMerkezi.z + (td.size.z / 2f);
            suObjesi.transform.position = new Vector3(xMerkez, 8f, zMerkez);
            suObjesi.transform.localScale = new Vector3(td.size.x / 10f, 1f, td.size.z / 10f);
        }
    }

    void AgaclariDagit()
    {
        td.treeInstances = new TreeInstance[0];
        List<TreeInstance> objeler = new List<TreeInstance>();

        // --- AÐAÇLAR ÝÇÝN KATI KURALLAR ---
        float agacMinY = (haritaTipi == HaritaTipi.Orman) ? 0.05f : 0.03f;
        float agacMaxY = (haritaTipi == HaritaTipi.Orman) ? 0.5f : 0.25f;
        float agacMaxEgim = (haritaTipi == HaritaTipi.Orman) ? 30f : 15f;

        // --- BÝTKÝLER ÝÇÝN GENÝÞLETÝLMÝÞ KURALLAR ---
        float bitkiMinY = (haritaTipi == HaritaTipi.Orman) ? 0.02f : 0.015f;
        float bitkiMaxY = (haritaTipi == HaritaTipi.Orman) ? 0.7f : 0.4f;
        float bitkiMaxEgim = (haritaTipi == HaritaTipi.Orman) ? 45f : 25f;

        int toplamPrototipler = td.treePrototypes.Length;
        int guvenliAgacModeliSayisi = Mathf.Min(agacModeliSayisi, toplamPrototipler);

        // --- 1. AÞAMA: SADECE AÐAÇLARI DAÐIT ---
        int deneme = 0;
        int uretilenAgacSayisi = 0;

        while (uretilenAgacSayisi < agacSayisi && deneme < agacSayisi * 15)
        {
            deneme++;
            float x = Random.Range(0.05f, 0.95f);
            float z = Random.Range(0.05f, 0.95f);

            float normalizeY = td.GetInterpolatedHeight(x, z) / td.size.y;
            float egim = td.GetSteepness(x, z);

            if (normalizeY >= agacMinY && normalizeY <= agacMaxY && egim <= agacMaxEgim)
            {
                TreeInstance agac = new TreeInstance
                {
                    position = new Vector3(x, normalizeY, z),
                    prototypeIndex = Random.Range(0, guvenliAgacModeliSayisi),
                    widthScale = Random.Range(minAgacBoyutu, maxAgacBoyutu),
                    heightScale = Random.Range(minAgacBoyutu, maxAgacBoyutu),
                    color = Color.white,
                    lightmapColor = Color.white
                };
                objeler.Add(agac);
                uretilenAgacSayisi++;
            }
        }

        // --- 2. AÞAMA: SADECE BÝTKÝLERÝ DAÐIT ---
        int bitkiDeneme = 0;
        int uretilenBitkiSayisi = 0;

        if (guvenliAgacModeliSayisi < toplamPrototipler)
        {
            while (uretilenBitkiSayisi < bitkiSayisi && bitkiDeneme < bitkiSayisi * 10)
            {
                bitkiDeneme++;
                float x = Random.Range(0.02f, 0.98f);
                float z = Random.Range(0.02f, 0.98f);

                float normalizeY = td.GetInterpolatedHeight(x, z) / td.size.y;
                float egim = td.GetSteepness(x, z);

                if (normalizeY >= bitkiMinY && normalizeY <= bitkiMaxY && egim <= bitkiMaxEgim)
                {
                    TreeInstance bitki = new TreeInstance
                    {
                        position = new Vector3(x, normalizeY, z),
                        prototypeIndex = Random.Range(guvenliAgacModeliSayisi, toplamPrototipler),
                        widthScale = Random.Range(0.5f, 1.0f),
                        heightScale = Random.Range(0.5f, 1.0f),
                        color = Color.white,
                        lightmapColor = Color.white
                    };
                    objeler.Add(bitki);
                    uretilenBitkiSayisi++;
                }
            }
        }
        else
        {
            Debug.LogWarning("Terrain'e hiç bitki modeli eklenmemiþ. Lütfen Terrain -> Paint Trees kýsmýna bitki/çalý ekleyip 'Agac Modeli Sayisi'ni doðru ayarlayýn.");
        }

        td.treeInstances = objeler.ToArray();
        terrain.Flush();

        if (uretilenAgacSayisi < agacSayisi || uretilenBitkiSayisi < bitkiSayisi)
        {
            Debug.Log($"Arazi þartlarý nedeniyle; {agacSayisi} aðaçtan {uretilenAgacSayisi} kadarý, {bitkiSayisi} bitkiden {uretilenBitkiSayisi} kadarý yerleþtirilebildi.");
        }

        // --- 3. AÞAMA: FÝZÝKSEL KALKANLARI ÜRET ---
        AgaclaraFizikEkle();
    }

    void AgaclaraFizikEkle()
    {
        // Kodu art arda çalýþtýrýrsan eski kalkanlar üst üste binmesin diye temizliyoruz
        Transform eskiKalkanlar = transform.Find("GörünmezAgacKalkanlari");
        if (eskiKalkanlar != null)
        {
            Destroy(eskiKalkanlar.gameObject);
        }

        // Tüm kalkanlarý içinde tutacak boþ bir klasör obje oluþtur (Hierarchy temiz kalsýn)
        GameObject kalkanMerkezi = new GameObject("GörünmezAgacKalkanlari");
        kalkanMerkezi.transform.SetParent(this.transform);

        foreach (TreeInstance agac in td.treeInstances)
        {
            // Sadece aðaç olanlara (bitkilere deðil) kalkan ekle
            if (agac.prototypeIndex < agacModeliSayisi)
            {
                // Aðacýn dünyadaki gerçek koordinatýný hesapla
                Vector3 gercekPozisyon = Vector3.Scale(agac.position, td.size) + terrain.transform.position;

                // Görünmez bir fiziksel obje oluþtur
                GameObject kalkan = new GameObject("AgacGovdesi");
                kalkan.transform.position = gercekPozisyon;
                kalkan.transform.SetParent(kalkanMerkezi.transform);

                // Oyunu kastýrmamak için objeyi statik (hareketsiz) yap
                kalkan.isStatic = true;

                // Aðaca kapsül þeklinde bir çarpýþma kutusu ekle
                CapsuleCollider col = kalkan.AddComponent<CapsuleCollider>();
                col.radius = agacGovdeKalinligi;
                col.height = 10f; // Araba üstünden atlamasýn diye uzun bir kalkan
                col.center = new Vector3(0, 5f, 0); // Kalkanýn yarýsý topraðýn üstünde dursun
            }
        }
    }
}
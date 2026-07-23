using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Hedef ve Hareket Ayarları")]
    public Transform target;
    public float uzaklik = 10f;
    public float fareHassasiyeti = 3f;
    public float smoothSpeed = 5f;

    [Header("Zemin Kontrolü (Çarpışma Önleyici)")]
    [Tooltip("Kameranın arazinin en az kaç birim üstünde durmasını istiyorsun?")]
    public float minimumYerdenYukseklik = 1.5f;

    float yatayAci = 0f;
    float dikeyAci = 20f;

    void LateUpdate()
    {
        if (target == null) return;

        // Sağ tık basılı tutunca döner
        if (Input.GetMouseButton(1))
        {
            yatayAci += Input.GetAxis("Mouse X") * fareHassasiyeti;
            dikeyAci -= Input.GetAxis("Mouse Y") * fareHassasiyeti;
            dikeyAci = Mathf.Clamp(dikeyAci, -10f, 80f);
        }

        // Scroll ile zoom
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        uzaklik -= scroll * 5f;
        uzaklik = Mathf.Clamp(uzaklik, 3f, 50f);

        // Kameranın gitmesi gereken ideal pozisyonu hesapla
        Quaternion donus = Quaternion.Euler(dikeyAci, yatayAci, 0);
        Vector3 hedefPos = target.position + donus * new Vector3(0, 0, -uzaklik);

        // --- ZEMİN (TERRAIN) KONTROLÜ BAŞLANGICI ---
        if (Terrain.activeTerrain != null)
        {
            // Kameranın gitmek istediği X ve Z koordinatlarındaki arazinin tam yüksekliğini ölç
            float araziYuksekligi = Terrain.activeTerrain.SampleHeight(hedefPos) + Terrain.activeTerrain.transform.position.y;

            // Eğer hedeflenen nokta arazinin altında veya belirlediğimiz sınırın altındaysa, kamerayı yukarı it
            if (hedefPos.y < araziYuksekligi + minimumYerdenYukseklik)
            {
                hedefPos.y = araziYuksekligi + minimumYerdenYukseklik;
            }
        }
        // --- ZEMİN KONTROLÜ BİTİŞİ ---

        // Kamerayı yumuşak bir şekilde yeni, güvenli pozisyona taşı
        transform.position = Vector3.Lerp(transform.position, hedefPos, smoothSpeed * Time.deltaTime);

        // Kameranın her zaman hedefe bakmasını sağla
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}
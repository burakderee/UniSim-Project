/*using UnityEngine;

public class SimulasyonManager : MonoBehaviour
{
    public GameObject iha;
    public GameObject karaAraci;
    public GameObject suAraci;
    public CameraFollow kameraFollow;

    void Start()
    {
        string secilenArac = PlayerPrefs.GetString("SecilenArac", "IHA");

        iha.SetActive(false);
        karaAraci.SetActive(false);
        suAraci.SetActive(false);

        if (secilenArac == "IHA")
        {
            iha.SetActive(true);
            kameraFollow.target = iha.transform;
        }
        else if (secilenArac == "KaraAraci")
        {
            karaAraci.SetActive(true);
            kameraFollow.target = karaAraci.transform;
        }
        else
        {
            suAraci.SetActive(true);
            kameraFollow.target = suAraci.transform;
        }
    }
}*/

using UnityEngine;
using System.Collections;

public class SimulasyonManager : MonoBehaviour
{
    public GameObject iha;
    public GameObject karaAraci;
    public GameObject suAraci;
    public CameraFollow kameraFollow;

    void Start()
    {
        string secilenArac = PlayerPrefs.GetString("SecilenArac", "IHA");

        iha.SetActive(false);
        karaAraci.SetActive(false);
        suAraci.SetActive(false);

        if (secilenArac == "IHA")
        {
            iha.SetActive(true);
            kameraFollow.target = iha.transform;
            StartCoroutine(TerrainUstuneBirak(iha, new Vector3(0, 0, 0)));
        }
        else if (secilenArac == "KaraAraci")
        {
            karaAraci.SetActive(true);
            kameraFollow.target = karaAraci.transform;
            StartCoroutine(TerrainUstuneBirak(karaAraci, new Vector3(0, 0, 0)));
        }
        else
        {
            suAraci.SetActive(true);
            kameraFollow.target = suAraci.transform;
            StartCoroutine(TerrainUstuneBirak(suAraci, new Vector3(0, 0, 200)));
        }
    }

    IEnumerator TerrainUstuneBirak(GameObject arac, Vector3 hedef)
    {
        yield return new WaitForSeconds(0.3f);
        Terrain terrain = Terrain.activeTerrain;
        if (terrain != null)
        {
            float yukseklik = terrain.SampleHeight(new Vector3(hedef.x, 0, hedef.z));
            hedef.y = yukseklik + 5f;
        }
        arac.transform.position = hedef;
    }
}
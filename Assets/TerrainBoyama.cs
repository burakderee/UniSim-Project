using UnityEngine;

public class TerrainBoyama : MonoBehaviour
{
    Terrain terrain;
    TerrainData td;

    void Start()
    {
        terrain = GetComponent<Terrain>();
        td = terrain.terrainData;
        OtomatikBoya();
    }

    void OtomatikBoya()
    {
        int genislik = td.alphamapWidth;
        int yukseklik = td.alphamapHeight;
        int katmanSayisi = td.alphamapLayers;

        float[,,] harita = new float[genislik, yukseklik, katmanSayisi];

        for (int y = 0; y < yukseklik; y++)
        {
            for (int x = 0; x < genislik; x++)
            {
                float normalX = (float)x / genislik;
                float normalY = (float)y / yukseklik;

                float yukseklikDegeri = td.GetHeight(
                    Mathf.RoundToInt(normalY * td.heightmapResolution),
                    Mathf.RoundToInt(normalX * td.heightmapResolution)
                ) / td.size.y;

                float egim = td.GetSteepness(normalX, normalY) / 90f;

                float cimen = 0f, toprak = 0f, kaya = 0f;

                if (egim > 0.4f)
                {
                    kaya = 1f;
                }
                else if (yukseklikDegeri > 0.4f)
                {
                    kaya = Mathf.InverseLerp(0.4f, 0.6f, yukseklikDegeri);
                    toprak = 1f - kaya;
                }
                else if (yukseklikDegeri > 0.2f)
                {
                    toprak = Mathf.InverseLerp(0.2f, 0.4f, yukseklikDegeri);
                    cimen = 1f - toprak;
                }
                else
                {
                    cimen = 1f;
                }

                if (katmanSayisi >= 3)
                {
                    harita[x, y, 0] = cimen;
                    harita[x, y, 1] = toprak;
                    harita[x, y, 2] = kaya;
                }
                else if (katmanSayisi == 2)
                {
                    harita[x, y, 0] = cimen;
                    harita[x, y, 1] = toprak;
                }
                else
                {
                    harita[x, y, 0] = 1f;
                }
            }
        }
        td.SetAlphamaps(0, 0, harita);
    }
}
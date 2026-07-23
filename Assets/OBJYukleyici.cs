using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class OBJYukle : MonoBehaviour
{
    public static Mesh OBJOku(string dosyaYolu)
    {
        if (!File.Exists(dosyaYolu)) return null;

        List<Vector3> vertices = new List<Vector3>();
        List<Vector2> uvler = new List<Vector2>();
        List<Vector3> normaller = new List<Vector3>();
        List<Vector3> finalVertices = new List<Vector3>();
        List<Vector2> finalUVler = new List<Vector2>();
        List<Vector3> finalNormaller = new List<Vector3>();
        List<int> triangles = new List<int>();

        string[] satirlar = File.ReadAllLines(dosyaYolu);

        foreach (string satir in satirlar)
        {
            string temizSatir = satir.Trim();
            if (string.IsNullOrEmpty(temizSatir)) continue;

            string[] parcalar = temizSatir.Split(new char[] { ' ' },
                System.StringSplitOptions.RemoveEmptyEntries);

            if (parcalar.Length == 0) continue;

            if (parcalar[0] == "v" && parcalar.Length >= 4)
            {
                vertices.Add(new Vector3(
                    float.Parse(parcalar[1], System.Globalization.CultureInfo.InvariantCulture),
                    float.Parse(parcalar[2], System.Globalization.CultureInfo.InvariantCulture),
                    float.Parse(parcalar[3], System.Globalization.CultureInfo.InvariantCulture)
                ));
            }
            else if (parcalar[0] == "vt" && parcalar.Length >= 3)
            {
                uvler.Add(new Vector2(
                    float.Parse(parcalar[1], System.Globalization.CultureInfo.InvariantCulture),
                    float.Parse(parcalar[2], System.Globalization.CultureInfo.InvariantCulture)
                ));
            }
            else if (parcalar[0] == "vn" && parcalar.Length >= 4)
            {
                normaller.Add(new Vector3(
                    float.Parse(parcalar[1], System.Globalization.CultureInfo.InvariantCulture),
                    float.Parse(parcalar[2], System.Globalization.CultureInfo.InvariantCulture),
                    float.Parse(parcalar[3], System.Globalization.CultureInfo.InvariantCulture)
                ));
            }
            else if (parcalar[0] == "f" && parcalar.Length >= 4)
            {
                // Quad veya üçgen destekle — fan triangulation
                int noktaSayisi = parcalar.Length - 1;
                for (int i = 1; i < noktaSayisi - 1; i++)
                {
                    EkleNokta(parcalar[1], vertices, uvler, normaller,
                        finalVertices, finalUVler, finalNormaller, triangles);
                    EkleNokta(parcalar[i + 1], vertices, uvler, normaller,
                        finalVertices, finalUVler, finalNormaller, triangles);
                    EkleNokta(parcalar[i + 2], vertices, uvler, normaller,
                        finalVertices, finalUVler, finalNormaller, triangles);
                }
            }
        }

        if (finalVertices.Count == 0) return null;

        Mesh mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.vertices = finalVertices.ToArray();
        if (finalUVler.Count == finalVertices.Count) mesh.uv = finalUVler.ToArray();
        if (finalNormaller.Count == finalVertices.Count) mesh.normals = finalNormaller.ToArray();
        mesh.triangles = triangles.ToArray();
        if (finalNormaller.Count == 0) mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        return mesh;
    }

    static void EkleNokta(string parca, List<Vector3> vertices, List<Vector2> uvler,
        List<Vector3> normaller, List<Vector3> finalV, List<Vector2> finalUV,
        List<Vector3> finalN, List<int> triangles)
    {
        string[] idx = parca.Split('/');
        int vIdx = int.Parse(idx[0]) - 1;
        if (vIdx < 0 || vIdx >= vertices.Count) return;
        finalV.Add(vertices[vIdx]);

        if (idx.Length > 1 && idx[1] != "" && uvler.Count > 0)
        {
            int uvIdx = int.Parse(idx[1]) - 1;
            if (uvIdx >= 0 && uvIdx < uvler.Count)
                finalUV.Add(uvler[uvIdx]);
        }

        if (idx.Length > 2 && idx[2] != "" && normaller.Count > 0)
        {
            int nIdx = int.Parse(idx[2]) - 1;
            if (nIdx >= 0 && nIdx < normaller.Count)
                finalN.Add(normaller[nIdx]);
        }

        triangles.Add(finalV.Count - 1);
    }
}
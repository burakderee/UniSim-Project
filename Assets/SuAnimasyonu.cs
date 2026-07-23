using UnityEngine;

public class SuAnimasyonu : MonoBehaviour
{
    public float dalgaHizi = 1f;
    public float dalgaYuksekligi = 0.05f;
    private Vector3 baslangicPozisyon;
    private Renderer suRenderer;

    void Start()
    {
        baslangicPozisyon = transform.position;
        suRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        float yeniY = baslangicPozisyon.y + Mathf.Sin(Time.time * dalgaHizi) * dalgaYuksekligi;
        transform.position = new Vector3(transform.position.x, yeniY, transform.position.z);
        suRenderer.material.mainTextureOffset += new Vector2(0.0005f, 0.0005f);
    }
}
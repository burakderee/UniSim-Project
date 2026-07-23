using UnityEngine;
using UnityEngine.UI;

public class ArkaplanAnimasyon : MonoBehaviour
{
    public RectTransform[] bulutlar;
    public float hiz = 20f;
    float ekranGenisligi;

    void Start()
    {
        ekranGenisligi = Screen.width;
    }

    void Update()
    {
        foreach (RectTransform bulut in bulutlar)
        {
            bulut.anchoredPosition += new Vector2(hiz * Time.deltaTime, 0);
            if (bulut.anchoredPosition.x > ekranGenisligi + 200)
                bulut.anchoredPosition = new Vector2(-200, bulut.anchoredPosition.y);
        }
    }
}
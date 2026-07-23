using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class PusulaSecici : MonoBehaviour, IPointerClickHandler
{
    public HavaDurumu havaDurumu;
    public TextMeshProUGUI yonText;
    public RectTransform ibre;
    public TMP_InputField yonInput;
    RectTransform rect;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        if (yonInput != null)
            yonInput.onEndEdit.AddListener(ManuelYonAyarla);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rect, eventData.position, eventData.pressEventCamera, out localPos);

        float aci = Mathf.Atan2(localPos.x, localPos.y) * Mathf.Rad2Deg;
        if (aci < 0) aci += 360f;

        havaDurumu.ruzgarYonu = aci;

        if (ibre != null)
            ibre.localRotation = Quaternion.Euler(0, 0, -aci);

        string yonAdi = AcidenYon(aci);
        if (yonText != null)
            yonText.text = $"Yön: {yonAdi} ({aci:F0}°)";
    }

    void ManuelYonAyarla(string deger)
    {
        float aci;
        if (float.TryParse(deger, out aci))
        {
            aci = Mathf.Clamp(aci, 0f, 360f);
            havaDurumu.ruzgarYonu = aci;
            if (ibre != null)
                ibre.localRotation = Quaternion.Euler(0, 0, -aci);
            if (yonText != null)
                yonText.text = $"Yön: {AcidenYon(aci)} ({aci:F0}°)";
        }
    }

    string AcidenYon(float aci)
    {
        if (aci < 22.5f || aci >= 337.5f) return "Kuzey";
        if (aci < 67.5f) return "Kuzeydoðu";
        if (aci < 112.5f) return "Doðu";
        if (aci < 157.5f) return "Güneydoðu";
        if (aci < 202.5f) return "Güney";
        if (aci < 247.5f) return "Güneybatý";
        if (aci < 292.5f) return "Batý";
        return "Kuzeybatý";
    }
}
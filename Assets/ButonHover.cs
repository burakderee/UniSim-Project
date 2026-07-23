using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Image butonImage;
    Color normalRenk;
    public Color hoverRenk = new Color(0.3f, 0.6f, 1f, 1f);
    public float gecisHizi = 5f;
    bool uzerinde = false;

    void Start()
    {
        butonImage = GetComponent<Image>();
        normalRenk = butonImage.color;
    }

    void Update()
    {
        Color hedef = uzerinde ? hoverRenk : normalRenk;
        butonImage.color = Color.Lerp(butonImage.color, hedef, gecisHizi * Time.deltaTime);
    }

    public void OnPointerEnter(PointerEventData e) => uzerinde = true;
    public void OnPointerExit(PointerEventData e) => uzerinde = false;
}
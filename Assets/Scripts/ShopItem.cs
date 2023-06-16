using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Image sprite;
    [SerializeField] private float price;
    [SerializeField] private Image slot;
    [SerializeField] private Text priceText;

    public void OnPointerDown(PointerEventData eventData)
    {
        slot.sprite = sprite.sprite;
        priceText.text = price.ToString();
    }
}

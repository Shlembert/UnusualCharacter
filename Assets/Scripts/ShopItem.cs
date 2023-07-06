using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Button buyButton;
    [SerializeField] private Text buyButtonText;
    [SerializeField] private Image sprite;
    [SerializeField] private int price;
    [SerializeField] private Image slot;
    [SerializeField] private Text priceText;
    [SerializeField] private int index;
    [SerializeField] private Text coinText;

    private void Start()
    {
        if(PlayerPrefs.GetInt("CurrentPose", 0) == index)
        {
            Select();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Select();
    }

    public void Select()
    {
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(BuyButtonPressed);
        slot.sprite = sprite.sprite;
        int isBuyed = PlayerPrefs.GetInt("Pose:" + index, 0);
        if (isBuyed == 1)
        {
            priceText.fontSize = 44;
            if (SettingsController.instance.En)
            {
                priceText.text = "Bought";
                buyButtonText.text = "Set";
            }
            else
            {
                priceText.text = "Куплено";
                buyButtonText.text = "Выбрать";
            }
        }
        else
        {
            priceText.fontSize = 88;
            priceText.text = price.ToString();
            if (SettingsController.instance.En)
            {
                buyButtonText.text = "Buy";
            }
            else
            {
                buyButtonText.text = "Купить";
            }
        }
    }

    private void UpdateText()
    {
        priceText.fontSize = 44;
        if (SettingsController.instance.En)
        {
            priceText.text = "Bought";
            buyButtonText.text = "Set";
        }
        else
        {
            priceText.text = "Куплено";
            buyButtonText.text = "Выбрать";
        }
    }

    public void BuyButtonPressed()
    {
        int isBuyed = PlayerPrefs.GetInt("Pose:" + index, 0);
        if (isBuyed == 0) {
            int coins = PlayerPrefs.GetInt("Coins", 0);
            if (coins >= price)
            {
                coins -= price;
                coinText.text = coins.ToString();
                PlayerPrefs.SetInt("Coins", coins);
                PlayerPrefs.SetInt("Pose:" + index, 1);
                PlayerPrefs.SetInt("CurrentPose", index);
                PoseChenger.instance.SetPose(index);
                PlayerPrefs.Save();
                UpdateText();
            }
        }
        else
        {
            PlayerPrefs.SetInt("CurrentPose", index);
            PoseChenger.instance.SetPose(index);
            PlayerPrefs.Save();
        }
    }
}

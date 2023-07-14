using FMODUnity;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private string PlayerPrefsKey;
    [SerializeField] private Button buyButton;
    [SerializeField] private Text buyButtonText;
    [SerializeField] private Sprite sprite;
    [SerializeField] private int price;
    [SerializeField] private Image slot;
    [SerializeField] private Text priceText;
    [SerializeField] private int index;
    [SerializeField] private Text coinText;
    [SerializeField] private GameObject itemChengerGO;
    [SerializeField] private StudioEventEmitter studioEventEmitter;
    private IItemChenger itemChenger;
    private bool en;


    private void Start()
    {
        Lang.instance.LangChenge.AddListener(ChengeLang);
        itemChenger = itemChengerGO.GetComponent<IItemChenger>();
        if (PlayerPrefs.GetInt("Current " + PlayerPrefsKey, 0) == index)
        {
            Select();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        studioEventEmitter.Play();
        Select();
    }

    public void Select()
    {
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(BuyButtonPressed);
        slot.sprite = sprite;
        int isBuyed = PlayerPrefs.GetInt(PlayerPrefsKey + index, index == 0 ? 1 : 0);
        UpdateText(isBuyed == 1);
        
    }

    public void ChengeLang(bool en)
    {
        this.en = en;
        if (PlayerPrefs.GetInt("Current " + PlayerPrefsKey, 0) == index)
        {
            UpdateText(true);
        }
    }
    private void UpdateText(bool isBuyed)
    {
        if (isBuyed)
        {
            priceText.fontSize = 66;
            if (en)
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
            if (en)
            {
                buyButtonText.text = "Buy";
            }
            else
            {
                buyButtonText.text = "Купить";
            }
        }
    }

    public void BuyButtonPressed()
    {
        int isBuyed = PlayerPrefs.GetInt(PlayerPrefsKey + index, 0);
        if (isBuyed == 0) {
            int coins = PlayerPrefs.GetInt("Coins", 0);
            if (coins >= price)
            {
                coins -= price;
                coinText.text = coins.ToString();
                PlayerPrefs.SetInt("Coins", coins);
                PlayerPrefs.SetInt(PlayerPrefsKey + index, 1);
                PlayerPrefs.SetInt("Current " + PlayerPrefsKey, index);
                itemChenger.SetItem(index);
                PlayerPrefs.Save();
                UpdateText(true);
            }
        }
        else
        {
            PlayerPrefs.SetInt("Current " + PlayerPrefsKey, index);
            itemChenger.SetItem(index);
            PlayerPrefs.Save();
        }
    }
}

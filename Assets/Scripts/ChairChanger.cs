using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairChanger : MonoBehaviour, IItemChenger
{
    [SerializeField] private List<GameObject> Items;
    public void SetItem(int itemIndex)
    {
        foreach (var item in Items)
        {
            item.SetActive(false);
        }
        Items[itemIndex].SetActive(true);

        PlayerMovement.instance.CrossSprite = Items[itemIndex].transform.GetChild(0).GetComponent<SpriteRenderer>();
        PlayerMovement.instance.SeatSprite = Items[itemIndex].transform.GetChild(1).GetComponent<SpriteRenderer>();
        PlayerMovement.instance.BackrestSprite = Items[itemIndex].transform.GetChild(2).GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        SetItem(PlayerPrefs.GetInt("Current Chair", 0));
    }
}

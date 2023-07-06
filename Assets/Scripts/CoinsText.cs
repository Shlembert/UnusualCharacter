using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class CoinsText : MonoBehaviour
{
    private Text text;
    void Start()
    {
        int coins = PlayerPrefs.GetInt("Coins", 0);
        text = GetComponent<Text>();
        text.text = coins.ToString();
    }
}

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Lang : MonoBehaviour
{
    public static Lang instance;

    [SerializeField] private Text start, score, setting, music, sound, sence, credits, exSet,
        buy, exShop, pause, resum, restart, setBtn, wake, exPaus;

    private void Awake()
    {
        instance = this;
    }

    public void SetEn()
    {
        start.text = "START";
        score.text = "SCORE: ";
        setting.text = setBtn.text = "SETTING";
        music.text = "MUSIC";
        sound.text = "SOUND";
        sence.text = "SENCE";
        credits.text = "CREDITS";
        exPaus.text = exSet.text = exShop.text = "EXIT";
        buy.text = "BUY";
        pause.text = "PAUSE";
        resum.text = "RESUME";
        restart.text = "RESTART";
        wake.text = "WAKE UP...";
    }

    public void SetRu()
    {
        start.text = "—“¿–“";
        score.text = "Œ◊ »: ";
        setting.text = setBtn.text = "Õ¿—“–Œ… »";
        music.text = "Ã”«€ ¿";
        sound.text = "«¬” ";
        sence.text = "Œ“ À» ";
        credits.text = "“»“–€";
        exPaus.text = exSet.text = exShop.text = "¬€’Œƒ";
        buy.text = " ”œ»“‹";
        pause.text = "œ¿”«¿";
        resum.text = "œ–ŒƒŒÀ∆»“‹";
        restart.text = "–≈—“¿–“";
        wake.text = "œ–Œ—Õ”“‹—ﬂ";
    }
}
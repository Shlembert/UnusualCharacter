using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class SettingsController : MonoBehaviour
{
    public static SettingsController instance;

    [SerializeField] private GameObject titleText, shopWindow, settingWindow, pauseWindow, startBtn, player, joy, enTxt, ruTxt;
    [SerializeField] private List<SpawnerObstacle> spawnerObstacles;
    [SerializeField] private BGScroller bGScroller;

    public bool play;

    private float titleOrigPos;
    private float startOrigPos;
    private float speedBG;
    private bool pressSetting = false;
    private bool pressShop = false;
    private bool pause = false;
    private bool en = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        titleOrigPos = titleText.transform.position.y;
        startOrigPos = startBtn.transform.position.y;
        LoadLanguage();
    }

    private void SetStartLang()
    {
        if (en)
        {
            Lang.instance.SetEn();
            ruTxt.SetActive(false);
            enTxt.SetActive(true);
        }
        else
        {
            Lang.instance.SetRu();
            ruTxt.SetActive(true);
            enTxt.SetActive(false);
        }
    }

    private void SaveLanguage()
    {
        PlayerPrefs.SetInt("Language", en ? 1 : 0);
    }

    private void LoadLanguage()
    {
        int languageIndex = PlayerPrefs.GetInt("Language", 0);
        en = (languageIndex == 1);
        SetStartLang();
    }

    public void PressPause()
    {
        if (!pause)
        {
            pause = true;

            speedBG = bGScroller.speed;
            bGScroller.speed = 0;
            MoodController.instance.decay = false;

            foreach (var item in spawnerObstacles) item.Pause();
           
            player.transform.position = new Vector2(10, 0);
            joy.SetActive(false);
            pauseWindow.SetActive(true);
            pauseWindow.transform.DOMoveY(0, 0.5f, false).SetEase(Ease.OutBack);
        }
        else
        {
            if (pressSetting) ExitSettings();
            else ExitPause();
        }
    }

    public void PressSetting()
    {
        Volume.instance.Send();

        if (play)
        {
            pressSetting = true;

            pauseWindow.transform.DOMoveY(10, 0.5f, false).SetEase(Ease.InBack).OnComplete(() =>
            {
                settingWindow.transform.DOMoveY(0, 0.5f, false).SetEase(Ease.OutBack);
            });
        }
        else
        {
            if (!pressSetting)
            {
                pressSetting = true;
                pressShop = true;

                titleText.transform.DOMoveY(10, 0.5f, false).SetEase(Ease.InBack).OnComplete(() =>
                {
                    startBtn.transform.DOMoveY(-10, 0.5f, false).SetEase(Ease.InBack).OnComplete(() =>
                    {
                        settingWindow.transform.DOMoveY(0, 0.5f, false).SetEase(Ease.OutBack);
                    });
                });
            }
        }
    }

    public void PressShop()
    {
        if (!pressShop)
        {
            pressSetting = true;
            pressShop = true;

            titleText.transform.DOMoveY(10, 0.5f, false).SetEase(Ease.InBack).OnComplete(() =>
            {
                startBtn.transform.DOMoveY(-10, 0.5f, false).SetEase(Ease.InBack).OnComplete(() =>
                {
                    shopWindow.SetActive(true);
                    shopWindow.transform.DOMoveY(0, 0.5f, false).SetEase(Ease.OutBack);
                });
            });
        }
    }

    public void ExitPause()
    {
        pauseWindow.transform.DOMoveY(10, 0.5f, false).SetEase(Ease.InBack).OnComplete(() =>
        {
            pause = false;

            bGScroller.speed = speedBG;
            MoodController.instance.decay = true;

            player.transform.position = Vector2.zero;

            joy.SetActive(true);

            foreach (var item in spawnerObstacles)
            {
                item.PauseExit();
            }

            UIController.instance.NoPause();
        });
    }

    public void ExitSettings()
    {
        SaveLanguage();

        if (play)
        {
            pressSetting = false;

            settingWindow.transform.DOMoveY(-10, 0.5f, false).SetEase(Ease.InBack).OnComplete(() =>
            {
                pauseWindow.transform.DOMoveY(0, 0.5f, false).SetEase(Ease.OutBack);
            });
        }
        else
        {
            settingWindow.transform.DOMoveY(-10, 0.5f, false).SetEase(Ease.InBack).OnComplete(() =>
            {
                titleText.transform.DOMoveY(titleOrigPos, 0.5f, false).SetEase(Ease.OutBack).OnComplete(() =>
                {
                    startBtn.transform.DOMoveY(startOrigPos, 0.5f, false).SetEase(Ease.OutBack);
                    pressSetting = false;
                    pressShop = false;
                });
            });
        }

        Volume.instance.Send();
    }

    public void ExitShop()
    {
        shopWindow.transform.DOMoveY(-10, 0.5f, false).SetEase(Ease.InBack).OnComplete(() =>
        {
            titleText.transform.DOMoveY(titleOrigPos, 0.5f, false).SetEase(Ease.OutBack).OnComplete(() =>
            {
                startBtn.transform.DOMoveY(startOrigPos, 0.5f, false).SetEase(Ease.OutBack);
                pressSetting = false;
                pressShop = false;
            });
        });
    }

    public void ChangeLangLeft()
    {
        if (en)
        {
            en = false;
            ruTxt.SetActive(false);
            enTxt.SetActive(true);

            enTxt.transform.DOMoveX(-3, 0.5f, false).SetEase(Ease.InBack).OnComplete(() =>
            {
                ruTxt.SetActive(true);
                enTxt.SetActive(false);
                enTxt.transform.DOMoveX(0, 0, false);
                ruTxt.transform.DOMoveX(3, 0.5f, false).SetEase(Ease.OutBack).From().OnComplete(() =>
                {
                    Lang.instance.SetRu();
                });
            });
        }
        else
        {
            en = true;
            ruTxt.SetActive(true);
            enTxt.SetActive(false);

            ruTxt.transform.DOMoveX(-3, 0.5f, false).SetEase(Ease.InBack).OnComplete(() =>
            {
                enTxt.SetActive(true);
                ruTxt.SetActive(false);
                ruTxt.transform.DOMoveX(0, 0, false);
                enTxt.transform.DOMoveX(3, 0.5f, false).SetEase(Ease.OutBack).From().OnComplete(() =>
                {
                    Lang.instance.SetEn();
                });
            });
        }
    }

    public void ChangeLangRight()
    {

        if (en)
        {
            en = false;
            enTxt.SetActive(true);
            ruTxt.SetActive(false);

            enTxt.transform.DOMoveX(3, 0.5f, false).SetEase(Ease.InBack).OnComplete(() =>
            {
                ruTxt.SetActive(true);
                enTxt.SetActive(false);
                enTxt.transform.DOMoveX(0, 0, false);
                ruTxt.transform.DOMoveX(-3, 0.5f, false).SetEase(Ease.OutBack).From().OnComplete(() =>
                {
                    Lang.instance.SetRu();
                }); ;
            });
        }
        else
        {
            en = true;
            ruTxt.SetActive(true);
            enTxt.SetActive(false);

            ruTxt.transform.DOMoveX(3, 0.5f, false).SetEase(Ease.InBack).OnComplete(() =>
            {
                enTxt.SetActive(true);
                ruTxt.SetActive(false);
                ruTxt.transform.DOMoveX(0, 0, false);
                enTxt.transform.DOMoveX(-3, 0.5f, false).SetEase(Ease.OutBack).From().OnComplete(() =>
                {
                    Lang.instance.SetEn();
                });
            });
        }
    }

    public void RestartGame()
    {
        UIController.instance.RestartGame();
        Vector2 origPos = pauseWindow.transform.position;

        pauseWindow.transform.DOMoveY(10, 0.5f, false).SetEase(Ease.InBack).OnComplete(() =>
        {
            pauseWindow.SetActive(false);
            pauseWindow.transform.position = origPos;
            pause = false;
            player.transform.position = Vector2.zero;

        });
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void WakeUp()
    {
        UIController.instance.StopFukingGame();
    }
}

using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MoodController : MonoBehaviour
{
    public static MoodController instance;

    [SerializeField] private Transform dedline, dedText;
    [SerializeField] private float dedUp1, dedUp2, dedUp3;
    [SerializeField] private Image moodFill;
    [SerializeField] private float decayRate, replenishmentRate, dedlineSpeed;

    private float step = 0;
    private float deadTextPosY;
    private Vector2 deadTextScale;
    public bool decay = false;
    public bool upMood = false;
    private bool check1 = false;
    private bool check2 = false;
    private bool check3 = false;

    private void Awake()
    {
        instance = this;
        ShowMood(false);
        deadTextPosY = dedText.position.y;
        deadTextScale = dedText.localScale;
    }

    public void ShowMood(bool value)
    {
        moodFill.gameObject.SetActive(value);
    }

    public void UpMood(float value)
    {
        decay = false;
       // upMood = true;
        step = value * 0.01f; // Процент от максимального значения

        float newFillAmount = moodFill.fillAmount + step;
        newFillAmount = Mathf.Clamp01(newFillAmount); // Ограничение значения в пределах от 0 до 1

        moodFill.DOFillAmount(newFillAmount, 1f).OnComplete(()=> { decay = true; });
    }

    public void GameStart()
    {
        ShowMood(true);
        moodFill.fillAmount = 1f;
        decay = true;
    }

    public void StopGame()
    {
        decay = false;
        upMood = false;
        dedline.DOMoveY(-14, 0, false);
        moodFill.fillAmount = 1f;
    }

    private void Update()
    {
        if (decay)
        {
            if (moodFill.fillAmount > 0f)
            {
                moodFill.fillAmount -= decayRate * Time.deltaTime;
            }
            MoveDedline();
        }
    }

    private void MoveDedline()
    {
        if (moodFill.fillAmount <= 0.25f && !check1)
        {
            check1 = true;
            dedline.DOMoveY(dedUp1, dedlineSpeed, false);
            dedText.DOMoveY(-4.25f, dedlineSpeed, false);
            decayRate *= 0.5f;
        }
        else if (moodFill.fillAmount <= 0.12f && !check2)
        {
            check2 = true;
            dedline.DOMoveY(dedUp2, dedlineSpeed, false);
            dedText.DOMoveY(-4, dedlineSpeed, false);
            dedText.DOScale(deadTextScale * 1.3f, dedlineSpeed);
            decayRate *= 0.5f;
        }
        else if (moodFill.fillAmount <= 0.005f && !check3)
        {
            check3 = true;
            dedline.DOMoveY(dedUp3, dedlineSpeed, false);
        }
    }

    public void MoveDedlineUp()
    {
        if (dedline.position.y == dedUp1)
        {
            dedline.DOMoveY(-14, dedlineSpeed, false).OnComplete(() => { check1 = false; });
            dedText.DOMoveY(deadTextPosY, dedlineSpeed, false);
        }
        else if (dedline.position.y == dedUp2)
        {
            dedline.DOMoveY(dedUp1, dedlineSpeed, false).OnComplete(() => { check2 = false; });
            dedText.DOMoveY(-4.25f, dedlineSpeed, false);
            dedText.DOScale(deadTextScale, dedlineSpeed);
        }

        // UpMood(15);
    }
}

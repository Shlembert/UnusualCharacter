using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MoodController : MonoBehaviour
{
    public static MoodController instance;

    [SerializeField] private Transform dedline;
    [SerializeField] private float dedUp1, dedUp2, dedUp3;
    [SerializeField] private Image moodFill;
    [SerializeField] private float decayRate, replenishmentRate, dedlineSpeed;

    private float step = 0;
    public bool decay = false;
    public bool upMood = false;
    private bool check1 = false;
    private bool check2 = false;
    private bool check3 = false;

    private void Awake()
    {
        instance = this;
        ShowMood(false);
    }

    public void ShowMood(bool value)
    {
        moodFill.gameObject.SetActive(value);
    }

    public void UpMood(float value)
    {
        decay = false;
       // upMood = true;
        step = value * 0.01f; // ������� �� ������������� ��������

        float newFillAmount = moodFill.fillAmount + step;
        newFillAmount = Mathf.Clamp01(newFillAmount); // ����������� �������� � �������� �� 0 �� 1

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
        dedline.DOMoveY(-11, 0, false);
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
        if (moodFill.fillAmount <= 0.35f && !check1)
        {
            check1 = true;
            dedline.DOMoveY(dedUp1, dedlineSpeed, false);
        }
        else if (moodFill.fillAmount <= 0.2f && !check2)
        {
            check2 = true;
            dedline.DOMoveY(dedUp2, dedlineSpeed, false);
        }
        else if (moodFill.fillAmount <= 0.005f && !check3)
        {
            check3 = true;
            dedline.DOMoveY(dedUp3, dedlineSpeed, false);
        }
    }
}

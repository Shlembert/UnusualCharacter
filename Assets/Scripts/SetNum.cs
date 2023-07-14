using DG.Tweening;
using FMODUnity;
using UnityEngine;
using UnityEngine.UI;

public class SetNum : MonoBehaviour
{
    [SerializeField] private float animationDuration;
    [SerializeField] private Text coinsTxt, scoreText;
    [SerializeField] private StudioEventEmitter coinsSoundEmitter , tutudu, alarm;

    public void AnimateNumber()
    {
        int targetNumberCoins = UIController.instance.coins;
        int targetNumberScore = UIController.instance.score;
        int startNumber = 0;
        coinsSoundEmitter.Play();
        alarm.Play();
        DOTween.To(() => startNumber, x => coinsTxt.text = x.ToString(), targetNumberCoins, animationDuration)
            .SetEase(Ease.Linear).OnComplete(() => { coinsSoundEmitter.Stop(); tutudu.Play();});
        DOTween.To(() => startNumber, x => scoreText.text = x.ToString(), targetNumberScore, animationDuration)
            .SetEase(Ease.Linear);
    }
}

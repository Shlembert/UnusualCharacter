using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SetNum : MonoBehaviour
{
    [SerializeField] private float animationDuration;
    [SerializeField] private Text coinsTxt, scoreText;

    public void AnimateNumber()
    {
        int targetNumberCoins = UIController.instance.coins;
        int targetNumberScore = UIController.instance.score;
        int startNumber = 0;

        DOTween.To(() => startNumber, x => coinsTxt.text = x.ToString(), targetNumberCoins, animationDuration)
            .SetEase(Ease.Linear);
        DOTween.To(() => startNumber, x => scoreText.text = x.ToString(), targetNumberScore, animationDuration)
            .SetEase(Ease.Linear);
    }
}

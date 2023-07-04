using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    [SerializeField] private FloatingJoystick joy;
    [SerializeField] private Text coinsTxt, scoreTxt, coinsNum, scoreNum;
    [SerializeField] private GameObject player, joystik, panelMenu, deadPnael;
    [SerializeField] private List<SpawnerObstacle> spawnersOb;
    [SerializeField] private List<SpawnerObstacle> spawnersBo;
    [SerializeField] private List<Image> hp;
    [SerializeField] private Transform den, table, btn0, btn1, btn2, title, menu, people, cooler, gus, guitar, mask, playerM;
    [SerializeField] private Text bonus;
    [SerializeField] private int pauseBonus, priceChip;
    [SerializeField] private AudioSource source, sfxSound;
    [SerializeField] private AudioClip musicSound, bangSound;
   

    public int score = 0;
    public int coins = 0;
    private Vector2 _parkPlayer = new Vector2(10, 0);
    private int _hpCount;

    public void SetScore(int score)
    {
        this.score = score;
        scoreNum.text = score.ToString();
    }
    // private int _bonusCount = 0;

    private void Start()
    {
        instance = this;

        source.enabled = false;
        _hpCount = hp.Count;
        player.transform.position = _parkPlayer;
        SettingsController.instance.play = false;
    }

    // ����� ��� ������������ ������ �������� � JSON ������
    [System.Serializable]
    private class SettingsData
    {
        public float joySpeed;
    }

    public async void StartGame()
    {
        await HideMenu();
        await UniTask.Delay(400);
        player.transform.position = Vector2.zero;

        source.enabled = true;
        panelMenu.SetActive(false);
        _hpCount = hp.Count;
        joystik.SetActive(true);
        joy.enabled = true;
        BGScroller.instance.StartGame();
        MoodController.instance.GameStart();
        SettingsController.instance.play = true;

        foreach (var item in hp) item.CrossFadeAlpha(1f, 0f, false);

        foreach (var item in spawnersOb)
        {
            item.StartGame();
            //await UniTask.Delay(5000);
        }

        foreach (var item in spawnersBo)
        {
            item.StartGame();
            //await UniTask.Delay(2500);
        }
    }

    public void StopFukingGame()
    {
        DOTween.KillAll();
        BGScroller.instance.StopGame();

        foreach (var item in spawnersOb)
        {
            item.StopGame();
            item.StopAllCoroutines();
        }

        foreach (var item in spawnersBo)
        {
            item.StopGame();
            item.StopAllCoroutines();
        }

        SceneManager.LoadScene(0);
    }

    public async void RestartGame()
    {
        _hpCount = hp.Count;
        score = 0;
        coins = 0;

        scoreNum.text = score.ToString();
        BGScroller.instance.StopGame();
        MoodController.instance.StopGame();

        foreach (var item in hp) item.CrossFadeAlpha(1f, 0f, false);

        foreach (var item in spawnersOb)
        {
            item.StopGame();
        }

        foreach (var item in spawnersBo)
        {
            item.StopGame();
        }

        player.transform.position = Vector2.zero;

        source.enabled = true;
        _hpCount = hp.Count;
        joystik.SetActive(true);
        joy.enabled = true;

        BGScroller.instance.StartGame();
        MoodController.instance.GameStart();

        foreach (var item in spawnersOb)
        {
            item.RestartExit();
            item.StartGame();
            //await UniTask.Delay(5000);
        }

        await UniTask.Delay(1500);

        foreach (var item in spawnersBo)
        {

            item.RestartExit();
            item.StartGame();
            //await UniTask.Delay(2500);
        }
    }

    public async void NoPause()
    {
        await UniTask.Delay(1500);

        foreach (var item in spawnersOb)
        {
            item.StartGame();
            //await UniTask.Delay(5000);
        }

        foreach (var item in spawnersBo)
        {
            item.StartGame();
            //await UniTask.Delay(500);
        }
    }

    private async UniTask HideMenu()
    {
        UniTaskCompletionSource completionSource = new UniTaskCompletionSource();

        await UniTask.Delay(500);
        title.DOMoveY(10, 0.9f, false).SetEase(Ease.InBack);
        await UniTask.Delay(600);
        btn0.DOMoveY(-10, 0.9f, false).SetEase(Ease.InBack);
        await UniTask.Delay(200);
        btn1.DOMoveX(7, 0.9f, false).SetEase(Ease.InBack);
        gus.DOMoveX(-7, 0.9f, false).SetEase(Ease.InBack);
        await UniTask.Delay(100);
        btn2.DOMoveX(7, 0.9f, false).SetEase(Ease.InBack);
        guitar.DOMoveX(-7, 0.9f, false).SetEase(Ease.InBack);
        await UniTask.Delay(300);
        table.GetComponent<Image>().DOFade(0, 0.9f);
        people.DOMoveX(6, 1.7f, false);
        cooler.DOScale(0, 0.9f);
        await UniTask.Delay(300);
        menu.GetComponent<Image>().DOFade(0, 1.2f);
        mask.gameObject.SetActive(false);

        den.gameObject.SetActive(false);
        playerM.gameObject.SetActive(true);
        playerM.DOMove(Vector2.zero, 0.5f);
        playerM.DOScale(0.4f, 0.5f);

        completionSource.TrySetResult();
    }

    public void PlusCoins()
    {
        coins+= priceChip;
    }

    public void StopGame()
    {
        DOTween.KillAll();
        BGScroller.instance.StopGame();
        MoodController.instance.StopGame();

        foreach (var item in spawnersOb)
        {
            item.StopGame();
            item.StopAllCoroutines();
        }

        foreach (var item in spawnersBo)
        {
            item.StopGame();
            item.StopAllCoroutines();
        }

        source.enabled = false;

        joystik.SetActive(false);
        joy.enabled = false;
        player.SetActive(false);
        deadPnael.SetActive(true);
    }

    public async void Damage()
    {
        CameraShake.instance.ShakeCamera(0.5f, 0.3f);

        if (_hpCount > 0)
        {
            PlayerMovement.instance.Damage();
            //sfxSound.PlayOneShot(bangSound);
            var item = hp[_hpCount - 1];
            item.CrossFadeAlpha(0.05f, 0.3f, false);

            _hpCount--;
        }
        else
        {
            //sfxSound.PlayOneShot(bangSound);
            Time.timeScale = 0.2f;
            player.GetComponent<PlayerMovement>().Fatality();
            await UniTask.Delay(300);
            Time.timeScale = 1f;
            StopGame();
        }
    }

    public void PressExit()
    {
        Application.Quit();
    }
}

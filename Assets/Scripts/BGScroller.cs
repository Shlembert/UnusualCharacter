using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class BGScroller : MonoBehaviour
{
    public static BGScroller instance;

    [SerializeField] private Transform bg1, bg2, bgFon;
    [SerializeField] private SpriteRenderer flash;

    public float speed;

    private float _startSpeed;
    private float p1;
    private float p2;
    private float bg;

    private Transform _flashTransform;

    private void Start()
    {
        instance = this;
        _flashTransform = flash.transform;
        _startSpeed = speed;
    }

    private void OnDisable()
    {
        DOTween.KillAll();
    }

    public void StartGame()
    {
        _startSpeed = speed;
        p1 = bg1.position.y;
        p2 = bg2.position.y;
        bg = bgFon.position.y;

        StartMoving();
    }

    public void StopGame()
    {
        StopMoving();
        bg1.position = new Vector2(0, p1);
        bg2.position = new Vector2(0, p2);
        bgFon.position = new Vector2(0, bg);
        speed = _startSpeed;
    }

    private bool isMoving = false;

    private void Update()
    {
        if (!isMoving)
            return;

        float step = -speed * Time.deltaTime;
        float stepBG = - speed * 0.05f * Time.deltaTime;

        bg1.position += new Vector3(0, step, 0);
        bg2.position += new Vector3(0, step, 0);
        bgFon.position += new Vector3(0, stepBG, 0);

        if (bg2.position.y <= p1)
        {
            bg1.position = new Vector2(bg1.position.x, p1);
            bg2.position = new Vector2(bg2.position.x, p2);
        }
    }

    public void StartMoving()
    {
        isMoving = true;
    }

    public void StopMoving()
    {
        isMoving = false;
    }

    public void ChangeSpeed()
    {
        if (speed < 1f) speed = 1;
        else if (speed < 10f) speed *= 1.5f;
        else speed *= 1.2f;

        Vector2 size = _flashTransform.localScale;

        flash.gameObject.SetActive(true);
        _flashTransform.DOMoveY(-2, 1.9f, false).SetEase(Ease.OutBack).OnComplete(async () =>
        {
            await UniTask.Delay(500);
            flash.DOFade(0, 0.5f).From();
            _flashTransform.DOMoveY(-10, 0.9f, false).SetEase(Ease.InBack).OnComplete(async () =>
            {
                await UniTask.Delay(1000);
                if (flash)
                {
                    flash.gameObject.SetActive(false);
                    _flashTransform.localScale = size;
                    _flashTransform.position = new Vector2(0, 3);

                }
            });
        });
    }
}

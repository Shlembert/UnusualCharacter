using Cysharp.Threading.Tasks;
using DG.Tweening;
using FMODUnity;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;
    public bool immune = false;
    public float sence;

    [SerializeField] private FloatingJoystick joystick;
    [SerializeField] private Transform panelGU;
    [SerializeField] private Transform den, seat, backrest, cross;
    [SerializeField] private SpriteRenderer denHeadSprite, denBodySprite, crossSprite, seatSprite, backrestSprite;
    [SerializeField] private ParticleSystem particle,starParticle;
    [SerializeField] private Animator faceAnimator;
    [SerializeField] private StudioEventEmitter stun,music;

    private new Rigidbody2D rigidbody;
    private CapsuleCollider2D capsuleCollider;

    private float leftBorder;
    private float rightBorder;
    private float topBorder;
    private float bottomBorder;

    private Transform denStart, seatStart, backStart, crossStart;

    private int defaultLayer;
    private readonly int obstacleLayer = 9;

    public SpriteRenderer DenBodySprite { set => denBodySprite = value; }
    public SpriteRenderer CrossSprite { set { crossSprite = value; cross = value.transform; } }
    public SpriteRenderer SeatSprite { set { seatSprite = value; seat = value.transform; } }
    public SpriteRenderer BackrestSprite { set { backrestSprite = value; backrest = value.transform; } }

    private void Awake()
    {
        //faceAnimator.SetInteger("Heals", UIController.instance.MaxHpCount);
        instance = this;
    }

    private  void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
       
        capsuleCollider = GetComponent<CapsuleCollider2D>();

        defaultLayer = gameObject.layer;
        denStart = den;
        seatStart = seat;
        backStart = backrest;
        crossStart = cross;

        // ќпределение границ экрана с учетом размеров персонажа
        leftBorder = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x + capsuleCollider.size.x / 2;
        rightBorder = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x - capsuleCollider.size.x / 2;
        topBorder = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y - panelGU.localScale.y * 1.1f - capsuleCollider.size.y / 2;
        bottomBorder = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y  + 0.5f+ capsuleCollider.size.y / 2;
    }

    public void Damage()
    {
        stun.Play();
        music.SetParameter("STUN", 1f);
        starParticle.Play();
        SetLayerRecursively(gameObject, obstacleLayer);
        immune = true;
        faceAnimator.SetBool("Dizziness", immune);
        faceAnimator.SetInteger("Heals",UIController.instance.HpCount);
        Blinking();
    }



    public void PsPlay()
    {
        particle.Play();
    }

    public void PsStop()
    {
        particle.Stop();
    }

    private void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }

    private async void Blinking()
    {
        for (int i = 0; i < 9; i++)
        {
            denHeadSprite.DOFade(0, 0.2f);
            denBodySprite.DOFade(0, 0.2f);
            crossSprite.DOFade(0, 0.2f);
            seatSprite.DOFade(0, 0.2f);
            backrestSprite.DOFade(0, 0.2f);
            await UniTask.Delay(200);
            denHeadSprite.DOFade(1, 0.2f);
            denBodySprite.DOFade(1, 0.2f);
            crossSprite.DOFade(1, 0.2f);
            seatSprite.DOFade(1, 0.2f);
            backrestSprite.DOFade(1, 0.2f);
            await UniTask.Delay(200);
        }
        music.SetParameter("STUN", 0f);
        immune = false;
        faceAnimator.SetBool("Dizziness", immune);
        SetLayerRecursively(gameObject, defaultLayer);
    }

    private void OnDestroy()
    {
        DOTween.KillAll();
    }

    private void OnDisable()
    {
        DOTween.KillAll();
    }

    public void Fatality()
    {
        capsuleCollider.enabled = false;
        den.DOScale(0.5f, 0.3f);
        seat.DOScale(5.5f, 0.2f);
        backrest.DOScale(5.5f, 0.2f);
        cross.DOScale(5.5f, 0.2f);
        den.DORotate(new Vector3(0, 0, 180), 0.5f);
        den.DOMoveY(-20, 0.5f, false);
        seat.DOMoveX(9, 0.3f, false);
        backrest.DOMoveX(-9, 0.3f, false);
        cross.DOMoveY(25, 0.3f, false);
    }

    public void Restart()
    {
        faceAnimator.SetInteger("Heals", UIController.instance.MaxHpCount);
        capsuleCollider.enabled = true;
        den.localScale = denStart.localScale;
        seat.localScale = seatStart.localScale;
        backrest.localScale = backStart.localScale;
        cross.localScale = cross.localScale;
        den.rotation = denStart.rotation;
        den.position = denStart.position;
        backrest.position = backStart.position;
        cross.position = crossStart.position;
    }

    private void FixedUpdate()
    {
        if (joystick.isActiveAndEnabled)
        {
            if (joystick.Horizontal != 0 || joystick.Vertical != 0)
            {
                if (transform.position.x < leftBorder && joystick.Horizontal < 0)
                {
                    // ƒостигнуто левое ограничение, игнорируем горизонтальное движение влево
                    rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
                }
                else if (transform.position.x > rightBorder && joystick.Horizontal > 0)
                {
                    // ƒостигнуто правое ограничение, игнорируем горизонтальное движение вправо
                    rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
                }

                if (transform.position.y < bottomBorder && joystick.Vertical < 0)
                {
                    // ƒостигнуто нижнее ограничение, игнорируем вертикальное движение вниз
                    rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
                }
                else if (transform.position.y > topBorder && joystick.Vertical > 0)
                {
                    // ƒостигнуто верхнее ограничение, игнорируем вертикальное движение вверх
                    rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
                }

                // ќбновл€ем скорость персонажа на основе значени€ джойстика
                Vector2 move = new Vector2(joystick.Horizontal, joystick.Vertical) * sence;
                rigidbody.velocity = move;
                return;
            }
        }
    }
}

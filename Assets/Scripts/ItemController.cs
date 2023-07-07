using UnityEngine;

public enum TypeItem { obstacle, bonus}

public class ItemController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private TypeItem type;
    [SerializeField] private ObstacleType bonus;
    [SerializeField] private float plusMood;
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private GameObject area;

    private AudioSource audioSource;
    private SpawnerObstacle spawner;
    private float _maxRotationSpeed; // Максимальная скорость вращения
    private float _impulseX;
    private float _impulseY;

    private UIController uIController;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        spawner = transform.parent.GetComponent<SpawnerObstacle>();
        uIController = FindObjectOfType<UIController>();
    }

    public void ViewArea(bool volume)
    {
        area.SetActive(volume);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerMovement>(out PlayerMovement player))
        {
            audioSource.PlayOneShot(audioClip);
            ViewArea(false);
            if (type == TypeItem.obstacle)
            {
                if (!player.immune)
                {
                    uIController.Damage();
                    spawner.ReturnToPool(transform);
                }
            }
            else 
            {
                CheckBonus();
                spawner.ReturnToPool(transform);
            }
        }
        else if (collision.gameObject.CompareTag("KillZone"))
        {
            spawner.ReturnToPoolFromKillZone(transform);
        }
        else if (collision.gameObject.CompareTag("Border"))
        {
            _impulseX = -_impulseX;
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(_impulseX, _impulseY), ForceMode2D.Impulse);
        }
    }

    private void CheckBonus()
    {
        switch (bonus)
        {
            case ObstacleType.THC:
                MoodController.instance.UpMood(plusMood);
                break;
            case ObstacleType.Chip: 
                UIController.instance.PlusCoins();
                //MoodController.instance.UpMood(plusMood);
                break;
            case ObstacleType.Guitar:
                MoodController.instance.UpMood(plusMood);
                break;
            case ObstacleType.Plan:
                MoodController.instance.MoveDedlineUp();
                break;
            case ObstacleType.Default:
                break;
            default:
                break;
        }
    }

    public void MoveItem(float minDuration, float maxDuration, float maxSpeedX, float maxRotationSpeed)
    {
        float impulseForceY = -1f * Random.Range(minDuration, maxDuration); // Устанавливаем случайное значение множителя для скорости движения объекта по оси Y

        float impulseForceX = Random.Range(-1f, 1f) * maxSpeedX; // Генерируем случайное значение для скорости движения объекта по оси X

        _maxRotationSpeed = maxRotationSpeed;
        _impulseY = impulseForceY;
        _impulseX = impulseForceX;
        // Применяем импульсы к Rigidbody2D объекта

        rb.AddForce(new Vector2(_impulseX, impulseForceY), ForceMode2D.Impulse);

        float torque;

        // Устанавливаем случайное вращение
        if (bonus == ObstacleType.Extinguisher)
        {
            torque = 40;
            rb.AddTorque(torque, ForceMode2D.Force);
        }
        else 
        {
            torque = Random.Range(-1f, 1f) * 0.05f; // Генерируем случайное значение для вращения

            rb.AddTorque(torque, ForceMode2D.Impulse); // Применяем случайное вращение
        }
        
    }
    private void FixedUpdate(
)
    {
        LimitRotationSpeed(); // Ограничение скорости вращения
    }

    private void LimitRotationSpeed()
    {
        float currentRotationSpeed = rb.angularVelocity; // Текущая скорость вращения
        float clampedRotationSpeed = Mathf.Clamp(currentRotationSpeed, -_maxRotationSpeed, _maxRotationSpeed); // Ограничение скорости вращения
        rb.angularVelocity = clampedRotationSpeed; // Присвоение ограниченной скорости вращения

        // Дополнительные проверки и ограничения, если необходимо
        if (Mathf.Abs(rb.angularVelocity) > _maxRotationSpeed)
        {
            // Выполнить дополнительные действия, например, изменить направление вращения или остановить вращение
        }
    }
}

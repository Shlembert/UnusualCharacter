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
    private float _maxRotationSpeed; // ������������ �������� ��������
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
        float impulseForceY = -1f * Random.Range(minDuration, maxDuration); // ������������� ��������� �������� ��������� ��� �������� �������� ������� �� ��� Y

        float impulseForceX = Random.Range(-1f, 1f) * maxSpeedX; // ���������� ��������� �������� ��� �������� �������� ������� �� ��� X

        _maxRotationSpeed = maxRotationSpeed;
        _impulseY = impulseForceY;
        _impulseX = impulseForceX;
        // ��������� �������� � Rigidbody2D �������

        rb.AddForce(new Vector2(_impulseX, impulseForceY), ForceMode2D.Impulse);

        float torque;

        // ������������� ��������� ��������
        if (bonus == ObstacleType.Extinguisher)
        {
            torque = 40;
            rb.AddTorque(torque, ForceMode2D.Force);
        }
        else 
        {
            torque = Random.Range(-1f, 1f) * 0.05f; // ���������� ��������� �������� ��� ��������

            rb.AddTorque(torque, ForceMode2D.Impulse); // ��������� ��������� ��������
        }
        
    }
    private void FixedUpdate(
)
    {
        LimitRotationSpeed(); // ����������� �������� ��������
    }

    private void LimitRotationSpeed()
    {
        float currentRotationSpeed = rb.angularVelocity; // ������� �������� ��������
        float clampedRotationSpeed = Mathf.Clamp(currentRotationSpeed, -_maxRotationSpeed, _maxRotationSpeed); // ����������� �������� ��������
        rb.angularVelocity = clampedRotationSpeed; // ���������� ������������ �������� ��������

        // �������������� �������� � �����������, ���� ����������
        if (Mathf.Abs(rb.angularVelocity) > _maxRotationSpeed)
        {
            // ��������� �������������� ��������, ��������, �������� ����������� �������� ��� ���������� ��������
        }
    }
}

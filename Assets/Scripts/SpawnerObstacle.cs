using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObstacleType { PC, Monitor, Table, THC, Chip, Guitar, Plan, Moto, Extinguisher, Default }

public class SpawnerObstacle : MonoBehaviour
{
    [SerializeField] private ObstacleType obType;
    [SerializeField] private Transform itemPool;
    [SerializeField] private Transform player;
    [SerializeField] private SpriteRenderer spawnArea;
    [SerializeField] private float minTimeBetweenSpawns;
    [SerializeField] private float maxTimeBetweenSpawns;
    [SerializeField] private float maxSpawnOffset = 0.5f;
    [SerializeField] private float maxSpeedX;
    [SerializeField] private float minDuration;
    [SerializeField] private float maxDuration;
    [SerializeField] private int complexityShift;
    [SerializeField] private float maxRotationSpeed;

    private int _scoreSpawn;
    private int _startComplex;
    private float _startMaxDur;
    private float _startMinDur;
    private float _startSpeedX;
    private float _startMinTimeSpawn;
    private float _startMaxTimeSpawn;

    private List<Vector2> directions = new List<Vector2>();

    private bool isPause = false;

    private Coroutine _corutine;

    private void Start()
    {
        _startMinTimeSpawn = minTimeBetweenSpawns;
        _startMaxTimeSpawn = maxTimeBetweenSpawns;
    }
    public void StartGame()
    {
        _startComplex = complexityShift;
        _startMaxDur = maxDuration;
        _startMinDur = minDuration;
        _startSpeedX = maxSpeedX;
        _startMinTimeSpawn = minTimeBetweenSpawns;
        _startMaxTimeSpawn = maxTimeBetweenSpawns;

        if (!isPause) _corutine = StartCoroutine(SpawnItem());
    }

    public void StopGame()
    {
        isPause = true;
        if (_corutine != null) StopCoroutine(_corutine);
        DOTween.KillAll();
        complexityShift = _startComplex;
        maxDuration = _startMaxDur;
        minDuration = _startMinDur;
        maxSpeedX = _startSpeedX;
        minTimeBetweenSpawns = _startMinTimeSpawn;
        maxTimeBetweenSpawns = _startMaxTimeSpawn;

        _scoreSpawn = 0;

        foreach (Transform item in transform)
        {
            if (item.gameObject.activeSelf)
            {
                item.DOKill();
                item.gameObject.SetActive(false);
            }
        }
    }

    public void Pause()
    {
        isPause = true;
        if (_corutine != null) StopCoroutine(_corutine);

        foreach (Transform item in transform)
        {
            if (item.gameObject.activeSelf)
            {
                directions.Add(item.GetComponent<Rigidbody2D>().velocity);
                item.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
    }

    public void PauseExit()
    {
        isPause = false;
        int i = 0;

        foreach (Transform item in transform)
        {
            if (item.gameObject.activeSelf)
            {
                item.GetComponent<Rigidbody2D>().velocity = directions[i];
                i++;
            }
        }

        directions.Clear();
    }

    public void RestartExit()
    {
        isPause = false;
    }

    private IEnumerator SpawnItem()
    {
        yield return new WaitForSeconds(Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns));
        while (!isPause)
        {   
            Transform inactiveItem = null;

            // Проверяем наличие неактивных айтемов в пуле и используем их, если они есть
            foreach (Transform item in transform)
            {
                if (!item.gameObject.activeSelf)
                {
                    inactiveItem = item;
                    break;
                }
            }

            // Если есть неактивный объект, используем его
            if (inactiveItem != null)
            {
                inactiveItem.gameObject.SetActive(true);
                inactiveItem.position = GetRandomSpawnPosition();
                ItemController itemController = inactiveItem.GetComponent<ItemController>();
                itemController.MoveItem(minDuration, maxDuration, maxSpeedX, maxRotationSpeed);
                if (obType == ObstacleType.PC) CheckComplexityShift();
            }
            else
            {
                Transform newItem = Instantiate(itemPool, GetRandomSpawnPosition(), Quaternion.identity, transform);
                ItemController itemController = newItem.GetComponent<ItemController>();
                itemController.MoveItem(minDuration, maxDuration, maxSpeedX, maxRotationSpeed);
                if (obType == ObstacleType.PC) CheckComplexityShift();
            }
            yield return new WaitForSeconds(Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns));
        }
    }

    // Получаем рандомную позицию для спауна айтема внутри спавн-зоны
    private Vector2 GetRandomSpawnPosition()
    {
        Bounds spawnBounds = spawnArea.bounds;
        float spawnX = UnityEngine.Random.Range(spawnBounds.min.x, spawnBounds.max.x);
        float spawnY = UnityEngine.Random.Range(spawnBounds.min.y, spawnBounds.max.y);

        // Добавляем разброс (случайное смещение) к позиции спавна
        float randomOffsetX = UnityEngine.Random.Range(-maxSpawnOffset, maxSpawnOffset);
        float randomOffsetY = UnityEngine.Random.Range(-maxSpawnOffset, maxSpawnOffset);
        spawnX += randomOffsetX;
        spawnY += randomOffsetY;

        if (obType == ObstacleType.PC && player) return new Vector2(player.position.x, spawnY);
        else return new Vector2(spawnX, spawnY);

    }

    // Метод для деактивации и возврата в пул
    public void ReturnToPool(Transform item)
    {

        item.GetComponent<Collider2D>().enabled = false;
        Vector2 scale = item.localScale;
        item.position = Vector2.zero;
        item.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        item.GetComponent<SpriteRenderer>().DOFade(0, 0.35f);
        item.DOScale(scale * 7, 0.4f).OnComplete(() =>
        {
            item.gameObject.SetActive(false);
            item.GetComponent<Collider2D>().enabled = true;
            item.GetComponent<SpriteRenderer>().DOFade(1, 0);
            item.localScale = scale;
        });
    }

    public void ReturnToPoolFromKillZone(Transform item)
    {
        item.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        item.gameObject.SetActive(false);
    }

    private void CheckComplexityShift()
    {
        _scoreSpawn++;
        UIController.instance.SetScore(_scoreSpawn);
        if (_scoreSpawn % 20 == 0) BGScroller.instance.ChangeSpeed();

    }
}

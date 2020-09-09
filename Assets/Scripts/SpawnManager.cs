using System.Collections;
using UnityEngine;

public class SpawnManager : MonoSingleton<SpawnManager>
{
    [SerializeField]
    GameObject _enemyPrefab;
    [SerializeField]
    GameObject _enemyContainer;
    [SerializeField]
    GameObject[] _powerUpPrefabs;
    [SerializeField]
    GameObject _powerUpContainer;

    float _waitTimeBetweenEnemySpawns;
    float _waitTimeBetweenPowerUpSpawns;
    float _delayAfterAsteroidDestroyed = 2.5f;
    bool _playerIsAlive = true;

    public override void Init()
    {
        base.Init();
        //Debug.Log("SpawnManager has been initialized");
    }

    private void Start()
    {
        //Spawn();
    }
    public void Spawn()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }
    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(_delayAfterAsteroidDestroyed);
        while (_playerIsAlive)
        {
            _waitTimeBetweenEnemySpawns = Random.Range(0.5f, 3.0f);
            GameObject newEnemy = Instantiate(_enemyPrefab, new Vector3(0, 10, 0), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_waitTimeBetweenEnemySpawns);
        }
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(_delayAfterAsteroidDestroyed + Random.Range(4.0f, 8.0f));
        while (_playerIsAlive)
        {
            _waitTimeBetweenPowerUpSpawns = Random.Range(9.0f, 15.0f);
            int _RNDPowerUp = Random.Range(0, _powerUpPrefabs.Length);
            // if Shield is active do not spawn another
            GameObject newPowerUp = Instantiate(_powerUpPrefabs[_RNDPowerUp], new Vector3(Random.Range(-6, 6), Random.Range(7, 14), 0), Quaternion.identity);
            newPowerUp.transform.parent = _powerUpContainer.transform;
            yield return new WaitForSeconds(_waitTimeBetweenPowerUpSpawns);
        }
    }
    public void OnPlayerDeath()
    {
        _playerIsAlive = false;
    }
}
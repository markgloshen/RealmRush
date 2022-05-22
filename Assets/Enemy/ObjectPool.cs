using System;
using System.Collections;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField][Range(0, 50)] private int _poolSize = 5;
    [SerializeField][Range(0.1f, 30f)] private float _spawnInterval = 2.5f;

    private GameObject[] _pool;

    private void Awake()
    {
        PopulatePool();
    }

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private void PopulatePool()
    {
        _pool = new GameObject[_poolSize];

        for (int i = 0; i < _pool.Length; i++)
        {
            _pool[i] = Instantiate(_enemyPrefab, transform);
            _pool[i].SetActive(false);
        }
    }

    private void EnableObjectInPool()
    {
        for (int i = 0; i < _pool.Length; i++)
        {
            if (!_pool[i].activeInHierarchy)
            {
                _pool[i].SetActive(true);
                return;
            }
        }
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            EnableObjectInPool();
            yield return new WaitForSeconds(_spawnInterval);
        }
    }
}
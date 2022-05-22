using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int _maxHitPoints = 5;

    [Tooltip("Adds amount to Max Hit Points when enmey dies.")]
    [SerializeField] private int _difficultyRamp = 1;
    
    private int _currentHitPoints = 0;
    private Enemy _enemy;

    private void OnEnable()
    {
        _currentHitPoints = _maxHitPoints;
    }

    private void Start()
    {
        _enemy = GetComponent<Enemy>();
    }

    private void OnParticleCollision(GameObject other)
    {
        ProcessHit();
    }

    private void ProcessHit()
    {
        _currentHitPoints--;

        if (_currentHitPoints < 1)
        {
            gameObject.SetActive(false);
            _maxHitPoints += _difficultyRamp;
            _enemy.RewardGold();
        }
    }
}
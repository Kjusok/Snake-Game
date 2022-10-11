using UnityEngine;

public class Apple : MonoBehaviour
{
    [SerializeField] private Animator _animAlarm;
    [SerializeField] private ParticleSystem _deathEffect;

    private float _timerForDestroy;

    private const float DestroyTime = 10f;
    private const float AnimationTime = 4f;


    private void Start()
    {
        _timerForDestroy = DestroyTime;
    }

    private void Update()
    {
        if (GameManager.Instance.GameIsPaused)
        {
            return;
        }

        if (_timerForDestroy > 0)
        {
            _timerForDestroy -= Time.deltaTime;
        }
        else
        {
            AppleDestroy();
        }

        if (_timerForDestroy < AnimationTime)
        {
            _animAlarm.SetTrigger("Alarm");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        AppleDestroy();
    }

    private void AppleDestroy()
    {
        SpawnDeathEffect();

        GameManager.Instance.AppleWasDestroyed();
        Destroy(gameObject);
    }

    private void SpawnDeathEffect()
    {
        var applePosition = transform.position;
        var spawnPosition = new Vector2(applePosition.x, applePosition.y);

        var deathEffect = Instantiate(_deathEffect.gameObject, spawnPosition, Quaternion.identity);

        Destroy(deathEffect, _deathEffect.main.startLifetime.constant);
    }
}


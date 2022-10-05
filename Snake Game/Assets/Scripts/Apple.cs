using UnityEngine;

public class Apple : MonoBehaviour
{
    [SerializeField] private Animator _animAlarm;
    [SerializeField] private ParticleSystem _deathEffect;

    private float _timerForDestroy;


    private void Start()
    {
        _timerForDestroy = 10;
    }

    private void Update()
    {
        if (_timerForDestroy > 0)
        {
            _timerForDestroy -= Time.deltaTime;
        }
        else
        {
            AppleDestroy();
        }
        if (_timerForDestroy < 4)
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
        SpaenDeathEffect();
        GameManager.Instance.AppleWasDestroyed();
        Destroy(gameObject);

        GameManager.Instance.SpawnApples();
    }

    private void SpaenDeathEffect()
    {
        var apllePosition = transform.position;
        var spawnPosition = new Vector2(apllePosition.x, apllePosition.y);

        var deathEffect = Instantiate(_deathEffect.gameObject, spawnPosition, Quaternion.identity);

        Destroy(deathEffect, _deathEffect.main.startLifetime.constant);
    }
}


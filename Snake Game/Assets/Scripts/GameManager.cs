using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Instance not specified");
            }
            return _instance;
        }
    }

    [SerializeField] private RectTransform _boardSpawn;
    [SerializeField] private Apple _applePrefub;
    [SerializeField] private Text _scoreText;
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _gameOverText;
    [SerializeField] private Button _startGameButton;
    [SerializeField] private List<Vector2> _possitionForSpawnList;
    [SerializeField] private Snake _listPosSnakeTail;

    private int _score;
    private Vector2 _positionsForSpawnApple;
    private int _counterApples;

    static bool _gameIsPlaying = true;

    private const float _maxY = 4.25f;
    private const float _minY = -4.25f;
    private const float _maxX = 8.25f;
    private const float _minX = -8.25f;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        Time.timeScale = 0;

        if (!_gameIsPlaying)
        {
            Time.timeScale = 1;

            _menuPanel.SetActive(false);
            _gameOverText.SetActive(false);
        }

        TakeCardinatsForSpawn();
        SpawnApples();
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }

    private void TakeCardinatsForSpawn()
    {
        for (float i = _minY; i <= _maxY; i += 0.5f)
        {
            var posY = i;

            for (float j = _minX; j <= _maxX; j += 0.5f)
            {
                var posX = j;
                var position = new Vector2(posX, posY);

                _possitionForSpawnList.Add(position);
            }
        }
    }

    private void TakePositionForSpawnApple()
    {
        _positionsForSpawnApple = _possitionForSpawnList[Random.Range(0, _possitionForSpawnList.Count)];

        for (int i = 0; i<= _listPosSnakeTail.TailSnakeListPosition.Count - 1; i++)
        {
            var position = _listPosSnakeTail.TailSnakeListPosition[i];
            if(_positionsForSpawnApple == position)
            {
                TakePositionForSpawnApple();
            }
        }
    }

    private void SpawnOneApple()
    {
        TakePositionForSpawnApple();

        var aplle = Instantiate(_applePrefub, _positionsForSpawnApple, Quaternion.identity);
        aplle.transform.SetParent(_boardSpawn.transform, false);

        _counterApples++;
    }

    public void SpawnApples()
    {
        if(_counterApples == 0)
        {
            SpawnOneApple();
        }

        if (Random.Range(0, 5) == 0 && _counterApples < 2)
        {
            SpawnOneApple();
        }
    }

    public void AppleWasDestroyed()
    {
        _counterApples--;
    }

    public void IncreaseScore()
    {
        _score++;

        _scoreText.text = _score.ToString();
    }

    public void ActivateMenuForRestart()
    {
        _menuPanel.SetActive(true);
        _gameOverText.SetActive(true);
        _gameIsPlaying = false;

        Time.timeScale = 0;
    }

    public void PressStartButton()
    {
        Time.timeScale = 1;
        _menuPanel.SetActive(false);
        _gameOverText.SetActive(false);

        if (!_gameIsPlaying)
        {
            SceneManager.LoadScene("MainScene");
        }
    }
}

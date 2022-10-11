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
    [SerializeField] private List<Vector2> _possitionForSpawnList;
    [SerializeField] private Snake _listPosSnakeTail;

    private bool _isPaused;
    private int _score;
    private Vector2 _positionsForSpawnApple;
    private int _counterApples;

    private const float MaxBoardY = 4.25f;
    private const float MinBoardY = -4.25f;
    private const float MaxBoardX = 8.25f;
    private const float MinBoardX = -8.25f;
    private const float DropPercentage = 0.2f;

    static bool _gameIsPlaying = true;

    public bool GameIsPaused => _isPaused;
    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        _isPaused = true;

        if (!_gameIsPlaying)
        {
            _isPaused = false;
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
        for (float i = MinBoardY; i <= MaxBoardY; i += 0.5f)
        {
            var posY = i;

            for (float j = MinBoardX; j <= MaxBoardX; j += 0.5f)
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

        var apple = Instantiate(_applePrefub, _positionsForSpawnApple, Quaternion.identity);
        apple.transform.SetParent(_boardSpawn.transform, false);

        _counterApples++;
    }

    private void SpawnApples()
    {
        if(_counterApples == 0)
        {
            SpawnOneApple();
        }

        if (Random.Range(0, 1f) == DropPercentage && _counterApples < 2)
        {
            SpawnOneApple();
        }
    }
   
    public void AppleWasDestroyed()
    {
        _counterApples--;

        SpawnApples();
    }

    public void IncreaseScore()
    {
        _score++;

        _scoreText.text = _score.ToString();
    }

    public void GameOver()
    {
        _menuPanel.SetActive(true);
        _gameOverText.SetActive(true);
        _gameIsPlaying = false;
        _isPaused = true;
    }

    public void PressStartButton()
    {
        _isPaused = false;
        _menuPanel.SetActive(false);
        _gameOverText.SetActive(false);


        if (!_gameIsPlaying)
        {
            SceneManager.LoadScene("MainScene");
        }
    }
}

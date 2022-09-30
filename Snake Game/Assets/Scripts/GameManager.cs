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
    [SerializeField] private List<float> _positionsY;
    [SerializeField] private List<float> _positionsX;
    [SerializeField] private Text _scoreText;
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _gameOverText;
    [SerializeField] private Button _startGameButton;

    private int _score;

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

        for (float i = _minY; i <= _maxY;)
        {
            _positionsY.Add(i);
            i += 0.5f;
        }
        for (float j = _minX; j <= _maxX;)
        {
            _positionsX.Add(j);
            j += 0.5f;
        }

        SpawnAplle();
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }

    public void SpawnAplle()
    {
        var aplle = Instantiate(_applePrefub,
            new Vector2(_positionsX[Random.Range(0, _positionsX.Count)],
            _positionsY[Random.Range(0, _positionsY.Count)]),
            Quaternion.identity);
        aplle.transform.SetParent(_boardSpawn.transform, false);
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

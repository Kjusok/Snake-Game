using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    [SerializeField] private float _delayForNextStepSnake;
    [SerializeField] private Transform _tailSnakePrefab;
    [SerializeField] private RectTransform _gameBoard;
    [SerializeField] List<Transform> _tailSnake;
    [SerializeField] List<Vector2> _tailSnakeListPosition;

    private Vector2 _direction;
    private Vector2 _previosPosition;

    private const int _step = 2;


    private void Start()
    {
        _direction = Vector2.right / _step;
        
        StartCoroutine(MovingSnake());
    }

    private void Update()
    {
        MovementSnake();

        for (int i = 0; i < _tailSnake.Count; i++)
        {
            _tailSnake[i].position = _tailSnakeListPosition[i];
        }
    }

    private IEnumerator MovingSnake()
    {
        while (true)
        {
            transform.position = new Vector3(
                transform.position.x + _direction.x,
                transform.position.y + _direction.y, 0);

            _previosPosition = transform.position;

            if (_tailSnakeListPosition.Count < _tailSnake.Count + 1)
            {
                _tailSnakeListPosition.Add(_previosPosition);
            }
            else
            {
                _tailSnakeListPosition.RemoveAt(0);
                _tailSnakeListPosition.Add(_previosPosition);
            }

            yield return new WaitForSeconds(_delayForNextStepSnake);

            for (int i = 0; i < _tailSnake.Count; i++)
            {
                _tailSnake[i].GetComponent<BoxCollider2D>().enabled = true;
            }
        }
    }

    private void MovementSnake()
    {
        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) &&
            _direction != Vector2.right / _step)
        {
            _direction = Vector2.left / _step;
        }
        else if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) &&
            _direction != Vector2.left / _step)
        {
            _direction = Vector2.right / _step;
        }
        else if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) &&
            _direction != Vector2.down / _step)
        {
            _direction = Vector2.up / _step;
        }
        else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) &&
            _direction != Vector2.up / _step)
        {
            _direction = Vector2.down / _step;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var apple = other.GetComponent<Apple>();

        if (!apple)
        {
            GameManager.Instance.ActivateMenuForRestart();
        }
        if (apple)
        {
            GrowSnake();
            GameManager.Instance.IncreaseScore();

            if (_delayForNextStepSnake > 0.05f)
            {
                _delayForNextStepSnake -= 0.0015f;
            }
        }
    }

    private void GrowSnake()
    {
        Transform tail = Instantiate(_tailSnakePrefab, transform.position, Quaternion.identity);
        tail.transform.SetParent(_gameBoard.transform, false);

        tail.GetComponent<BoxCollider2D>().enabled = false;
        
       _tailSnake.Add(tail);
    }
}

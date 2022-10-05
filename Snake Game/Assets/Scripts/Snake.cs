using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    [SerializeField] private float _delayForNextStepSnake;
    [SerializeField] private Transform _tailSnakePrefab;
    [SerializeField] private RectTransform _gameBoard;
    [SerializeField] private List<Transform> _tailSnake;

    private Vector2 _direction;
    private Vector2 _previosPosition;

    private const int _step = 2;

    public List<Vector2> TailSnakeListPosition;


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
            _tailSnake[i].position = TailSnakeListPosition[i];
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

            if (TailSnakeListPosition.Count < _tailSnake.Count + 1)
            {
                TailSnakeListPosition.Add(_previosPosition);
            }
            else
            {
                TailSnakeListPosition.RemoveAt(0);
                TailSnakeListPosition.Add(_previosPosition);
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
        else
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

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
    private Vector2 _previousPosition;
    private bool _keyPressed;

    private const int Step = 2;
    private const float MaxSpeed = 0.05f;
    private const float StepForBoost = 0.0015f;

    public List<Vector2> TailSnakeListPosition;


    private void Start()
    {
        _direction = Vector2.right / Step;

        StartCoroutine(MovingSnake());
    }

    private void Update()
    {
        MovementSnake();
    }

    private IEnumerator MovingSnake()
    {

        while (_tailSnake.Count > 0)
        {
            while (GameManager.Instance.GameIsPaused)
            {
                yield return 0;
            }

            _keyPressed = false;

            transform.position = new Vector3(
                transform.position.x + _direction.x,
                transform.position.y + _direction.y,
                0);

            _previousPosition = transform.position;

            if (TailSnakeListPosition.Count < _tailSnake.Count + 1)
            {
                TailSnakeListPosition.Add(_previousPosition);
            }
            else
            {
                TailSnakeListPosition.RemoveAt(0);
                TailSnakeListPosition.Add(_previousPosition);
            }

            ChangePositionsForTails();

            yield return new WaitForSeconds(_delayForNextStepSnake);

            EnabledBoxCollider();
        }
    }

    private void ChangePositionsForTails()
    {
        for (int i = 0; i < _tailSnake.Count; i++)
        {
            _tailSnake[i].position = TailSnakeListPosition[i];
        }
    }

    private void EnabledBoxCollider()
    {
        for (int i = 0; i < _tailSnake.Count; i++)
        {
            _tailSnake[i].GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    private void MovementSnake()
    {
        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) &&
            _direction != Vector2.right / Step &&
            !_keyPressed)
        {
            _direction = Vector2.left / Step;
            _keyPressed = true;
        }
        else if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) &&
            _direction != Vector2.left / Step && 
            !_keyPressed)
        {
            _direction = Vector2.right / Step;
            _keyPressed = true;
        }
        else if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) &&
            _direction != Vector2.down / Step && 
            !_keyPressed)
        {
            _direction = Vector2.up / Step;
            _keyPressed = true;
        }
        else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) &&
            _direction != Vector2.up / Step && 
            !_keyPressed)
        {
            _direction = Vector2.down / Step;
            _keyPressed = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var apple = other.GetComponent<Apple>();

        if (!apple)
        {
            StopAllCoroutines();

            GameManager.Instance.GameOver();
        }
        else
        {
            GrowSnake();
            GameManager.Instance.IncreaseScore();

            if (_delayForNextStepSnake > MaxSpeed)
            {
                _delayForNextStepSnake -= StepForBoost;
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

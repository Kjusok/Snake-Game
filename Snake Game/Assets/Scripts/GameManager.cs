using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private List<float> _positionsY;
    private List<float> _positionsX;


    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        for (float i = -4.25f; i <= 4.25;)
        {
            _positionsY.Add(i);
            i += 0.5f;
        }
        for (float j = -8.25f; j <= 8.25f;)
        {
            _positionsX.Add(j);
            j += 0.5f;
        }

        SpawnAplle();
    }

    public void SpawnAplle()
    {
        var aplle = Instantiate(_applePrefub,
            new Vector2(_positionsX[Random.Range(0, _positionsX.Count)],
            _positionsY[Random.Range(0, _positionsY.Count)]),
            Quaternion.identity);
        aplle.transform.SetParent(_boardSpawn.transform, false);
    }
    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }
}

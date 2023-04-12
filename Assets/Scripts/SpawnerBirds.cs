using System.Collections.Generic;
using UnityEngine;

public class SpawnerBirds : MonoBehaviour
{
    [Header("Spawning Birds")]
    [SerializeField] public GameObject _birdPrefab;
    [SerializeField] private Transform _birdAnchor;
    [SerializeField] private int _amountBirds;
    [SerializeField] public float _spawnRadius;
    [SerializeField] private float _spawnDelay;

    [Header("Birds")]
    [SerializeField] public float _velocityBirds;
    [SerializeField] public float _neighborDistantion;
    [SerializeField] public float _collisionDistantion;
    [SerializeField] private float _velocityMatching;
    [SerializeField] private float _flockCentering;
    [SerializeField] private float _collisionAvoid;
    [SerializeField] public float _attractPull;
    [SerializeField] public float _attractPush;
    [SerializeField] public float _attractPushDistation;

    static public SpawnerBirds _spawnerBirds;
    static public List<Bird> _flockOfBirds;

    private void Awake()
    {
        _spawnerBirds = this;

        _flockOfBirds = new List<Bird>();
        InstantiateBirds();
    }

    public void InstantiateBirds()
    {
        GameObject _gameObject = Instantiate(_birdPrefab);
        Bird _bird = _gameObject.GetComponent<Bird>();

        _bird.transform.SetParent(_birdAnchor);
        _flockOfBirds.Add(_bird);

        if (_flockOfBirds.Count < _amountBirds)
        {
            Invoke("InstantiateBirds", _spawnDelay);
        }
    }


}

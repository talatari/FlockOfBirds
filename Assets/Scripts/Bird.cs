using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(TrailRenderer))]
public class Bird : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbodyBird;

    private Vector3 _startRandomVelocity;
    private Color _randomColorBird;
    private Renderer[] _renderers;
    private TrailRenderer _trailRenderer;
    private Vector3 _velocityBird;
    private Vector3 _deltaToAttractor;
    private Vector3 _velocityAttractor;
    private SpawnerBirds _spawnerBirds;
    private float _fixDeltaTime;

    private void Awake()
    {
        RandomStartBirds();

        LookHead();

        SetRandomColorBirds();
    }

    private void RandomStartBirds()
    {
        _rigidbodyBird = GetComponent<Rigidbody>();

        StartRandomPosition = Random.insideUnitSphere * SpawnerBirds._spawnerBirds._spawnRadius;
        _startRandomVelocity = Random.onUnitSphere * SpawnerBirds._spawnerBirds._velocityBirds;
        _rigidbodyBird.velocity = _startRandomVelocity;
    }

    private void LookHead()
    {
        transform.LookAt(StartRandomPosition + _rigidbodyBird.velocity);
    }

    private void SetRandomColorBirds()
    {
        _randomColorBird = Color.black;
        while (_randomColorBird.r + _randomColorBird.g + _randomColorBird.b < 1.0f)
        {
            _randomColorBird = new Color(Random.value, Random.value, Random.value);
        }

        _renderers = gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer _renderer in _renderers)
        {
            _renderer.material.color = _randomColorBird;
        }

        _trailRenderer = GetComponent<TrailRenderer>();
        _trailRenderer.material.SetColor("_TintColor", _randomColorBird);
    }

    public Vector3 StartRandomPosition
    {
        get
        {
            return transform.position;
        }

        set
        {
            transform.position = value;
        }
    }

    private void FixedUpdate()
    {
        _spawnerBirds = SpawnerBirds._spawnerBirds;
        _fixDeltaTime = Time.fixedDeltaTime;

        Attraction();

        ApplyVelocityChange();

        LookHead();
    }

    private void Attraction()
    {
        _velocityBird = _rigidbodyBird.velocity;
        _deltaToAttractor = Attractor._positionZero - StartRandomPosition;
        _velocityAttractor = _deltaToAttractor.normalized * _spawnerBirds._velocityBirds;
    }

    private void ApplyVelocityChange()
    {
        if (_deltaToAttractor.magnitude > _spawnerBirds._attractPushDistation)
        {
            _velocityBird = Vector3.Lerp(
                _velocityBird, _velocityAttractor, _spawnerBirds._attractPull * _fixDeltaTime);
        }
        else
        {
            _velocityBird = Vector3.Lerp(
                _velocityBird, -1 * _velocityAttractor, _spawnerBirds._attractPull * _fixDeltaTime);
        }

        _velocityBird = _velocityBird.normalized * _spawnerBirds._velocityBirds;
        _rigidbodyBird.velocity = _velocityBird;
    }


}

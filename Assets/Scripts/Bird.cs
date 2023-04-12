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

    private void Awake()
    {
        _rigidbodyBird = GetComponent<Rigidbody>();

        StartRandomPosition = Random.insideUnitSphere * SpawnerBirds._spawnerBirds._spawnRadius;
        _startRandomVelocity = Random.onUnitSphere * SpawnerBirds._spawnerBirds._velocityBirds;
        _rigidbodyBird.velocity = _startRandomVelocity;

        LookHead();

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

    private void LookHead()
    {
        transform.LookAt(StartRandomPosition + _rigidbodyBird.velocity);
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


}

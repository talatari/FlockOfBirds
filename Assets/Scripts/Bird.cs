using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(TrailRenderer))]
[RequireComponent(typeof(Neighborhood))]
public class Bird : MonoBehaviour
{
    [HideInInspector] public Rigidbody _rigidbodyBird;

    private Vector3 _startRandomVelocity;
    private Color _randomColorBird;
    private Renderer[] _renderers;
    private TrailRenderer _trailRenderer;
    private Vector3 _velocityBird;
    private Vector3 _deltaToAttractor;
    private Vector3 _velocityAttractor;
    private SpawnerBirds _spawnerBirds;
    private float _fixDeltaTime;
    private Neighborhood _neighborhood;
    private Vector3 _velocityAvoid;
    private Vector3 _tooClosePosition;
    private Vector3 _velocityAlign;
    private Vector3 _velocityToCenterNeighborhood;
    private Vector3 _zero = Vector3.zero;

    private void Awake()
    {
        RandomStartBirds();

        LookHead();

        SetRandomColorBirds();
    }

    private void RandomStartBirds()
    {
        _neighborhood = GetComponent<Neighborhood>();
        _rigidbodyBird = GetComponent<Rigidbody>();

        PositionBird = Random.insideUnitSphere * SpawnerBirds._spawnerBirds._spawnRadius;
        _startRandomVelocity = Random.onUnitSphere * SpawnerBirds._spawnerBirds._velocityBirds;
        _rigidbodyBird.velocity = _startRandomVelocity;
    }

    private void LookHead()
    {
        transform.LookAt(PositionBird + _rigidbodyBird.velocity);
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

    public Vector3 PositionBird
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

        CollisionPrevention();

        VelocityAgreement();

        ConcentrationOfNeighbors();

        Attraction();

        ApplyVelocityChange();

        LookHead();
    }

    private void CollisionPrevention()
    {
        _velocityAvoid = _zero;
        _tooClosePosition = _neighborhood.AvgClosePositionBirds;

        if (_tooClosePosition != _zero)
        {
            _velocityAvoid = PositionBird - _tooClosePosition;
            _velocityAvoid.Normalize();
            _velocityAvoid *= _spawnerBirds._velocityBirds;
        }
    }

    private void VelocityAgreement()
    {
        _velocityAlign = _neighborhood.AvgVelocityBirds;

        if (_velocityAlign != _zero)
        {
            _velocityAlign.Normalize();
            _velocityAlign *= _spawnerBirds._velocityBirds;
        }
    }

    private void ConcentrationOfNeighbors()
    {
        _velocityToCenterNeighborhood = _neighborhood.AvgPositionBirds;

        if (_velocityToCenterNeighborhood != _zero)
        {
            _velocityToCenterNeighborhood -= transform.position;
            _velocityToCenterNeighborhood.Normalize();
            _velocityToCenterNeighborhood *= _spawnerBirds._velocityBirds;
        }
    }

    private void Attraction()
    {
        _velocityBird = _rigidbodyBird.velocity;
        _deltaToAttractor = Attractor._positionZero - PositionBird;
        _velocityAttractor = _deltaToAttractor.normalized * _spawnerBirds._velocityBirds;
    }

    private void ApplyVelocityChange()
    {
        if (_velocityAvoid != _zero)
        {
            _velocityBird = Vector3.Lerp(
                _velocityBird, _velocityAvoid, _spawnerBirds._collisionAvoid * _fixDeltaTime);
        }
        else
        {
            if (_velocityAlign != _zero)
            {
                _velocityBird = Vector3.Lerp(
                    _velocityBird, _velocityAlign, _spawnerBirds._velocityMatching * _fixDeltaTime);
            }

            if (_velocityToCenterNeighborhood != _zero)
            {
                _velocityBird = Vector3.Lerp(
                    _velocityBird, _velocityAlign, _spawnerBirds._flockCentering * _fixDeltaTime);
            }

            if (_velocityAttractor != _zero)
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
            }
        }

        _velocityBird = _velocityBird.normalized * _spawnerBirds._velocityBirds;
        _rigidbodyBird.velocity = _velocityBird;
    }


}

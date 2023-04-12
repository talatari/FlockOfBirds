using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Bird))]
public class Neighborhood : MonoBehaviour
{
    public List<Bird> _neighboringBirds;
    private SphereCollider _colliderBird;
    private SpawnerBirds _spawnerBirds;

    private void Awake()
    {
        _spawnerBirds = SpawnerBirds._spawnerBirds;
    }

    private void Start()
    {
        _neighboringBirds = new List<Bird>();
        _colliderBird = GetComponent<SphereCollider>();
        _colliderBird.radius = _spawnerBirds._neighborDistantion / 2;
    }

    private void FixedUpdate()
    {
        if (_colliderBird.radius != _spawnerBirds._neighborDistantion / 2)
        {
            _colliderBird.radius = _spawnerBirds._neighborDistantion / 2;
        }
    }

    private void OnTriggerEnter(Collider _other)
    {
        Bird _bird = _other.GetComponent<Bird>();

        if (_bird != null)
        {
            if (_neighboringBirds.IndexOf(_bird) == -1)
            {
                _neighboringBirds.Add(_bird);
            }
        }
    }

    private void OnTriggerExit(Collider _other)
    {
        Bird _bird = _other.GetComponent<Bird>();

        if (_bird != null)
        {
            if (_neighboringBirds.IndexOf(_bird) != -1)
            {
                _neighboringBirds.Remove(_bird);
            }
        }
    }

    public Vector3 AvgPositionBirds
    {
        get
        {
            Vector3 _avg = Vector3.zero;

            if (_neighboringBirds.Count == 0)
            {
                return _avg;
            }

            for (int i = 0; i < _neighboringBirds.Count; i++)
            {
                _avg += _neighboringBirds[i].PositionBird;
            }

            _avg /= _neighboringBirds.Count;

            return _avg;
        }
    }

    public Vector3 AvgVelocityBirds
    {
        get
        {
            Vector3 _avg = Vector3.zero;

            if (_neighboringBirds.Count == 0)
            {
                return _avg;
            }

            for (int i = 0; i < _neighboringBirds.Count; i++)
            {
                _avg += _neighboringBirds[i]._rigidbodyBird.velocity;
            }

            _avg /= _neighboringBirds.Count;

            return _avg;
        }
    }

    public Vector3 AvgClosePositionBirds
    {
        get
        {
            Vector3 _avg = Vector3.zero;
            Vector3 _delta;
            int _nearCount = 0;

            for (int i = 0; i < _neighboringBirds.Count; i++)
            {
                _delta = _neighboringBirds[i].PositionBird - transform.position;

                if (_delta.magnitude <= _spawnerBirds._collisionDistantion)
                {
                    _avg += _neighboringBirds[i].PositionBird;
                    _nearCount++;
                }
            }

            if (_nearCount == 0)
            {
                return _avg;
            }

            _avg /= _nearCount;

            return _avg;
        }
    }


}

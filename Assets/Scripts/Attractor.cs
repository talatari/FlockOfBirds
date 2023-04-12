using UnityEngine;

public class Attractor : MonoBehaviour
{
    [SerializeField] private float _radiusAttractor;
    [SerializeField] private float _xPhase;
    [SerializeField] private float _yPhase;
    [SerializeField] private float _zPhase;

    static public Vector3 _positionZero = Vector3.zero;
    private Vector3 _tempPosition;
    private Vector3 _scale;

    private void FixedUpdate()
    {
        _tempPosition = Vector3.zero;
        _scale = transform.localScale;

        _tempPosition.x = Mathf.Sin(_xPhase * Time.time) * _radiusAttractor * _scale.x;
        _tempPosition.y = Mathf.Sin(_yPhase * Time.time) * _radiusAttractor * _scale.y;
        _tempPosition.z = Mathf.Sin(_zPhase * Time.time) * _radiusAttractor * _scale.z;

        transform.position = _tempPosition;
        _positionZero = _tempPosition;
    }


}

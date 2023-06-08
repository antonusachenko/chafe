using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCam : MonoBehaviour
{
    [SerializeField] private GameObject _target;
    [SerializeField] private float _lerpSpeed;

    [SerializeField] private ChunkHandler _currentChunk;
    [SerializeField] private Vector3 _enterPointDirection;
    [SerializeField] private Vector3 _exitPointDirection;
    [SerializeField] private float _distanceBetweenPoints;

    [SerializeField] private float _distanceBetweenPointsNormalized;

    void Start()
    {
        
    }

    private void Update()
    {
        if (_currentChunk != null)
        {
            _distanceBetweenPointsNormalized = Vector3.Distance(_enterPointDirection, transform.position) / _distanceBetweenPoints;
            //Debug.Log("distance N: " + _distanceBetweenPointsNormalized + " of " + _currentChunk.transform.parent.name);
        }
    }
    void LateUpdate()
    {
        if(_target != null)
        {
            transform.position = Vector3.Lerp(transform.position, _target.transform.position, Time.deltaTime * _lerpSpeed); 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var chunk = other.GetComponentInParent<ChunkHandler>();
        if (chunk != null && _currentChunk != chunk)
        {
            //Debug.Log(other.transform.parent.name + " collided");

            _currentChunk = chunk;

            _enterPointDirection = _currentChunk.enter.transform.forward;
            _exitPointDirection = _currentChunk.exit.transform.forward;

            var pos1 = _currentChunk.enter.transform.position;
            var pos2 = _currentChunk.exit.transform.position;

            _distanceBetweenPoints = Vector3.Distance(pos1, pos2);

            //Debug.Log("distance: " + _distanceBetweenPoints);
        }
    }
}

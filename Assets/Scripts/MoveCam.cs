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
        _distanceBetweenPointsNormalized = Vector3.Distance(_enterPointDirection, transform.position) / _distanceBetweenPoints;
        Debug.Log("distance N: " + _distanceBetweenPointsNormalized);
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
        //Debug.Log("cam found a some trigger");
        var chunk = other.GetComponentInParent<ChunkHandler>();
        if (chunk != null)
        {
            //Debug.Log("cam found a chunk enter trigger");
            _currentChunk = chunk;

            _enterPointDirection = _currentChunk.enter.transform.forward;
            _exitPointDirection = _currentChunk.exit.transform.forward;

            _distanceBetweenPoints = Vector3.Distance(
                _currentChunk.enter.transform.position, _currentChunk.enter.transform.position);
            Debug.Log("distance: " + _distanceBetweenPoints);
        }
    }
}

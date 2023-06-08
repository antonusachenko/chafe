using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event EventHandler OnCurrentChunkUpdated;

    public event EventHandler<OnGetNewTargetEventArgs> OnGetNewTarget;
    public class OnGetNewTargetEventArgs : EventArgs
    {
        public Vector3 target;
    }


    [SerializeField] private NavMeshSurface _NMS;

    [SerializeField] private ChunkHandler _currentChunk;
    [SerializeField] private ChunkHandler[] _chunkList;
    private Transform _newEnterPoint;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameManager.Instance.OnCurrentChunkUpdated += OnCurrentChunkUpdated_BuildNewChunk;
    }

    public void SetChunk(ChunkHandler newChunk)
    {
        if (newChunk != null && _currentChunk != newChunk)
        {
            _currentChunk = newChunk;
            OnCurrentChunkUpdated?.Invoke(this, EventArgs.Empty);
            Debug.Log($"GAME MANAGER: chunk updated to ({_currentChunk.transform.name})");
        }
    }

    private void OnCurrentChunkUpdated_BuildNewChunk(object sender, EventArgs e)
    {
        //Get enter point
        _newEnterPoint = _currentChunk.exit.transform;

        //Try to spawn new chunk
        var randomChunk = _chunkList[UnityEngine.Random.Range(0, _chunkList.Length)];
        var nextChunk = Instantiate(randomChunk, _newEnterPoint);
        nextChunk.transform.SetParent(null);
        Debug.Log($"GAME MANAGER: New chunk was generate, it's named ({nextChunk.gameObject.name})");

        //If new chunk spawned correctly - rebuild navmesh surface and get new target point
        if (nextChunk != null)
        {
            //bake new NavMeshSurface
            _NMS.BuildNavMesh();

            //get next chunk data
            //var target = nextChunk.player_target_point.position;
            var target = nextChunk.player_start_point.position;
            Debug.Log($"GAME MANAGER: New target point ({target})");

            OnGetNewTarget?.Invoke(this, new OnGetNewTargetEventArgs
            {
                target = target
            }); 
        }
    }

}

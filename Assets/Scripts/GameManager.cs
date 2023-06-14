using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using static GameManager;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event EventHandler<OnCurrentChunkUpdatedEventArgs> OnCurrentChunkUpdated;
    public class OnCurrentChunkUpdatedEventArgs : EventArgs
    {
        public ChunkHandler chunk;
    }

    public event EventHandler<OnGetNewTargetEventArgs> OnGetNewTarget;

    public class OnGetNewTargetEventArgs : EventArgs
    {
        public Vector3 target;
    }

    [SerializeField] private bool GENERATE_NEW_NMDATA;

    [SerializeField] private NavMeshSurface _navMeshSurface;

    [SerializeField] private ChunkHandler _currentChunk;
    [SerializeField] private ChunkHandler[] _chunkList;

    private List<ChunkHandler> _spawnedChunks;

    private Transform _newEnterPoint;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameManager.Instance.OnCurrentChunkUpdated += OnCurrentChunkUpdated_BuildNewChunk;
        //spawn first chunk
    }

    private void OnCurrentChunkUpdated_BuildNewChunk(object sender, EventArgs e)
    {
        //Get enter point
        _newEnterPoint = _currentChunk.exit.transform;

        //Try to spawn new chunk
        var randomChunk = _chunkList[UnityEngine.Random.Range(0, _chunkList.Length)];
        var nextChunk = Instantiate(randomChunk, _newEnterPoint);
        nextChunk.transform.SetParent(null);
        if(_spawnedChunks.Count >= 3){
            _spawnedChunks.First().SelfDestroy();
            _spawnedChunks.Add(nextChunk);
        }
        else
        {
            _spawnedChunks.Add(nextChunk);
        }

        Debug.Log($"GAME MANAGER: New chunk was generate, it's named ({nextChunk.gameObject.name})");

        var data = InitializeBakeData(_navMeshSurface);
        //Get NavMesh data from all chunks
        for (int i=0; i < _spawnedChunks.Count; i++)
        {

        }
        
    }

    private void AddNavMeshData(NavMeshSurface surface, NavMeshData newData)
    {
        //_navMeshDataInstance = NavMesh.AddNavMeshData(newData);
        //surface.navMeshData = _navMeshDataInstance;
    }

    private NavMeshData GetNavMeshDataFromPrefab(ChunkHandler chunk)
    {
        return chunk.navMeshSurface.navMeshData;
    }

    public void EnteredTheChunk(ChunkHandler newChunk)
    {
        if (newChunk != null && _currentChunk != newChunk)
        {
            _currentChunk = newChunk;
            OnCurrentChunkUpdated?.Invoke(this, new OnCurrentChunkUpdatedEventArgs { chunk = newChunk});
            Debug.Log($"GAME MANAGER: chunk updated to ({_currentChunk.transform.name})");
        }
    }

    static NavMeshData InitializeBakeData(NavMeshSurface surface)
    {
        var emptySources = new List<NavMeshBuildSource>();
        var emptyBounds = new Bounds();

        return UnityEngine.AI.NavMeshBuilder.BuildNavMeshData(surface.GetBuildSettings(), emptySources, emptyBounds, surface.transform.position, surface.transform.rotation);
    }
}








////If new chunk spawned correctly - rebuild navmesh surface and get new target point
//if (nextChunk != null)
//{
//    //bake new NavMeshSurface
//    //_NMS.BuildNavMesh();
//    StartCoroutine(BuildNavMesh(_navMeshSurface));

//    //get next chunk data
//    //var target = nextChunk.player_target_point.position;
//    //var target = nextChunk.player_start_point.position;
//    //Debug.Log($"GAME MANAGER: New target point ({target})");

//    //OnGetNewTarget?.Invoke(this, new OnGetNewTargetEventArgs
//    //{
//    //    target = target
//    //}); 
//}
//else if (nextChunk != null)
//{

//}

//// called by startcoroutine whenever you want to build the navmesh
//IEnumerator BuildNavMesh(NavMeshSurface surface)
//{
//    // get the data for the surface
//    var data = InitializeBakeData(surface);

//    // start building the navmesh
//    var async = surface.UpdateNavMesh(data);

//    // wait until the navmesh has finished baking
//    yield return async;

//    Debug.Log("NavMesh baking (updating) is finished");

//    surface.RemoveData();

//    // you need to save the baked data back into the surface
//    surface.navMeshData = data;

//    // call AddData() to finalize it
//    surface.AddData();

//    var target = nextChunk.player_start_point.position;
//    Debug.Log($"GAME MANAGER: New target point ({target})");

//    OnGetNewTarget?.Invoke(this, new OnGetNewTargetEventArgs
//    {
//        target = target
//    });
//}

//// creates the navmesh data
//static NavMeshData InitializeBakeData(NavMeshSurface surface)
//{
//    var emptySources = new List<NavMeshBuildSource>();
//    var emptyBounds = new Bounds();

//    return UnityEngine.AI.NavMeshBuilder.BuildNavMeshData(surface.GetBuildSettings(), emptySources, emptyBounds, surface.transform.position, surface.transform.rotation);
//}

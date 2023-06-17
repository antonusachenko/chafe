using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private Camera _cam;

    [SerializeField] private NavMeshAgent _agent;

    [SerializeField] private ThirdPersonCharacter _character;

    [SerializeField] private Vector3 _target;

    //private bool _navMeshReady;


    private void Awake()
    {
        _agent.enabled = false;
    }

    void Start()
    {
        //Subscribe
        GameManager.Instance.OnGetNewTarget += GM_OnGetNewTarget;

        //Setup
        _agent.updateRotation = false;

        //Start run
        
    }

    void Update()
    {
        if (!_agent.isActiveAndEnabled)
        {
            _character.Move(new Vector3(0f, 0f, 1f), false, false);
        }

        if (_agent.isActiveAndEnabled && Input.GetMouseButtonDown(0))
        {
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                _agent.SetDestination(hit.point);
            }
        }

        if (_agent.isActiveAndEnabled && _agent.remainingDistance > _agent.stoppingDistance)
        {
            _character.Move(_agent.desiredVelocity, false, false);
        }
        else
        {
            _character.Move(Vector3.zero, false, false);
        }
    }

    private void GM_OnGetNewTarget(object sender, GameManager.OnGetNewTargetEventArgs e)
    {
        _agent.SetDestination(e.target);
        Debug.Log($"PLAYER: Aprove target point {e.target}");
    }

    private void OnTriggerEnter(Collider trigger)
    {
        Debug.Log($"THE PLAYER ENTERED THE TRIGGER ({trigger.gameObject.name} of {trigger.gameObject.transform.parent.name})");

        if (trigger.transform.parent.TryGetComponent(out ChunkHandler chunk))
        {
            _agent.enabled = true;
            Debug.Log($"PLAYER: NavMesh Agent was enabled");
            GameManager.Instance.EnteredTheChunk(chunk);
        }
    }
}
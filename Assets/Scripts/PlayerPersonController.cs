using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private Camera _cam;

    [SerializeField] private NavMeshAgent _agent;

    [SerializeField] private ThirdPersonCharacter _character;

    [SerializeField] private Vector3 _target;

    public Vector3 characterDirection;

    private Vector3 _analogTarget;

    private bool _analogCharacterControlEnabled;

    private bool _agentDestinationCorrect;


    private void Awake()
    {
        EnableAnalogCharacterControl();
        characterDirection = new Vector3(0, 0, 5f);
    }

    void Start()
    {
        //Subscribe
        GameManager.Instance.OnGetNewTarget += GM_OnGetNewTarget;

        //Setup
        _agent.updateRotation = false;
    }

    void Update()
    {
        if (_analogCharacterControlEnabled)
        {
            //moving by direction
            _character.Move(characterDirection, false, false);
        }
        else if(_agent.isActiveAndEnabled)
        {
            Debug.Log($"PLAYER: _agent.remainingDistance {_agent.remainingDistance}");

            //moving by navmesh
            if (_agent.remainingDistance > _agent.stoppingDistance)
            {
                _character.Move(_agent.desiredVelocity, false, false);
            }
            else
            {
                //destination was get
                Debug.Log($"PLAYER: Destination was get !!!");
                EnableAnalogCharacterControl();
                characterDirection = (_analogTarget - transform.position).normalized;
            }

            //click control
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    _agent.SetDestination(hit.point);
                }
            }
        }
        else
        {
            _character.Move(Vector3.zero, false, false);
        }

        //if (Input.GetMouseButtonDown(0) && !_analogCharacterControlEnabled )
        //{
        //    Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hit;

        //    if (Physics.Raycast(ray, out hit))
        //    {
        //        _agent.SetDestination(hit.point);
        //    }
        //}

        //if (!_analogCharacterControlEnabled && _agent.remainingDistance > _agent.stoppingDistance)
        //{
        //    _character.Move(_agent.desiredVelocity, false, false);
        //}
        //else
        //{
        //    _character.Move(Vector3.zero, false, false);
        //}
    }

    private void GM_OnGetNewTarget(object sender, GameManager.OnGetNewTargetEventArgs e)
    {
        SetAgentDestination(e.chunkTarget);
        _analogTarget = e.analogTarget;

        //Debug.Log($"PLAYER: Aprove chunk target point {e.chunkTarget} and next target point {e.analogTarget}");
    }

    private void SetAgentDestination(Vector3 target)
    {
        _agentDestinationCorrect = _agent.SetDestination(target);
        if (_agentDestinationCorrect)
        {
            EnableNavMeshCharacterControl();
            Debug.Log($"PLAYER: Aprove chunk target point {target} ");
        }
        else
        {
            //SetAgentDestination(target);
        }
    }

    private void OnTriggerEnter(Collider trigger)
    {
        Debug.Log($"THE PLAYER ENTERED THE TRIGGER ({trigger.gameObject.name} of {trigger.gameObject.transform.parent.name})");

        if (trigger.transform.parent.TryGetComponent(out ChunkHandler chunk))
        {
            EnableNavMeshCharacterControl();
            GameManager.Instance.EnteredTheChunk(chunk);
        }
    }

    private void EnableAnalogCharacterControl()
    {

        Debug.Log($"PLAYER: NavMesh Agent was disabled");
        _analogCharacterControlEnabled = true;
        _agent.enabled = false;
    }

    private void EnableNavMeshCharacterControl()
    {

        Debug.Log($"PLAYER: NavMesh Agent was enabled");
        _analogCharacterControlEnabled = false;
        _agent.enabled = true;
    }
}
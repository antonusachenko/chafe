using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : MonoBehaviour
{
    public enum PlayerState { ANALOG_RUN, NM_RUN, WAITING4PATH, NMRUN_REACHED, DEATH };
    public PlayerState state;

    [SerializeField] private Camera _cam;

    [SerializeField] private NavMeshAgent _agent;

    [SerializeField] private ThirdPersonCharacter _character;

    [SerializeField] private Vector3 _target;

    [SerializeField] private float _timerToSwitch4AnalogRun = 1f;

    public Vector3 characterDirection;

    private Vector3 _navmeshTarget;
    private Vector3 _analogTarget;

    private bool _agentDestinationCorrect;



    private void Awake()
    {
        _agent.enabled = false;
        characterDirection = new Vector3(0, 0, 5f);
    }

    void Start()
    {
        //Subscribe
        GameManager.Instance.OnGetNewTarget += GM_OnGetNewTarget;

        //Setup
        _agent.updateRotation = false;
        state = PlayerState.ANALOG_RUN;
    }

    void Update()
    {

        switch (state)
        {
            case PlayerState.ANALOG_RUN:
                //code
                Debug.Log($"PLAYER: ANALOG_RUN ");
                
                _character.Move(characterDirection, false, false);

                break;

            case PlayerState.NM_RUN:
                //code
                Debug.Log($"PLAYER: NM_RUN ");

                // click control
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit))
                    {
                        _agent.SetDestination(hit.point);
                    }
                }

                // NavMesh
                if (_agent.remainingDistance > _agent.stoppingDistance)
                {
                    _character.Move(_agent.desiredVelocity, false, false);
                }
                else
                {
                    _character.Move(Vector3.zero, false, false);

                    if (_agent.velocity.magnitude < _timerToSwitch4AnalogRun)
                    {
                        state = PlayerState.NMRUN_REACHED;
                        _agent.enabled = false;
                    }
                }

                break;

            case PlayerState.WAITING4PATH:
                //code
                Debug.Log($"PLAYER: WAITING4PATH ");

                if (_agent.isActiveAndEnabled && _agentDestinationCorrect)
                {
                    state = PlayerState.NM_RUN;
                }

                break;

            case PlayerState.NMRUN_REACHED:
                //code
                Debug.Log($"PLAYER: NMRUN_REACHED ");

                characterDirection = (_analogTarget - transform.position).normalized;
                state = PlayerState.ANALOG_RUN;

                break;

            case PlayerState.DEATH:
                //code
                break;

            default:
                //code
                _character.Move(Vector3.zero, false, false);
                Debug.LogWarning($"PLAYER: default state ? ");
                break;
        }



        
    }

    private void GM_OnGetNewTarget(object sender, GameManager.OnGetNewTargetEventArgs e)
    {
        _navmeshTarget = e.chunkTarget;
        _analogTarget = e.analogTarget;
        _agent.enabled = true;
        _agentDestinationCorrect = _agent.SetDestination(_navmeshTarget);
        if (_agentDestinationCorrect)
        {
            state = PlayerState.WAITING4PATH;
            Debug.Log($"PLAYER: Aprove chunk target point {_navmeshTarget} ");
        }
        else
        {
            Debug.LogError($"PLAYER: NAVMESH TARGET SET FAILED");
        }

        //Debug.Log($"PLAYER: Aprove chunk target point {e.chunkTarget} and next target point {e.analogTarget}");
    }

    private void OnTriggerEnter(Collider trigger)
    {
        Debug.Log($"THE PLAYER ENTERED THE TRIGGER ({trigger.gameObject.name} of {trigger.gameObject.transform.parent.name})");

        if (trigger.transform.parent.TryGetComponent(out ChunkHandler chunk))
        {
            GameManager.Instance.EnteredTheChunk(chunk);
        }
    }

}
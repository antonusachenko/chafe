using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class EnemyPersonController : MonoBehaviour
{
    public enum EnemyState { ANALOG_RUN, NM_RUN, WAITING_PATH, WAITING_START, NMRUN_REACHED, DEATH };
    [HideInInspector] public EnemyState moveState;

    public enum EnemyAttackState { WAIT, AIM, SHOOTING};
    [HideInInspector] public EnemyAttackState attackState;

    [Header("Links")]

    [SerializeField] private NavMeshAgent _agent;

    [Header("Movement")]

    [SerializeField] private ThirdPersonCharacter _character;

    [SerializeField] private bool _haveStartTarget;
    [SerializeField] private Transform _startTarget;

    [SerializeField] private float _minSpeedMagnitudeToSwitchMoveState = 1f;

    private Vector3 characterDirection;

    private Vector3 _navmeshTarget;
    private Vector3 _analogTarget;

    private bool _timeToStart;

    [Header("Health")]

    [SerializeField] private float _hp;

    private EnemyPersonController()
    {
        // need subscribe to parent chunk
    }

    private void Awake()
    {
        _agent.enabled = true;
        _timeToStart = true; // for testing
        characterDirection = new Vector3(0, 0, 0f);

        _navmeshTarget = _startTarget.position;
        _startTarget.GetChild(0).gameObject.SetActive(false);
    }

    void Start()
    {
        //Subscribe
        //GameManager.Instance.OnGetNewTarget += GM_OnGetNewTarget; 

        //Setup
        _agent.updateRotation = false;
        _agent.SetDestination(_navmeshTarget);
        if (_haveStartTarget)
        {
            moveState = EnemyState.WAITING_START;
        }
        else
        {
            moveState = EnemyState.WAITING_PATH;
        }

        _hp = 100;
    }

    void Update()
    {
        MoveStateUpdate();

    }


    //private void OnTriggerEnter(Collider trigger)
    //{
    //    Debug.Log($"THE PLAYER ENTERED THE TRIGGER ({trigger.gameObject.name} of {trigger.gameObject.transform.parent.name})");

    //    if (trigger.transform.parent.TryGetComponent(out ChunkHandler chunk))
    //    {
    //        GameManager.Instance.EnteredTheChunk(chunk);
    //    }
    //}

    private void MoveStateUpdate()
    {
        switch (moveState)
        {
            case EnemyState.ANALOG_RUN:
                //code
                Debug.Log($"ENEMY{gameObject.name}: ANALOG_RUN ");

                _character.Move(characterDirection, false, false);
                //maybe add timer? after then set state to waiting4path

                break;

            case EnemyState.NM_RUN:
                //code
                Debug.Log($"ENEMY{gameObject.name}: NM_RUN ");

                // NavMesh
                if (_agent.remainingDistance > _agent.stoppingDistance)
                {
                    _character.Move(_agent.desiredVelocity, false, false);
                }
                else
                {
                    _character.Move(Vector3.zero, false, false);

                    if (_agent.velocity.magnitude < _minSpeedMagnitudeToSwitchMoveState)
                    {
                        moveState = EnemyState.NMRUN_REACHED;
                        //_agent.enabled = false;
                    }
                }

                break;

            case EnemyState.WAITING_PATH:
                //code
                Debug.Log($"ENEMY{gameObject.name}: WAITING_PATH ");

                _character.Move(Vector3.zero, false, false);

                if (_agent.isActiveAndEnabled)
                {
                    //code to set random target in random time
                    //kinda _agent.FindNearbyPoint
                    //if was set then:
                    //state = EnemyState.NM_RUN;
                }
                else
                {
                    Debug.LogError($"ENEMY{gameObject.name}: NAVMESH DESTINATION IS UNCORRECT");
                }

                break;

            case EnemyState.WAITING_START:
                //code
                Debug.Log($"ENEMY{gameObject.name}: WAITING_START ");

                if (_timeToStart)
                    moveState = EnemyState.NM_RUN;

                break;

            case EnemyState.NMRUN_REACHED:
                //code
                Debug.Log($"ENEMY{gameObject.name}: NMRUN_REACHED ");

                //characterDirection = (_analogTarget - transform.position).normalized;
                moveState = EnemyState.WAITING_PATH;

                break;

            case EnemyState.DEATH:
                //code
                break;

            default:
                //code
                _character.Move(Vector3.zero, false, false);
                Debug.LogWarning($"ENEMY{gameObject.name}: default state ??? ");
                break;
        }
    }

    public void SetDamage(float value)
    {
        if (value > 0)
        {
            _hp -= value;

            if (_hp < 0)
            {
                _hp = 0;
                Debug.LogWarning($"ENEMY{ gameObject.name} was killed");
                //code to change state to DEATH
            }
        }
    }

    public float GetHP()
    {
        return _hp;
    }
}

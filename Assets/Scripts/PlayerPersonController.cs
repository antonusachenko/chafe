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
    

    void Start()
    {
        //Subscribe
        GameManager.Instance.OnGetNewTarget += GM_OnGetNewTarget;

        //Setup
        _agent.updateRotation = false;

        //Start run
        _agent.SetDestination(_target);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                _agent.SetDestination(hit.point);
            }
        }

        if (_agent.remainingDistance > _agent.stoppingDistance)
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
        //if (!_navMeshReady)
        //    _navMeshReady = true;

        _agent.SetDestination(e.target);
        Debug.Log($"PLAYER: Aprove target point {e.target}");
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
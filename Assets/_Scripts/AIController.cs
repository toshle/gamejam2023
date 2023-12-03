using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.GlobalIllumination;

public class AIController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private float _startWaitTime = 4f;
    [SerializeField] private float _rotationTime = 2f;
    [SerializeField] private float _walkSpeed = 6f;
    [SerializeField] private float _runSpeed = 9f;

    [SerializeField] private float _viewRadius = 15f;
    [SerializeField] private float _viewAngle = 90f;
    [SerializeField] private LayerMask _playerMask;
    [SerializeField] private LayerMask _obstacleMask;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private float _meshResoulution = 1f;
    [SerializeField] private int _edgeIterations = 4;
    [SerializeField] private float _edgeDistance = 0.5f;

    [SerializeField] private Transform[] _waypoints;
    [SerializeField] private int _currentWaypointIndex;

    [SerializeField] private Vector3 _playerLastPosition = Vector3.zero;
    [SerializeField] private Vector3 _playerPosition;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Light _light;
    [SerializeField] private Color _detectedLightColor;
    [SerializeField] private Color _alertLightColor;
    [SerializeField] private Color _lightColor;
    [SerializeField] private Animator _animator;

    float _waitTime, _timeToRotate;
    bool _playerInRange, _playerNear, _isPatrol, _caughtPlayer;


    bool _goTolastPlayerPosition = false;
    bool _chasePlayer = false;
    [SerializeField] public bool Activated = false;
    bool _isPatroling = false;

    void Start()
    {
        /*_playerPosition = Vector3.zero;
        _caughtPlayer = false;
        _playerInRange = false;*/
        _waitTime = _startWaitTime;
        _timeToRotate = _rotationTime;

        _currentWaypointIndex = 0;

        _isPatroling = true;
        _agent.isStopped = false;
        _agent.speed = _walkSpeed;
        Activated = true;
        _agent.SetDestination(_waypoints[_currentWaypointIndex].position);
    }

    private void Update()
    {
        if (Activated)
        {
            if (_isPatroling)
            {
                Patrol();
            }
            if (_chasePlayer)
            {
                Chase();
            }
            if (_goTolastPlayerPosition)
            {
                Move(_walkSpeed);
                _agent.SetDestination(_playerLastPosition);
                if (_agent.remainingDistance <= _agent.stoppingDistance)
                {
                    Debug.Log("Checked last position, going back to patrol.");
                    _goTolastPlayerPosition = false;
                    _isPatroling = true;
                    _light.color = _lightColor;
                }
            }
        }
    }

    public void CaughtPlayer()
    {
        _caughtPlayer = true;
    }

    private void Move(float speed)
    {
        _agent.isStopped = false;
        _agent.speed = speed;
        _animator.SetBool("Moving", true);
    }

    private void Stop()
    {
        _agent.isStopped = true;
        //_agent.speed = 0;
    }

    public void NextPoint()
    {
        _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
        _agent.SetDestination(_waypoints[_currentWaypointIndex].position);
    }

    private void Patrol()
    {
        _animator.SetBool("Running", false);
        _animator.SetBool("Moving", true);
        _agent.isStopped = false;
        _playerLastPosition = Vector3.zero;
        _agent.SetDestination(_waypoints[_currentWaypointIndex].position);
        if(_agent.remainingDistance <= _agent.stoppingDistance)
        {
            if(_waitTime <= 0)
            {
                NextPoint();
                Move(_walkSpeed);
                _waitTime = _startWaitTime;
            } else
            {
                Stop();
                _animator.SetBool("Moving", false);
                _waitTime -= Time.deltaTime * 10;
            }
        }
    }

/*    private void OnCollisionEnter(Collision collision)
    {
        collision.
    }*/

    private void Chase()
    {
        _animator.SetBool("Moving", true);
        _animator.SetBool("Running", true);
        _agent.isStopped = false;
        Move(_runSpeed);
        _agent.SetDestination(_playerController.transform.position);

        if(_agent.remainingDistance <= _agent.stoppingDistance)
        {
            Debug.Log("Caught!");
            //GameManager.Instance.UpdateGameState(GameManager.GameState.Lose);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform player = other.transform;
        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            _chasePlayer = true;
            _isPatroling = false;
            _playerLastPosition = player.position;
            _light.color = _detectedLightColor;
            Debug.Log("I see you!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("I DO NOT see you anymoe!");
        Transform player = other.transform;
        //Vector3 dirToPlayer = (player.position - transform.position).normalized;
        //float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            _animator.SetBool("Running", false);
            _light.color = _alertLightColor;
            _chasePlayer = false;
            _isPatroling = false;
            _goTolastPlayerPosition = true;
            _playerLastPosition = player.position;
            Debug.Log("PLAYER LEFT");
        }
        else
        {
            //Debug.Log("NOT PLAYER");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            GameManager.Instance.UpdateGameState(GameManager.GameState.Lose);
        }

    }
}

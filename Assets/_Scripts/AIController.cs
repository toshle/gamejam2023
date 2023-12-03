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

    float _waitTime, _timeToRotate;
    bool _playerInRange, _playerNear, _isPatrol, _caughtPlayer;


    bool _goTolastPlayerPosition = false;
    bool _chasePlayer = false;
    [SerializeField] bool _activated = false;
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
        /*_activated = true;
        _agent.SetDestination(_waypoints[_currentWaypointIndex].position);*/
    }

    private void Update()
    {
        if (_activated)
        {
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
            if (_isPatroling)
            {
                Patrol();
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
    }

    private void Stop()
    {
        _agent.isStopped = true;
        _agent.speed = 0;
    }

    public void NextPoint()
    {
        _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
        _agent.SetDestination(_waypoints[_currentWaypointIndex].position);
    }

    private void Patrol()
    {
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
                //Stop();
                _waitTime -= Time.deltaTime;
            }
        }
    }

/*    private void OnCollisionEnter(Collision collision)
    {
        collision.
    }*/

    private void Chase()
    {
        Move(_runSpeed);
        _agent.SetDestination(_playerController.transform.position);

        if(_agent.remainingDistance <= _agent.stoppingDistance)
        {
            Debug.Log("Caught!");
            GameManager.Instance.UpdateGameState(GameManager.GameState.Lose);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform player = other.transform;
        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (Physics.Raycast(transform.position, dirToPlayer, distanceToPlayer, _playerMask))
        {
            Debug.DrawRay(transform.position, dirToPlayer, Color.red);
            _chasePlayer = true;
            _isPatroling = false;
            _playerLastPosition = player.position;
            _light.color = _detectedLightColor;
            Debug.Log("I see you!");
        }
        else if (!Physics.Raycast(transform.position, dirToPlayer, distanceToPlayer, _obstacleMask) &&
                   !Physics.Raycast(transform.position, dirToPlayer, distanceToPlayer, _groundMask))
        {
            /*_light.color = _alertLightColor;
            _chasePlayer = false;
            _goTolastPlayerPosition = true;
            _playerLastPosition = player.position;*/
            Vector3 dirToLastPos = (_playerLastPosition - transform.position).normalized;
            float distanceToLastPos = Vector3.Distance(transform.position, _playerLastPosition);
            if (Physics.Raycast(transform.position, dirToLastPos, distanceToLastPos, _playerMask))
            {
                _chasePlayer = true;
                _isPatroling = false;
                _playerLastPosition = player.position;
                Debug.Log("Still CHASING PLAYER");
            }
            else
            {
                _light.color = _lightColor;
                _chasePlayer = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("I DO NOT see you anymoe!");
        Transform player = other.transform;
        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (Physics.Raycast(transform.position, dirToPlayer, distanceToPlayer, _playerMask))
        {
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
}

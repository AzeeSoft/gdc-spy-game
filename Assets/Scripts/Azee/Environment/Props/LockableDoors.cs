using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LockableDoors : MonoBehaviour
{
    [SerializeField]
    private bool _isLocked = true;

    private Animator _animator;

    private int _objectsInProximity = 0;

    private InteractiveObject _interactiveObject;

    private NavMeshObstacle _navMeshObstacle;

    // Use this for initialization
    void Awake()
    {
        _interactiveObject = GetComponent<InteractiveObject>();
        _animator = GetComponent<Animator>();
        _navMeshObstacle = GetComponent<NavMeshObstacle>();
    }

    void Start()
    {
        _animator.SetBool("open", false);

        _interactiveObject.enabled = _isLocked;
        _navMeshObstacle.enabled = _isLocked;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Guard")
        {
            _objectsInProximity++;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Guard")
        {
            _objectsInProximity--;
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool isOpen = !_isLocked && _objectsInProximity > 0;
        _animator.SetBool("open", isOpen);
    }

    public void Lock()
    {
        _isLocked = true;
        _interactiveObject.enabled = true;
        _navMeshObstacle.enabled = true;
    }

    public void Unlock()
    {
        _isLocked = false;
        _interactiveObject.enabled = false;
        _navMeshObstacle.enabled = false;
    }
}

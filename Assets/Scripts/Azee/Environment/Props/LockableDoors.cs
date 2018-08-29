using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LockableDoors : MonoBehaviour
{
    [SerializeField] private bool _isLocked = true;

    public Color LockedXRayColor = Color.red;
    public Color UnlockedXRayColor = Color.green;

    private Animator _animator;

    private int _objectsInProximity = 0;

    private InteractiveObject _interactiveObject;

    private NavMeshObstacle _navMeshObstacle;

    private Material _material;

    // Use this for initialization
    void Awake()
    {
        _interactiveObject = GetComponent<InteractiveObject>();
        _animator = GetComponent<Animator>();
        _navMeshObstacle = GetComponent<NavMeshObstacle>();
    }

    void Start()
    {
        _material = GetComponent<Renderer>().material;

        _animator.SetBool("open", false);

        _interactiveObject.enabled = _isLocked;
        _navMeshObstacle.enabled = _isLocked;

        UpdateXRayColor();
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

        UpdateXRayColor();
    }

    public void Unlock()
    {
        _isLocked = false;
        _interactiveObject.enabled = false;
        _navMeshObstacle.enabled = false;

        UpdateXRayColor();
    }

    private void UpdateXRayColor()
    {
        _material.SetColor("_XRayColor", (_isLocked ? LockedXRayColor : UnlockedXRayColor));
    }
}
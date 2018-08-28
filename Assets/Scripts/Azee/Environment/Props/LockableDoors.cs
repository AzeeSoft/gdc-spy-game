using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockableDoors : MonoBehaviour
{
    [SerializeField]
    private bool _isLocked = true;

    private Animator _animator;

    private int _objectsInProximity = 0;

    private InteractiveObject _interactiveObject;

    // Use this for initialization
    void Awake()
    {
        _interactiveObject = GetComponent<InteractiveObject>();
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        _animator.SetBool("open", false);

        _interactiveObject.enabled = _isLocked;
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
        _animator.SetBool("open", !_isLocked && _objectsInProximity > 0);
    }

    public void Lock()
    {
        _isLocked = true;
        _interactiveObject.enabled = true;
    }

    public void Unlock()
    {
        _isLocked = false;
        _interactiveObject.enabled = false;
    }
}

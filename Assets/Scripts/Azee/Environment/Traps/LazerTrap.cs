using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerTrap : MonoBehaviour
{
    public float LoopTime = 1f;
    public GameObject LazerBeamsGameObject;

    private Animator _animator;
    private LazerBeamCollisionDetector _lazerBeamCollisionDetector;

    private bool _wasOpen = true;
    private bool _isOpen = true;

    private float _guardsInsideZone = 0;

    private IEnumerator LazerOpenCloseCoroutine;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _lazerBeamCollisionDetector = LazerBeamsGameObject.AddComponent<LazerBeamCollisionDetector>();
        _lazerBeamCollisionDetector.Initialize(this);

        LazerOpenCloseCoroutine = LoopBeamOpenClose();
    }

    // Use this for initialization
    void Start()
    {

    }

    void OnEnable()
    {
//        Debug.Log("Enabling Lazer Trap");
        StartCoroutine(LazerOpenCloseCoroutine);
    }

    // Update is called once per frame
    void Update()
    {
        if (_guardsInsideZone > 0)
        {
            _isOpen = false;
        }

        if (_wasOpen != _isOpen)
        {
            _wasOpen = _isOpen;
            _animator.SetTrigger(_isOpen ? "open" : "close");
        }
    }

    IEnumerator LoopBeamOpenClose()
    {
        while (true)
        {
            yield return new WaitForSeconds(LoopTime);
            _isOpen = !_isOpen;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Guard"))
        {
            _guardsInsideZone++;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Guard"))
        {
            _guardsInsideZone--;
        }
    }

    void OnDisable()
    {
//        Debug.Log("Disabling Lazer Trap");
        StopCoroutine(LazerOpenCloseCoroutine);
    }


    private void OnLazerBeamCollisionDetected(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("Player hit by Lazer");
            // TODO: Do whateva
        }
    }


    public class LazerBeamCollisionDetector : MonoBehaviour
    {
        private LazerTrap _lazerTrap;

        public void Initialize(LazerTrap lazerTrap)
        {
            _lazerTrap = lazerTrap;
        }

        void OnCollisionEnter(Collision collision)
        {
//            Debug.Log("Lazer Beam Collision");
//            Debug.Log(collision.collider.gameObject);
            _lazerTrap.OnLazerBeamCollisionDetected(collision);
        }
    }

    public void OnScenePaused()
    {

    }

    public void OnSceneResumed()
    {

    }
}
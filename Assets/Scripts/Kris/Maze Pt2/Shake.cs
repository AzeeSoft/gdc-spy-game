using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class ShakeProps
{
    public float x;
    public float y;
}

public class Shake : MonoBehaviour {

    Transform _target;

    Vector3 _initialPos;



    public ShakeProps shakeProp1;
    public ShakeProps shakeProp2;
    

        

    private void Start()
    {
        _target = GetComponent<Transform>();
        _initialPos = _target.localPosition;
    }

    float _pendingShakeDuration = 0f;

    public void cShake(float duration)
    {
        if (duration > 0)
        {
            _pendingShakeDuration += duration;
        }
    }

    bool _isShaking = false;

    private void Update()
    {
        if(_pendingShakeDuration > 0 && !_isShaking)
        {
            StartCoroutine(DoShake());
        }
    }

    IEnumerator DoShake()
    {
        _isShaking = true;

        var startTime = Time.realtimeSinceStartup;
        Vector3 originalLocalPos = _target.localPosition;

        while(Time.realtimeSinceStartup < startTime + _pendingShakeDuration)
        {
            var randomPoint = new Vector3(UnityEngine.Random.Range(-shakeProp1.x, shakeProp2.x), UnityEngine.Random.Range(-shakeProp1.y, shakeProp2.y), _initialPos.z);
            _target.localPosition = originalLocalPos + randomPoint;
            yield return null;
        }

        _pendingShakeDuration = 0f;
        _target.localPosition = _initialPos;
        _isShaking = false;
    }
}
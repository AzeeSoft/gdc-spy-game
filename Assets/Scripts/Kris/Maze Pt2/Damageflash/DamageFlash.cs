using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour {

    private Animator _animator;



    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void onDamageTake()
    {
        _animator.SetBool("isDamaged", true);
    }

    public void resetAnimation()
    {
        _animator.SetBool("isDamaged", false);
    }
}

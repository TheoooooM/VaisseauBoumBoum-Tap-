using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private float lerpSpeed=.1f;
    [SerializeField] private float minLerpSpeed = .2f;
    [Space]
    [SerializeField]private Transform target;

    private Vector3 _targetOffset;

    private void Start()
    {
        _targetOffset = transform.position - target.position;
    }

    private void FixedUpdate()
    {
        var lerpValue = Vector3.Lerp(transform.position, target.position, lerpSpeed);
        if (lerpValue.magnitude < minLerpSpeed) lerpValue = lerpValue.normalized * minLerpSpeed;
        transform.position =  lerpValue;
        transform.rotation =Quaternion.Lerp(transform.rotation, target.rotation, lerpSpeed);
    }
}

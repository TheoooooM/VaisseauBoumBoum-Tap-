using System;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rb;
    
    [SerializeField] private float pitchForce = 1; //X axis
    [SerializeField] private float rollForce = 1;  //Z axis 
    [SerializeField] private float yawForce = 1; //Y axis
    [Space]
    [SerializeField] private float torqueForce = 1;

    [Space] [SerializeField] private GameObject reactorTrails;

    private float _pitchValue;
    private float _rollValue;
    private float _yawValue;
    private float _torqueValue;
    
    private StarshipController _inputs;

    private void Awake()
    {
        _inputs = new StarshipController();

        _inputs.Movement.Pitch.performed += value => _pitchValue = value.ReadValue<float>(); 
        _inputs.Movement.Roll.performed += value => _rollValue = value.ReadValue<float>(); 
        _inputs.Movement.Yaw.performed += value => _yawValue = value.ReadValue<float>(); 
        _inputs.Movement.Pitch.canceled += _ => _pitchValue = 0; 
        _inputs.Movement.Roll.canceled += _ => _rollValue = 0; 
        _inputs.Movement.Yaw.canceled += _ => _yawValue = 0;
        
        _inputs.Movement.Propulse.performed += value => _torqueValue = value.ReadValue<float>();
        _inputs.Movement.Propulse.canceled += _ => _torqueValue = 0;
        
        _inputs.Enable();
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
       
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        Rotate();
        if (_torqueValue != 0)
        {
            _rb.AddForce(transform.forward*_torqueValue*torqueForce);
        }
    }

    void Rotate()
    {
        if (_pitchValue != 0) transform.Rotate(Vector3.right, pitchForce*_pitchValue);
        if (_rollValue != 0) transform.Rotate(Vector3.forward, rollForce*_rollValue);
        if (_yawValue != 0) transform.Rotate(Vector3.up, yawForce*_yawValue);
    }
    
    
}

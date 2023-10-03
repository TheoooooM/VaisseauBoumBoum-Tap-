using System;
using UnityEngine;
using UnityEngine.UIElements;

enum Controller
{
    Casual, DualStick
}

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rb;
    
    [SerializeField] private float pitchForce = 1; //X axis
    [SerializeField] private float rollForce = 1;  //Z axis 
    [SerializeField] private float yawForce = 1; //Y axis
    [Space]
    [SerializeField] private float torqueForce = 1;

    [Space] [SerializeField] private Controller controlType;
    [Space] [SerializeField] private GameObject VFXThrottle;

    private float _pitchValue;
    private float _rollValue;
    private float _yawValue;
    private float _torqueValue;
    
    private StarshipController _inputs;

    private void Awake()
    {
        _inputs = new StarshipController();

        switch (controlType)
        {
            case Controller.Casual:
                _inputs.Movement.Pitch.performed += value => _pitchValue = value.ReadValue<float>(); 
                _inputs.Movement.Roll.performed += value => _rollValue = value.ReadValue<float>(); 
                _inputs.Movement.Yaw.performed += value => _yawValue = value.ReadValue<float>(); 
                _inputs.Movement.Pitch.canceled += _ => _pitchValue = 0; 
                _inputs.Movement.Roll.canceled += _ => _rollValue = 0; 
                _inputs.Movement.Yaw.canceled += _ => _yawValue = 0;
        
                _inputs.Movement.Propulse.performed += value => _torqueValue = value.ReadValue<float>();
                _inputs.Movement.Propulse.canceled += _ => _torqueValue = 0;
                
                break;
            case Controller.DualStick:
                _inputs.Movement1.Pitch.performed += value => _pitchValue = value.ReadValue<float>(); 
                _inputs.Movement1.Roll.performed += value => _rollValue = value.ReadValue<float>(); 
                _inputs.Movement1.Yaw.performed += value => _yawValue = value.ReadValue<float>(); 
                _inputs.Movement1.Pitch.canceled += _ => _pitchValue = 0; 
                _inputs.Movement1.Roll.canceled += _ => _rollValue = 0; 
                _inputs.Movement1.Yaw.canceled += _ => _yawValue = 0;

                _inputs.Movement1.Propulse.started += _ => VFXThrottle.SetActive(true);
                _inputs.Movement1.Propulse.performed += value => _torqueValue = value.ReadValue<float>();
                _inputs.Movement1.Propulse.canceled += _ =>
                {
                    _torqueValue = 0;
                    VFXThrottle.SetActive(false);
                };
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        /*_inputs.Movement1.Pitch.performed += value => _pitchValue = value.ReadValue<float>(); 
        _inputs.Movement1.Roll.performed += value => _rollValue = value.ReadValue<float>(); 
        _inputs.Movement1.Yaw.performed += value => _yawValue = value.ReadValue<float>(); 
        _inputs.Movement1.Pitch.canceled += _ => _pitchValue = 0; 
        _inputs.Movement1.Roll.canceled += _ => _rollValue = 0; 
        _inputs.Movement1.Yaw.canceled += _ => _yawValue = 0;
        
        _inputs.Movement1.Propulse.performed += value => _torqueValue = value.ReadValue<float>();
        _inputs.Movement1.Propulse.canceled += _ => _torqueValue = 0;*/
        
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

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
    private bool isShooting;
    
    private StarshipController _inputs;

    [Header("Shooting Variables")]

    [SerializeField] private float shootingRate;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Rigidbody bullet;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private GameObject hitPointDebug;
    [SerializeField] private LayerMask layerMask;
    private float lastShootTime;
    private Camera camera;
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
                _inputs.Movement.Shoot.performed += shoot => isShooting = true;
                _inputs.Movement.Shoot.canceled += shoot => isShooting = false;
                
                break;
            case Controller.DualStick:
                _inputs.Movement1.Pitch.performed += value =>
                {
                    _pitchValue = value.ReadValue<float>();
                }; 
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
                _inputs.Movement1.Shoot.performed += shoot => isShooting = true;
                _inputs.Movement1.Shoot.canceled += shoot => isShooting = false;
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
        camera = Camera.main;
    }

    private void Update()
    {
       Shoot();
    }
    private void FixedUpdate()
    {
        Move();
    }

    private void Shoot()
    {
        RaycastHit hit;
        var position = camera.transform.position;
        Physics.Raycast(position, camera.transform.forward, out hit, 10000f, layerMask);
        var bulletDirection = (hit.point - shootingPoint.position).normalized;
        hitPointDebug.transform.position = hit.point;
        if (isShooting && Time.time >= lastShootTime + 1 / shootingRate)
        {
            lastShootTime = Time.time;
            var newBullet = Instantiate(bullet, shootingPoint.position, transform.rotation, null);
            newBullet.velocity = bulletDirection * bulletSpeed;
        }

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

    private void OnDrawGizmos()
    {
        var position = camera.transform.position;
        Gizmos.DrawRay(position, camera.transform.forward);
    }
}

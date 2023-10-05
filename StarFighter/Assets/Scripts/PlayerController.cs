using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

enum Controller
{
    Casual, DualStick, ChorusLike
}

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    
    
    private Rigidbody _rb;


    [SerializeField] private Animator _animator;
    [Space]
    [SerializeField] private float pitchForce = 1; //X axis
    [SerializeField] private float rollForce = 1;  //Z axis 
    [SerializeField] private float yawForce = 1; //Y axis
    [Space]
    [SerializeField] private float torqueForce = 1;
    [Space]
    [SerializeField] private float spinDuration = 1;
    [SerializeField] private float spinForce = 1;
    [SerializeField] private float spinMarge = .2f;
    [Space] 
    [SerializeField] private Controller controlType;
    [Space] 
    [SerializeField] private GameObject VFXThrottle;
    

    private float _pitchValue;
    private float _rollValue;
    private float _yawValue;
    private float _torqueValue;
    private bool isShooting;
    private bool trySpin;
    private bool spinning;
    
    
    private StarshipController _inputs;

    [Header("Shooting Variables")]

    [SerializeField] private float shootingRate;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Image crossHair;
    private Transform targetObject;
    private float lastShootTime;
    [SerializeField] private float heightDistance = 0.2f;
    private float maxAngle;
    [SerializeField] private Transform camera;

    [SerializeField] private Transform debugger;
    private void Awake()
    {
        _inputs = new StarshipController();

        if (instance != null) DestroyImmediate(gameObject);
        else instance = this;
        
        switch (controlType)
        {
            case Controller.Casual:
                _inputs.Casual.Pitch.performed += value => _pitchValue = value.ReadValue<float>(); 
                _inputs.Casual.Roll.performed += value => _rollValue = value.ReadValue<float>(); 
                _inputs.Casual.Yaw.performed += value => _yawValue = value.ReadValue<float>(); 
                _inputs.Casual.Pitch.canceled += _ => _pitchValue = 0; 
                _inputs.Casual.Roll.canceled += _ => _rollValue = 0; 
                _inputs.Casual.Yaw.canceled += _ => _yawValue = 0;
        
                _inputs.Casual.Propulse.performed += value => _torqueValue = value.ReadValue<float>();
                _inputs.Casual.Propulse.canceled += _ => _torqueValue = 0;
                _inputs.Casual.Shoot.performed += shoot => isShooting = true;
                _inputs.Casual.Shoot.canceled += shoot => isShooting = false;
                
                break;
            case Controller.DualStick:
                _inputs.DualStick.Pitch.performed += value =>
                {
                    _pitchValue = value.ReadValue<float>();
                }; 
                _inputs.DualStick.Roll.performed += value => _rollValue = value.ReadValue<float>(); 
                _inputs.DualStick.Yaw.performed += value => _yawValue = value.ReadValue<float>(); 
                _inputs.DualStick.Pitch.canceled += _ => _pitchValue = 0; 
                _inputs.DualStick.Roll.canceled += _ => _rollValue = 0; 
                _inputs.DualStick.Yaw.canceled += _ => _yawValue = 0;

                _inputs.DualStick.Propulse.started += _ => VFXThrottle.SetActive(true);
                _inputs.DualStick.Propulse.performed += value => _torqueValue = value.ReadValue<float>();
                _inputs.DualStick.Propulse.canceled += _ =>
                {
                    _torqueValue = 0;
                    VFXThrottle.SetActive(false);
                };
                _inputs.DualStick.Shoot.performed += shoot => isShooting = true;
                _inputs.DualStick.Shoot.canceled += shoot => isShooting = false;
                _inputs.DualStick.Spin.started += _ => trySpin = !spinning;
                _inputs.DualStick.Spin.canceled += _ => trySpin = false;
                break;
            case Controller.ChorusLike:
                _inputs.ChorusMapping.Pitch.performed += value =>
                {
                    _pitchValue = Deadzoner(value.ReadValue<float>());
                };
                _inputs.ChorusMapping.Roll.performed += value => _rollValue = Deadzoner(value.ReadValue<float>()); 
                _inputs.ChorusMapping.Yaw.performed += value => _yawValue = Deadzoner(value.ReadValue<float>()); 
                _inputs.ChorusMapping.Pitch.canceled += _ => _pitchValue = 0; 
                _inputs.ChorusMapping.Roll.canceled += _ => _rollValue = 0; 
                _inputs.ChorusMapping.Yaw.canceled += _ => _yawValue = 0;

                _inputs.ChorusMapping.Propulse.started += _ => VFXThrottle.SetActive(true);
                _inputs.ChorusMapping.Propulse.performed += value => _torqueValue = Deadzoner(value.ReadValue<float>());
                _inputs.ChorusMapping.Propulse.canceled += _ =>
                {
                    _torqueValue = 0;
                    VFXThrottle.SetActive(false);
                };
                _inputs.ChorusMapping.Shoot.performed += shoot => isShooting = true;
                _inputs.ChorusMapping.Shoot.canceled += shoot => isShooting = false;
                
                _inputs.ChorusMapping.Spin.started += _ => trySpin = !spinning;
                _inputs.ChorusMapping.Spin.canceled += _ => trySpin = false;
                break;
            default: throw new ArgumentOutOfRangeException();
        }

        _inputs.General.TogglePause.performed += _ => UIManager.Instance.TogglePause();
        /*_inputs.DualStick.Pitch.performed += value => _pitchValue = value.ReadValue<float>(); 
        _inputs.DualStick.Roll.performed += value => _rollValue = value.ReadValue<float>(); 
        _inputs.DualStick.Yaw.performed += value => _yawValue = value.ReadValue<float>(); 
        _inputs.DualStick.Pitch.canceled += _ => _pitchValue = 0; 
        _inputs.DualStick.Roll.canceled += _ => _rollValue = 0; 
        _inputs.DualStick.Yaw.canceled += _ => _yawValue = 0;
        
        _inputs.DualStick.Propulse.performed += value => _torqueValue = value.ReadValue<float>();
        _inputs.DualStick.Propulse.canceled += _ => _torqueValue = 0;*/
        
        _inputs.Enable();
    }

    private float Deadzoner(float value)
    {
        value = Mathf.Clamp(value, -Mathf.Sqrt(2)/2, Mathf.Sqrt(2)/2);
        value *= 1.41421356f;
        return value;
    }
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        //camera = Camera.main;
        var cameraTransform = camera.transform;
        maxAngle = Vector3.Angle(cameraTransform.forward,
            cameraTransform.forward + (cameraTransform.up * heightDistance));

    }

    private void Update()
    {
       Shoot();
       if(trySpin)TrySpin();
    }
    private void FixedUpdate()
    {
        Move();
    }

    private void Shoot()
    {
        RaycastHit hit;
        var position = camera.transform.position;
        var bulletDirection = Vector3.zero;
        if (Physics.Raycast(position, camera.transform.forward, out hit, 350f, layerMask))
        {
            crossHair.color = Color.red;
            bulletDirection = (hit.point - shootingPoint.position).normalized;
            targetObject = hit.collider.transform;
        }
        else if (targetObject) 
        {
            var objectAngle = Vector3.Angle(camera.transform.forward, targetObject.position- transform.position);
            Debug.DrawRay(transform.position, camera.transform.forward, Color.black, 5);
            Debug.DrawRay(transform.position, (targetObject.position - transform.position).normalized, Color.red,5);
            if (objectAngle < maxAngle)
            {
                crossHair.color = Color.red;
                bulletDirection = (hit.point - shootingPoint.position).normalized;
            }
            else
            {
                var objectVector = targetObject.position - camera.transform.position;
                var objectMagnitude = objectVector.magnitude;

                var adjacentLength = Mathf.Cos(objectAngle*Mathf.Deg2Rad) * objectMagnitude;
                var direction = (targetObject.position - (camera.position + camera.forward * adjacentLength)).normalized;
                var maxHeightValue = Mathf.Tan(maxAngle* Mathf.Deg2Rad) * adjacentLength;
                var point = (camera.position + camera.transform.forward * adjacentLength) + direction * maxHeightValue;
                if(debugger)debugger.position = point;
                var rayDir = point - camera.transform.position;
                
                
                Debug.DrawRay(camera.position, objectVector, Color.green, 5f);
                Debug.DrawRay(camera.position, camera.forward* adjacentLength, Color.yellow, 5f);
                Debug.DrawRay(camera.position + camera.forward * adjacentLength, direction, Color.magenta, 5f);
                Debug.DrawRay(camera.position + camera.forward * adjacentLength, direction*maxHeightValue, Color.white, 5f);
                
                if (Physics.Raycast(camera.transform.position, rayDir, out hit, 350f, layerMask))
                {
                    Debug.DrawRay(camera.position, rayDir, Color.blue, 5f);
                    if (hit.transform.gameObject == targetObject.gameObject)
                    {
                        crossHair.color = Color.red;
                        bulletDirection = (hit.point - shootingPoint.position).normalized;
                    }
                    else
                    {
                        crossHair.color = Color.white;
                        targetObject = null;
                        bulletDirection = camera.transform.forward; 
                    }
                }
                else
                {
                    Debug.DrawRay(camera.position, rayDir, Color.red, 5f);

                    targetObject = null;
                    crossHair.color = Color.white;
                    bulletDirection = camera.transform.forward; 
                }

            }
        }
        else
        {
            targetObject = null;
            crossHair.color = Color.white;
            bulletDirection = camera.transform.forward; 
            
        }
        //hitPointDebug.transform.position = hit.point;
        if (isShooting && Time.time >= lastShootTime + 1 / shootingRate)
        {
            lastShootTime = Time.time;
            var newBullet = PoolOfObject.instance.SpawnFromPool(PoolOfObject.Type.Bullet, shootingPoint.position, transform.rotation);
            newBullet.GetComponent<Rigidbody>().velocity = bulletDirection * bulletSpeed;
        }

    }

    void Move()
    {
        if(!spinning)Rotate();
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

    void TrySpin()
    {
        switch (controlType)
        {
            case Controller.Casual: break;
            case Controller.DualStick:
                if (_yawValue > spinMarge) StartCoroutine(Spin(true));
                if (_yawValue < -spinMarge) StartCoroutine(Spin(false));
                break;
            case Controller.ChorusLike:
                if (_yawValue > spinMarge) StartCoroutine(Spin(true));
                if (_yawValue < -spinMarge) StartCoroutine(Spin(false));
                break;
            default: throw new ArgumentOutOfRangeException();
        }
        
    }
    
    IEnumerator Spin(bool spinRight)
    {
        trySpin = false;
        spinning = true;
        _animator.SetTrigger(spinRight?"SpinRight":"SpinLeft");
        var startTime = Time.time;
        while (Time.time< startTime + spinDuration)
        {
            var force = transform.right * ((spinRight ? 1 : -1) * spinForce);
            _rb.AddForce(force);
            yield return new WaitForEndOfFrame();
        }
        spinning = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody _playerRB;
    private Controls _controls;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _rotateSpeed;
    [SerializeField]
    private Gun _gun;

    private void Awake()
    {
        _playerRB = GetComponent<Rigidbody>();
        _controls = new Controls();
        _controls.Player.Enable();
        _controls.Player.Shoot.performed += Shoot;
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _gun.Shoot();
        }
    }

    private void FixedUpdate()
    {
        Vector2 inputVector = _controls.Player.Move.ReadValue<Vector2>();
        float strafe = _controls.Player.Strafe.ReadValue<float>();
        if (strafe == 1)
        {
            _playerRB.velocity = transform.transform.TransformDirection(new Vector3( inputVector.x * _speed,0, inputVector.y * _speed));
        }
        else
        {
            _playerRB.velocity = transform.transform.TransformDirection(new Vector3(0,  0, inputVector.y * _speed));
            transform.Rotate(Vector3.up, _rotateSpeed * inputVector.x * Time.deltaTime);
        }
    }

    
    

}

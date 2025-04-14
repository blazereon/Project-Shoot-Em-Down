using System;
using UnityEngine;
using UnityEngine.InputSystem;
//4/14/25-nathan(-n)
//if u find bugs or any undesirable trait feel free to debug or message me
public class playerScript : MonoBehaviour
{
    public Rigidbody2D rb;

    //horizontal movement var -n
    [SerializeField]public float moveSpeed;
    private Vector2 _moveDirection;

    //jump var -n
    private Vector2 _playerVelocity;
    private bool _playerGrounded;
    [SerializeField]private float _jumpHeight = 5f;

    //input actions -n
    public InputActionReference move;

    //start horizontall movement code -n
    private void Update()
    {
        _moveDirection = move.action.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(_moveDirection.x * moveSpeed, _moveDirection.y * 0);
    }
    //end horizontal movement code -n

    //
}

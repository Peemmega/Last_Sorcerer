using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    PlayerStats player;
    [SerializeField] public Animator anim;
    [SerializeField] public SpriteRenderer playerSprite;
    [SerializeField] private Movement movement;

    private PlayerControls playerControls;
    private Rigidbody rb;
    private Vector3 moveTo;
    private Vector3 scale = Vector3.one;
    private Transform _hand;

    private const string IS_WALK_PARAM = "IsWalk";
    void Awake()
    {
        playerControls = new PlayerControls();
    }
    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void Start()
    {
        player = GetComponent<PlayerStats>();
        rb = GetComponent<Rigidbody>();
        _hand = FindObjectOfType<PlayerAction>().transform.Find("ItemHand").transform;
    }

    void Update()
    {
        //Debug.Log(movement.isSprinting);
        float x = playerControls.Player.Move.ReadValue<Vector2>().x;
        float z = playerControls.Player.Move.ReadValue<Vector2>().y;


        moveTo = new Vector3(x,0,z).normalized;

        anim.SetBool(IS_WALK_PARAM, moveTo != Vector3.zero);

        Vector3 mouseDirection = player.GetComponent<PlayerAction>().MouseDirection;

        if (mouseDirection.x != 0 && mouseDirection.x > 0)
        {
            playerSprite.transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
            _hand.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
            _hand.localPosition = new Vector3(0.53f, -0.197f, -1);
        }

        if (mouseDirection.x != 0 && mouseDirection.x < 0)
        {
            playerSprite.transform.localScale = new Vector3(scale.x, scale.y, scale.z);
            _hand.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            _hand.localPosition = new Vector3(-0.53f, -0.197f, -1);
        }
    }

    public void Sprint(InputAction.CallbackContext context)
    {
        movement.isSprinting = context.started || context.performed;
    }

    private void FixedUpdate()
    {
        if (movement.isSprinting && transform.GetComponent<PlayerStats>().currentSTA > 5)
        {
            rb.MovePosition(transform.position + moveTo * player.CurrentMoveSpeed * 2 * Time.fixedDeltaTime);
        }
        else
        {
            rb.MovePosition(transform.position + moveTo * player.CurrentMoveSpeed * Time.fixedDeltaTime);

        }
    }

    public void SetOverworldVisuals(Animator animator, SpriteRenderer spriteRenderer, Vector3 playerScale)
    {
        anim = animator;
        playerSprite = spriteRenderer;
        scale = playerScale;
    }

    public bool GetSprint()
    {
        return movement.isSprinting;
    }
}


[Serializable]
public struct Movement
{
    [HideInInspector] public bool isSprinting;
}

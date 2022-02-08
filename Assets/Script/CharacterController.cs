using Spine.Unity;
using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SkeletonAnimation))]
[RequireComponent(typeof(BoxCollider2D))]

public class CharacterController : MonoBehaviour
{

    public static Action<bool> CharacterIsALive = delegate { };

    [SerializeField] private GameObject _character;
    [SerializeField] private float _speed;
    [SerializeField] private LayerMask _isGround;
    [SerializeField] private float _checkRadius;

    private Transform GroundCheck;
    private bool isGrounded;
    private Rigidbody2D rb2D;
    private Animator animator;
    private SkeletonAnimation anim;
    private Vector2 direction;

    private const string _walk = "Walk";
    private const string _hit = "Hit";

    private void Start()
    {
        GroundCheck = gameObject.GetComponent<Transform>();
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        anim = animator.GetComponent<SkeletonAnimation>();
        CharacterIsALive(true);

    }

    public void OnEnable()
    {
        AttackState.Hit += Hit;
    }

    public void OnDisable()
    {
        AttackState.Hit -= Hit;
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(GroundCheck.position, _checkRadius, _isGround);
    }

    private void Update()
    {

        #region Key Input

        if (Input.GetKeyDown(KeyCode.None))
        {
            animator.SetBool(_walk, false);
            anim.Initialize(true);
            direction.x = 0;
            return;
        }

        if (Input.anyKey)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                anim.initialFlipX = false;
                animator.SetBool(_walk, true);
                anim.Initialize(true);
                direction.x = -1f;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                anim.initialFlipX = true;
                animator.SetBool(_walk, true);
                anim.Initialize(true);
                direction.x = 1f;
            }

            _character.transform.Translate(direction.normalized * _speed);
        }
        else
        {
            if (animator.GetBool(_walk) == true)
            {
                animator.SetBool(_walk, false);
                anim.Initialize(true);
            }

            direction.x = 0;
        }

        #endregion

    }

    private void Hit()
    {
        animator.SetTrigger(_hit);

    }

}

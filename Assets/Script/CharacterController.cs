using Spine.Unity;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SkeletonAnimation))]
[RequireComponent(typeof(BoxCollider2D))]

public class CharacterController : MonoBehaviour
{

    [SerializeField] private GameObject _character;
    [SerializeField] private float _speed;
    [SerializeField] private LayerMask _isGround;
    [SerializeField] private float _checkRadius;

    private Animator animator;
    private SkeletonAnimation anim;
    private Vector2 direction;

    private const string Walk = "Walk";
    private const string Hit = "Hit";

    private void Start()
    {
        animator = GetComponent<Animator>();
        anim = animator.GetComponent<SkeletonAnimation>();

    }

    private void OnEnable()
    {
        AttackState.TakeHit += TakingDamage;
    }

    private void OnDisable()
    {
        AttackState.TakeHit -= TakingDamage;
    }

    private void Update()
    {

        #region Key Input

        if (Input.GetKeyDown(KeyCode.None))
        {
            animator.SetBool(Walk, false);
            anim.Initialize(true);
            direction.x = 0;
            return;
        }

        if (Input.anyKey)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                anim.initialFlipX = false;
                animator.SetBool(Walk, true);
                anim.Initialize(true);
                direction.x = -1f;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                anim.initialFlipX = true;
                animator.SetBool(Walk, true);
                anim.Initialize(true);
                direction.x = 1f;
            }

            _character.transform.Translate(direction.normalized * _speed);
        }
        else
        {
            if (animator.GetBool(Walk) == true)
            {
                animator.SetBool(Walk, false);
                anim.Initialize(true);
            }

            direction.x = 0;
        }

        #endregion

    }

    private void TakingDamage()
    {
        animator.SetTrigger(Hit);

    }

}

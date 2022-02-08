using Spine.Unity;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SkeletonAnimation))]

public class AnimationController : MonoBehaviour
{

    [SerializeField] private string _lostAnim;
    [SerializeField] private bool _lostRotate;

    private Animator _animator;
    private SkeletonAnimation _anim;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _anim = _animator.GetComponent<SkeletonAnimation>();

    }

    public void OnEnable()
    {
        PatrolState.SetAnim += SetAnimation;
        EatState.SetAnim += SetAnimation;
        AttackState.SetAnim += SetAnimation;
        WaitState.SetAnim += SetAnimation;
        CallState.SetAnim += SetAnimation;

    }
    public void OnDisable()
    {
        PatrolState.SetAnim -= SetAnimation;
        EatState.SetAnim -= SetAnimation;
        AttackState.SetAnim -= SetAnimation;
        WaitState.SetAnim -= SetAnimation;
        CallState.SetAnim -= SetAnimation;

    }


    private void SetAnimation(Animations.PoliceMan PoliceMan, bool rotate)
    {

        if (($"{PoliceMan}" == _lostAnim) 
            && (rotate == _lostRotate)) 
        { 
            return; 
        }

        _animator.SetBool(_lostAnim, false);
        _animator.SetBool($"{PoliceMan}", true);
        _anim.AnimationName = $"{PoliceMan}";
        _anim.initialFlipX = rotate;
        _anim.loop = true;
        _anim.Initialize(true);
        _lostAnim = $"{PoliceMan}";
        _lostRotate = rotate;

    } 

}

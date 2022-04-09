using UnityEngine;
using Spine.Unity;

public class AnimState : StateMachineBehaviour
{
    
    [Header("Animation State Parameters")]
    [SerializeField] private string _animationName;
    [SerializeField] private string _skinName;
    [SerializeField] private float _speedAnimation;

     
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int LayerIndex)
    {
        SkeletonAnimation anim = animator.GetComponent<SkeletonAnimation>();
        anim.initialSkinName = _skinName;
        anim.Initialize(true);
        anim.state.SetAnimation(0, _animationName, true).TimeScale = _speedAnimation;

    }




}

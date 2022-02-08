using System;
using UnityEngine;
using Spine;
using Spine.Unity;

public class AnimState : StateMachineBehaviour
{

    public static event Action<float> currentAnimDuration = delegate { };

    [Header("Animation State Parameters")]
    [SerializeField] private string _animationName;
    [SerializeField] private string _skinName;
    [SerializeField] private float _speed;
    [SerializeField] private float _animationEnd;
    Spine.AnimationState animationState;


    public void OnEnable()
    {
        try
        {
            Spine.TrackEntry trackEntry = animationState.SetAnimation(0, _animationName, true); // 0 - track Index
            animationState.End += OnSpineAnimationEnd;
        }
        catch
        {
            
        }

    }

    public void OnDisable()
    {
        try
        {
            animationState.End -= OnSpineAnimationEnd;
        }
        catch
        {
            
        }

    }

     
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int LayerIndex)
    {
        SkeletonAnimation anim = animator.GetComponent<SkeletonAnimation>();
        anim.initialSkinName = _skinName;
        anim.Initialize(true);
        currentAnimDuration.Invoke(_animationEnd);
        anim.state.SetAnimation(0, _animationName, true).TimeScale = _speed;
    }

    #region OnSpineAnimationTrackEntry

    private void OnSpineAnimationStart(TrackEntry trackEntry)
    {
        // Add your implementation code here to react to start events
    }
    private void OnSpineAnimationInterrupt(TrackEntry trackEntry)
    {
        // Add your implementation code here to react to interrupt events
    }
    private void OnSpineAnimationEnd(TrackEntry trackEntry)
    {
        _animationEnd = trackEntry.AnimationEnd;
    }
    private void OnSpineAnimationDispose(TrackEntry trackEntry)
    {
        // Add your implementation code here to react to dispose events
    }
    private void OnSpineAnimationComplete(TrackEntry trackEntry)
    {
        // Add your implementation code here to react to complete events
    }

    #endregion


}

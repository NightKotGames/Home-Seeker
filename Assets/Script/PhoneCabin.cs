﻿using Spine.Unity;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SkeletonAnimation))]

public class PhoneCabin : MonoBehaviour
{
    private Animator _animator;
    private SkeletonAnimation _anim;
    private bool _doorOpen;

    private const string _open = "Open";
    private const string _close = "Close";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _anim = _animator.GetComponent<SkeletonAnimation>();

    }

    public void OnEnable()
    {
        CallState.Phone += DoorState;
    }
    public void OnDisable()
    {
        CallState.Phone -= DoorState;
    }

    private void DoorState(bool _doorOpen)
    {
        if (this._doorOpen == _doorOpen) { return; }

        if (_doorOpen)
        {
            _animator.SetTrigger(_open);
            this._doorOpen = _doorOpen;
        }
        else if (!_doorOpen)
        {
            _animator.SetTrigger(_close);
            this._doorOpen = _doorOpen;
        }

    }


}

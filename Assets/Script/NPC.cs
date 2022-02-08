﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]

public class NPC : MonoBehaviour
{

    public static event Action<Needs.NeedsCollection> NeedsRestore = delegate { };

    [Header("State kit: ")]
    [SerializeField] private State _startState;
    [SerializeField] private State _attackState;
    [SerializeField] private State _eatState;
    [SerializeField] private State _callState;
    [SerializeField] private State _waitState;

    [Header("Hero Settings")]
    [SerializeField] private float _normalSpeed;
    [SerializeField] private float _doubleSpeed;
    [SerializeField] private float _attackSpeed;

    [Header("Progression of needs")]
    [SerializeField] private float _hungryProgress;
    [SerializeField] private float _boredomProgress;
    [SerializeField] private float _fatigueProgress;

    [Header("Actual State: ")]
    [SerializeField] private State _currentState;

    [Header("State Hero: ")]
    [SerializeField] private float _maxHungry;

    #region Public Properties

    public float MaxHungry
    {
        get { return _maxHungry; }
    }

    [SerializeField] private float _maxBoredom;
    public float MaxBoredom
    {
        get { return _maxBoredom; }
    }

    [SerializeField] private float _maxFatique;
    public float MaxFatique
    {
        get { return _maxFatique; }
    }

    [SerializeField] private float _speed;
    public float Speed
    {
        get { return _speed; }
    }

    private Camera _cam;
    public Camera Cam
    {
        get { return _cam; }
    }

    public float Hungry
    {
        get { return _npcNeeds[Needs.NeedsCollection.hungry]; }
    }
    public float Boredom
    {
        get { return _npcNeeds[Needs.NeedsCollection.boredom]; }
    }
    public float Fatique
    {
        get { return _npcNeeds[Needs.NeedsCollection.fatique]; }
    }

    #endregion

    [HideInInspector] public Vector3 StartPos;
    private Vector3 _lookPoint;
    private bool _alarm = false;
    private bool _hide;

    private Dictionary<Needs.NeedsCollection, float> _npcNeeds;

    private void Awake()
    {
        _cam = FindObjectOfType<Camera>();
        _npcNeeds = new Dictionary<Needs.NeedsCollection, float>() { { Needs.NeedsCollection.hungry, 0f},
                                                                   { Needs.NeedsCollection.boredom, 0f},
                                                                   { Needs.NeedsCollection.fatique, 0f} };
        StartPos = transform.position;
        SetState(_startState);

    }

    public void OnEnable()
    {
        EatState.Eating += RestoreNeeds;
        CallState.Calling += RestoreNeeds;
        AlarmZoneDetector.Alarm += Alarm;
        HideZone.Hide += Hide;
        WaitState.Wait += RestoreNeeds;

    }

    public void OnDisable()
    {
        EatState.Eating -= RestoreNeeds;
        CallState.Calling -= RestoreNeeds;
        AlarmZoneDetector.Alarm -= Alarm;
        HideZone.Hide -= Hide;
        WaitState.Wait -= RestoreNeeds;

    }


    void Update()
    {
        _npcNeeds[Needs.NeedsCollection.hungry] += _hungryProgress;
        _npcNeeds[Needs.NeedsCollection.boredom] += _boredomProgress;
        _npcNeeds[Needs.NeedsCollection.fatique] += _fatigueProgress;

        #region State Switching

        if (!_currentState.IsFinished)
        {
            _currentState.Run();
        }
        else
        {

            if (_alarm)
            {
                SetState(_attackState);
                _speed = _attackSpeed;
            }

            else if (_hide && _alarm)
            {
                SetState(_startState);
                _speed = _normalSpeed;
                _alarm = false;

            }

            else if (Hungry >= _maxHungry)
            {
                SetState(_eatState);
                _speed = _doubleSpeed;
            }

            else if (Boredom >= _maxBoredom)
            {
                SetState(_callState);
                _speed = _doubleSpeed;
            }

            else if (Fatique >= _maxFatique)
            {
                SetState(_waitState);
            }
            else
            {
                SetState(_startState);
                _speed = _normalSpeed;
            }

        }

        #endregion

    }

    public void SetState(State state)
    {
        _currentState = Instantiate(state);
        _currentState.Character = this;
        _currentState.Init();

    }

    public void MoveTo(Vector3 position)
    {
        position.z = transform.position.z;
        position.y = transform.position.y;
        transform.position = Vector3.MoveTowards(transform.position, position, Time.deltaTime * _speed);

    }

    private void Alarm(bool _alarm, GameObject _enemy)
    {
        this._alarm = _alarm;
    }

    private void Hide(bool _hide, GameObject _enemy)
    {
        this._hide = _hide;
    }

    private void RestoreNeeds(Needs.NeedsCollection _needs, float _time)
    {
        StartCoroutine(NeedsReset());
        IEnumerator NeedsReset()
        {
            yield return new WaitForSeconds(_time);
            _npcNeeds[_needs] = 0;
            NeedsRestore.Invoke(_needs);
        }
        StopCoroutine(NeedsReset());
    }

}
using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "State/AttackState", fileName = "AttackState")]

public class AttackState : State
{

    public static event Action<Animations.PoliceMan, bool> SetAnim = delegate { };
    public static event Action TakeHit = delegate { };

    [Header("StateOptions")]
    [SerializeField] private float _distance;
    [SerializeField] private Transform _targetPos;
    [SerializeField] private GameObject _enemy;
    [SerializeField] private bool _alarm;
    [SerializeField] private float _policemanHitAnimLenth;
    [SerializeField] private float _timeHitAnimLength;

    [Header("Anim Options")]

    [SerializeField] private Animations.PoliceMan _moveAnim;
    [SerializeField] private Animations.PoliceMan _hitAnim;

    private MonoBehaviour _monoBehaviour;


    private void Awake()
    {
        _monoBehaviour = FindObjectOfType<MonoBehaviour>();
    }


    public override void Init()
    {
        if (IsFinished)
            return;
        if (_enemy != null)
        {
            _targetPos = _enemy.transform;
            SetAnim.Invoke(_moveAnim, SetRotate());
        }

    }

    public override void Run()
    {
        if (IsFinished) return;

        MoveToTarget();

    }

    private void OnEnable()
    {
        HideZone.ActivateHide += Hide;
        AlarmZoneDetector.AlarmTriggered += Alarm;

    }
    private void OnDisable()
    {
        HideZone.ActivateHide -= Hide;
        AlarmZoneDetector.AlarmTriggered -= Alarm;

    }

    private void Alarm(bool alarm, GameObject enemy)
    {

        if (alarm)
        {
            _enemy = enemy;
            _alarm = alarm;
            IsFinished = true;
            Init();
        }

    }

    private void Hide(bool hide, GameObject enemy)
    {
        _enemy = null;
        IsFinished = true;
        Init();
    }

    private void MoveToTarget()
    {
        try
        {
            var distance = (_targetPos.position - Character.transform.position).magnitude;
            if (_enemy != null && distance > _distance)
            {
                Character.MoveTo(_targetPos.position);

            }
            else if (_enemy != null && distance < _distance)
            {
                SetAnim.Invoke(_hitAnim, SetRotate());

                SeetPauseforAnim(_policemanHitAnimLenth);
                void SeetPauseforAnim(float _time)
                {
                    _monoBehaviour.StartCoroutine(Wait());
                    IEnumerator Wait()
                    {
                        yield return new WaitForSeconds(_time);
                        TakeHit.Invoke();
                    }
                    _monoBehaviour.StopCoroutine(Wait());
                }

            }
        }
        catch (Exception err)
        {
            Debug.Log($"{err}");
        }

    }

    private bool SetRotate()
    {
        bool rotate = false;

        Vector3 targetPos = Character.Cam.WorldToViewportPoint(_targetPos.position);
        Vector3 CharacterPos = Character.Cam.WorldToViewportPoint(Character.transform.position);

        if (targetPos.x < CharacterPos.x)
        {
            return rotate;
        }

        rotate = true;
        return rotate;
    }


}

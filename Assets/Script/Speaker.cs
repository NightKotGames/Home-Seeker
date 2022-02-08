using Spine.Unity;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(SkeletonAnimation))]
[RequireComponent(typeof(AudioSource), typeof(Animator))]

public class Speaker : MonoBehaviour
{

    [Header("Alarm Speaker MaxVolume: ")]
    [SerializeField] private float _maxVolume;

    private Animator _animator;
    private SkeletonAnimation _anim;
    private AudioSource _audio;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _anim = _animator.GetComponent<SkeletonAnimation>();
        _audio = GetComponent<AudioSource>();
    }

    public void OnEnable()
    {
        AlarmZoneDetector.Alarm += OnSetAlarm;
    }

    public void OnDisable()
    {
        AlarmZoneDetector.Alarm -= OnSetAlarm;
    }

    private void OnSetAlarm(bool alarmState, GameObject enemyObj)
    {
        if (alarmState)
        {
            AlarmON();
        }
        else if (!alarmState)
        {
            AlarmOFF();
        }
    }

    private void AlarmOFF()
    {
        float currentVolume = _maxVolume;
        StartCoroutine(SetVolume());
        IEnumerator SetVolume()
        {
            while (currentVolume > 0f)
            {

                yield return new WaitForSeconds(.2f);
                {
                    currentVolume -= _maxVolume / 10;
                }

                _audio.volume = currentVolume;
            }

            StopCoroutine(SetVolume());

            _animator.SetBool($"{Animations.Speaker.Alarm}", false);
            _anim.loop = true;
            _anim.Initialize(true);
            _audio.Stop();
        }

    }

    private void AlarmON()
    {

        _animator.SetBool($"{Animations.Speaker.Alarm}", true);
        _anim.loop = true;
        _anim.Initialize(true);
        _audio.Play();

        float currentVolume = 0f;
        StartCoroutine(SetVolume());
        IEnumerator SetVolume()
        {
            while (currentVolume < _maxVolume)
            {

                yield return new WaitForSeconds(.2f);
                {
                    currentVolume += _maxVolume / 10;
                }

                _audio.volume = currentVolume;
            }

            StopCoroutine(SetVolume());

        }

    }
}

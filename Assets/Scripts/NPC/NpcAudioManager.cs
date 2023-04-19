using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcAudioManager : MonoBehaviour
{
    private AudioSource _audioSource;

    [SerializeField] private AudioClip _walkClip;
    [SerializeField] private AudioClip _attackClip;
    [SerializeField] private AudioClip _damagingClip;
    [SerializeField] private AudioClip _damagedClip;

    private void Start()
    {
        _audioSource = GetComponentInChildren<AudioSource>();
    }

    private void Update()
    {
    }

    public void PlayStartWalkClip()
    {
        _audioSource.clip = _walkClip;
        _audioSource.Play();
    }

    public void PlayStopWalkClip()
    {
        _audioSource.clip = null;
    }

    public void PlaySwingClip_AnimEvent()
    {
        _audioSource.PlayOneShot(_attackClip);
    }

        public void PlayDamagingClip()
    {
        _audioSource.PlayOneShot(_damagingClip);
    }

    public void PlayDamagedClip()
    {
        _audioSource.PlayOneShot(_damagedClip);
    }
}

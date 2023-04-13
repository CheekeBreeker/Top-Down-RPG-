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
        if (GetComponent<NavMeshAgent>().speed > 0 &&
            !_audioSource.isPlaying)
        {
            _audioSource.clip = _walkClip;
            _audioSource.Play();
        }
        else if (GetComponent<NavMeshAgent>().speed <= 0 && Time.deltaTime != 0)
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

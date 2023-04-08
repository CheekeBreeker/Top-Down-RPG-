using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioListener _audioListener;
    private AudioSource _audioSource;

    [SerializeField] private AudioClip _walkClip;
    [SerializeField] private AudioClip _attackClip;
    [SerializeField] private AudioClip _damagingClip;
    [SerializeField] private AudioClip _damagedClip;

    private void Start()
    {
        _audioListener = GetComponent<AudioListener>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.W) ||
            Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.D))
            _audioSource.Play(_walkClip, transform.position);
        else _audioSource.clip = null;
    }

    public void PlaySwingClip()
    {
        _audioSource.PlayOneShot(_attackClip);
    }

    public void PlayDamagingClip()
    {

    }

    public void PlayDamagedClip()
    {

    }
}

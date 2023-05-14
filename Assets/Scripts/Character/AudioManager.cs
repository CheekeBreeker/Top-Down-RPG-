using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioSource _musicAudioSource;
    [SerializeField] private CharacterStatus _characterStatus;
    [SerializeField] private PlayerStats _playerStats;

    [SerializeField] private AudioClip _walkClip;
    [SerializeField] private AudioClip _attackClip;
    [SerializeField] private AudioClip _damagingClip;
    [SerializeField] private AudioClip _damagedClip;

    public List<AudioClip> musicAudioClips = new List<AudioClip>();

    // Задержка между треками
    public int newTrackDelay = 0;
    // Используется для отслеживания заглушения музыки
    private int _muted = 0;

    private void Start()
    {
        StartCoroutine(PlayBackgroudMusic());
        _musicAudioSource.volume = 0.3f;
    }

    private void Update()
    {
        if ((Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.W) ||
            Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.D)) &&
            !_audioSource.isPlaying)
        {
            _audioSource.clip = _walkClip;
            _audioSource.Play();
        }
        else if (!Input.GetKey(KeyCode.A) &&
            !Input.GetKey(KeyCode.W) &&
            !Input.GetKey(KeyCode.S) &&
            !Input.GetKey(KeyCode.D))
            _audioSource.clip = null;

        if (_characterStatus.isAttack || _characterStatus.isTalk || _characterStatus.isScream || _characterStatus.isDodge || _playerStats._health < _playerStats._maxHealth / 2) 
            StartCoroutine(TurnOnBackgroudMusic());
        else StartCoroutine(MuteBackgroudMusic());
    }
    
    IEnumerator PlayBackgroudMusic()
    {
        int musicIndex = 0;
        while (musicAudioClips.Count > 0)
        {
            float waitTime = musicAudioClips[musicIndex].length + newTrackDelay;
            _musicAudioSource.PlayOneShot(musicAudioClips[musicIndex]);

            musicIndex++;
            if (musicIndex >= musicAudioClips.Count)
            {
                musicIndex = 1;
            }

            yield return new WaitForSeconds(waitTime);
        }
    }

    IEnumerator MuteBackgroudMusic()
    {
        StopCoroutine(TurnOnBackgroudMusic());
        while (_musicAudioSource.volume > 0.3f)
        {
            _musicAudioSource.volume -= 0.01f;
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator TurnOnBackgroudMusic()
    {
        StopCoroutine(MuteBackgroudMusic());
        while (_musicAudioSource.volume < 0.75f)
        {
            _musicAudioSource.volume += 0.01f;
            yield return new WaitForSeconds(1f);
        }
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

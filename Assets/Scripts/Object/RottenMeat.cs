using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RottenMeat : MonoBehaviour
{
    [SerializeField] private AudioClip _stepOnClip;
    [SerializeField] private float _damage;
    [SerializeField] private float _timeToDamage = 1;

    private AudioSource _audioSource;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
            StartCoroutine(TakeAwayPlayerHealthCor(other.gameObject.GetComponent<PlayerStats>()));
        else if (other.CompareTag("Enemy") || other.CompareTag("Traider") || other.CompareTag("Freandly Npc"))
            StartCoroutine(TakeAwayNpcHealthCor(other.gameObject.GetComponent<NpcStats>()));
        else return;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopCoroutine(TakeAwayPlayerHealthCor(other.gameObject.GetComponent<PlayerStats>()));
            _timeToDamage = 1;
        }
        else if (other.CompareTag("Enemy") || other.CompareTag("Traider") || other.CompareTag("Freandly Npc"))
        {
            StopCoroutine(TakeAwayNpcHealthCor(other.gameObject.GetComponent<NpcStats>()));
            _timeToDamage = 1;
        }
    }

    IEnumerator TakeAwayPlayerHealthCor(PlayerStats playerStats)
    {
        if (_timeToDamage < 0)
        {
            playerStats.TakeAwayHealth(_damage);
            _audioSource.clip = _stepOnClip;
            _audioSource.Play();
            _timeToDamage = 1;
        }
        else _timeToDamage -= Time.deltaTime;
        yield return null;
    }

    IEnumerator TakeAwayNpcHealthCor(NpcStats npcStats)
    {
        npcStats.TakeAwayHealth(_damage);
        _audioSource.clip = _stepOnClip;
        _audioSource.Play();
        yield return new WaitForSeconds(100);
    }
}

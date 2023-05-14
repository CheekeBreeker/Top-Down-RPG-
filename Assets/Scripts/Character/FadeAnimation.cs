using Unity.VisualScripting;
using UnityEngine;

public class FadeAnimation : MonoBehaviour
{
    [DoNotSerialize] public Animator _anim;
    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    void FadeControllerDead_AnimEvent()
    {
        if (Time.timeScale == 1)
            Time.timeScale = 0;
        else Time.timeScale = 1;
    }
}

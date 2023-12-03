using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoundPlayer : MonoBehaviour
{

    [SerializeField] private AudioClip[] _steppingClips;
    [SerializeField] private AudioSource _fxAudioSource;

    public void PlayStep()
    {
        var index = Random.Range(0, _steppingClips.Length);
        _fxAudioSource.PlayOneShot(_steppingClips[index]);
    }
}

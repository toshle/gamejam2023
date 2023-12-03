using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip _battleMusic;
    [SerializeField] private AudioSource _battleMusicSource;
    [SerializeField] private AudioClip _ambientMusic;
    [SerializeField] private AudioSource _ambientMusicSource;

    public static SoundManager Instance;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void PlayAmbientMusic(bool play = true)
    {
        if (!play && _ambientMusicSource.enabled)
        {
            _ambientMusicSource.enabled = false;
        }
        else
        {
            _ambientMusicSource.clip = _ambientMusic;
            _ambientMusicSource.enabled = true;
            _ambientMusicSource.volume = 0.6f;
            _ambientMusicSource.Play();
        }
    }

    public void PlayBattleMusic(bool play = true)
    {
        if(!play)
        {
            _battleMusicSource.enabled = false;
        } else
        {
            _battleMusicSource.clip = _battleMusic;
            _battleMusicSource.enabled = true;
            _battleMusicSource.volume = 0.6f;
            _battleMusicSource.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

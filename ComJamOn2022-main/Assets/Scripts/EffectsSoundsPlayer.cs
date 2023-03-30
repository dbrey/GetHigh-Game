using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Sounds { AhhCorrect, BotonYApuntes, OhhhIncorrect, AQueSeJuega_V1, AQueSeJuega_V2, AQueSeJuega_V3, UITransition}

public class EffectsSoundsPlayer : MonoBehaviour
{
    private AudioSource audio;
    [SerializeField] AudioClip[] clips;

    public void PlayClip(PlaySoundSignal sound)
    {
        audio.PlayOneShot(clips[(int)sound.sound], sound.volume);
    }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        audio = GetComponent<AudioSource>();
        SignalBus<PlaySoundSignal>.Subscribe(PlayClip);
    }

    private void OnDestroy()
    {
        SignalBus<PlaySoundSignal>.Unsubscribe(PlayClip);
    }
}

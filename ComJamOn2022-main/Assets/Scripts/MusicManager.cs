using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public AudioClip sevilla;
    public AudioClip montana;

    AudioSource musica;
    AudioSource sound_effects;

    public EffectsSoundsPlayer sndManager;

    public static MusicManager instance;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            musica = GetComponent<AudioSource>();
            sndManager = GetComponent<EffectsSoundsPlayer>();
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else Destroy(gameObject);
    }


    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "Game Over":

                int random = Random.Range(0, 2);
                GetComponentInChildren<EffectsSoundsPlayer>().PlayClip(new PlaySoundSignal(Sounds.AQueSeJuega_V1 + random));
                musica.Stop();

                break;
            case "Escena Rioni":
                {
                    musica.clip = montana;
                    musica.Play();
                    break;
                }

            case "Modo Normal":
                {
                    musica.clip = sevilla;
                    musica.Play();
                    break;
                }
            
            case "Menu Inicio":
                {
                    musica.clip = sevilla;
                    musica.Play();
                    break;
                }

        }
    }
}

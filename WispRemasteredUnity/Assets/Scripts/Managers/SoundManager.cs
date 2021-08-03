using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    float masterVolumePercent = 0.05f;
    float sfxVolumePercent = 1f;
    float musicVolumePercent = 1f;

    bool sfxMuted = false;
    bool musicMuted = false;

    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioClip backgroundMusic;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PlayMusic(backgroundMusic);    
    }

    public void PlayMusic(AudioClip clip, float fadeDuration = 1)
    {
        musicSource.clip = clip;
        musicSource.Play();

        StartCoroutine(FadeMusic(fadeDuration));
    }

    public void PlaySound(AudioClip clip, Vector3 position)
    {
        if(!sfxMuted)
            AudioSource.PlayClipAtPoint(clip, position, masterVolumePercent * sfxVolumePercent);
    }

    IEnumerator FadeMusic(float duration)
    {
        float percent = 0;

        while(percent < 1)
        {
            percent += Time.deltaTime * 1 / duration;
            musicSource.volume = Mathf.Lerp(0, masterVolumePercent * musicVolumePercent, percent);
            yield return null;
        }
    }

    public void SetMusicMute(bool muted)
    {
        if(muted)
        {
            musicSource.volume = 0;
        }
        else
        {
            StartCoroutine(FadeMusic(1));
        }

        musicMuted = muted;
    }

    public void SetSfxMute(bool muted)
    {
        sfxMuted = muted;
    }
}

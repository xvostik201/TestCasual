using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private int _audioSystemCount = 10;
    private AudioSource[] _audioSystems;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioSystems();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeAudioSystems()
    {
        _audioSystems = new AudioSource[_audioSystemCount];
        for (int i = 0; i < _audioSystemCount; i++)
        {
            GameObject audioObject = new GameObject("AudioSystem_" + i);
            audioObject.transform.parent = this.transform;
            _audioSystems[i] = audioObject.AddComponent<AudioSource>();
        }
    }

    public void PlaySFX(AudioClip clip, float volume = 1.0f)
    {
        if (clip == null) return;

        foreach (var audioSystem in _audioSystems)
        {
            if (!audioSystem.isPlaying)
            {
                audioSystem.clip = clip;
                audioSystem.volume = volume;
                audioSystem.Play();
                return;
            }
        }

        _audioSystems[0].Stop();
        _audioSystems[0].clip = clip;
        _audioSystems[0].volume = volume;
        _audioSystems[0].Play();
    }

}

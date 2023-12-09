using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField, Range(1, 15)] private int _maxSfxTracks = 5;
    [SerializeField] private GameObject _audioObject;
    
    private AudioSource[] _sfxTracks;
    private int _curSfxIndex = 0;

    private AudioSource _bgm;
    public AudioSource BGM { get { return _bgm; } }

    private void Awake()
    {
        if (AudioManager.instance == null)
            AudioManager.instance = this;
        else if (AudioManager.instance != this)
            Destroy(gameObject);
        InitAudioSources();
        DontDestroyOnLoad(gameObject);
    }

    private void InitAudioSources()
    {
        _sfxTracks = new AudioSource[_maxSfxTracks];

        for (int i = 0; i < _maxSfxTracks; i++)
        {
            _sfxTracks[i] = gameObject.AddComponent<AudioSource>();
        }

        _bgm = gameObject.AddComponent<AudioSource>();
    }

    public void PlayBGM(AudioClip musicToPlay, float fadeDuration, bool isLooping = true, float volume = 0.5f)
    {
        //start coroutine
        StartCoroutine(PlayBGMCo(musicToPlay, fadeDuration, isLooping, volume));
    }

    private IEnumerator PlayBGMCo(AudioClip musicToPlay, float fadeDuration, bool isLooping = true, float volume = 0.5f)
    {
        Debug.Log("Music started.");
        float _oldVolume = _bgm.volume;
        AudioSource newBGM = gameObject.AddComponent<AudioSource>();
        newBGM.clip = musicToPlay;
        newBGM.loop = isLooping;
        newBGM.volume = 0;
        newBGM.Play();

        float t = 0;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float percent = t / fadeDuration;
            _bgm.volume = Mathf.Lerp(_oldVolume, 0, percent);
            newBGM.volume = Mathf.Lerp(0, volume, percent);

            yield return null;
        }

        Destroy(_bgm);
        _bgm = newBGM;

    }

    /// <summary>
    /// Set the SFX index clip to the desired clip
    /// Plays that clip
    /// Increases index by 1
    /// Resets the index when it is over the maximum
    /// </summary>
    /// <param name="clipToPlay">The clip you want to play</param>
    /// <param name="volume">The volume you want to set the clip to</param>
    public void PlaySFX(AudioClip clipToPlay, float volume = 1)
    {
        //PlayerSettingsManager _psm = PlayerSettingsManager.Instance;
        AudioSource _source = _sfxTracks[_curSfxIndex];
        //_source.volume = volume * _psm.SfxVolume * _psm.MasterVolume;
        _source.clip = clipToPlay;
        _source.Play();

        _curSfxIndex++;
        if (_curSfxIndex > _sfxTracks.Length - 1)
            _curSfxIndex = 0;
    }

    /// <summary>
    ///Make an audio object at the desired position
    /// Check if that object has an audio source
    /// if not, add one
    /// set the clip and the spatial blend
    /// </summary>
    /// <param name="clipToPlay">Desired clip</param>
    /// <param name="position">Desired location</param>
    /// <param name="volume">Volume, defaults to 1</param>
    /// <param name="spatialBlend">Spatial blend, defaults to 1 (3D sound)</param>
    public void PlaySFX(AudioClip clipToPlay, Vector3 position, float volume = 1, float spatialBlend = 1)
    {
        //PlayerSettingsManager _psm = PlayerSettingsManager.Instance;
        GameObject go = GameObject.Instantiate(_audioObject, position, Quaternion.identity);
        if(go.GetComponent<AudioSource>() == null)
        {
            go.AddComponent<AudioSource>();
        }

        AudioSource temp = go.GetComponent<AudioSource>();
        temp.clip = clipToPlay;
        //temp.volume = volume * _psm.SfxVolume * _psm.MasterVolume;
        temp.spatialBlend = spatialBlend;
        temp.Play();

        StartCoroutine(CleanUp(go, clipToPlay.length));
    }

    private IEnumerator CleanUp(GameObject go, float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(go);
    }

   
}

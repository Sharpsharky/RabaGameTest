using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer: MonoBehaviour{

    static MusicPlayer s_instance;
    public static MusicPlayer instance {
        get {
            return s_instance;
        }
    }

    MusicPlayerSettings _settings;

    AudioSource _audioSource;

    public float volume{
        get{
            return _audioSource.volume;
        }
    }

    public void SetSettings(MusicPlayerSettings pSettings){
        _settings = pSettings;
         PlayMusic();
    }

    public void RestoreMusicTrackForScene(){
        PlayMusic();
    }

    public void SetVolume(float pVolume){
        _audioSource.volume = pVolume;
    }

    public void SetMusicTrack(AudioClip pClip) {
        SetMusicTrack(pClip, _audioSource.volume);
    }

    public void SetMusicTrack(AudioClip pClip, float pVolume) {
        AudioClip previousClip = _audioSource.clip;

        _audioSource.loop = true;
        _audioSource.clip = pClip;
        _audioSource.volume = pVolume;

        if (pClip != previousClip) {
            if (ShouldPlayMusic()) {
                Debug.Log("Different clips so toggle audiosource to restart music");
                _audioSource.enabled = false;
                _audioSource.enabled = true;
            }else{
                _audioSource.enabled = false;
            }
        }else{
            _audioSource.enabled = ShouldPlayMusic();
        }
    }

    public void SetEnabled(bool pMusicEnabled) {
        _audioSource.enabled = pMusicEnabled;
    }

    void Awake(){
        s_instance = this;
        _audioSource = gameObject.GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable() {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
        PlayMusic();
    }

    bool ShouldPlayMusic() {
        if (PlayerPrefs.HasKey(Constants.kMusicPrefs)) {
            int shouldPlayMusic = PlayerPrefs.GetInt(Constants.kMusicPrefs);
            Debug.Log("Should play music in MusicPlayer: " + shouldPlayMusic);
            return (shouldPlayMusic == 1);
        } else {
            return true; // If no key play music.
        }
    }

    void PlayMusic() {
        if(_settings == null){
            _audioSource.enabled = false;
            return;
        }

      
        MusicPlayerSettings.SceneMusicSettings settings = _settings.GetMusicSettingsForScene(SceneManager.GetActiveScene().name);

        if (settings != null) {
            SetMusicTrack(settings.audioClip, settings.volume);
        }  
    }

}

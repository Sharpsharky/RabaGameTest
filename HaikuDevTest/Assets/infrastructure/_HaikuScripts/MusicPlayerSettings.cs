using UnityEngine;

public class MusicPlayerSettings : MonoBehaviour {
    
    [SerializeField]
    SceneMusicSettings[] _settings;

#if UNITY_EDITOR
    public SceneMusicSettings[] editorSettings{
        get{
            return _settings;
        }set{
            _settings = value;
        }
    }
#endif

    [System.Serializable]
    public class SceneMusicSettings{
        public string sceneName;
        public AudioClip audioClip;
        public float volume = 1f;
    }

    void Awake() {
        MusicPlayer musicPlayer = MusicPlayer.instance;
        if(musicPlayer == null){
            GameObject musicPlayerObj = new GameObject("MusicPlayer");
            musicPlayer = musicPlayerObj.AddComponent<MusicPlayer>();
           
        }

        musicPlayer.SetSettings(this);
    }

    public SceneMusicSettings GetMusicSettingsForScene(string pSceneName){
        foreach(SceneMusicSettings settings in _settings){
            if(settings.sceneName.Equals(pSceneName)){
                return settings;
            }
        }

        return null;
    }
}
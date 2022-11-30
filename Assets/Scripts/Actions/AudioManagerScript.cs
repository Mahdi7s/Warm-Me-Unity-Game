using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class AudioManagerScript : MonoBehaviour {
    // one of the bg musics can be selected randomaly
    public AudioClip MusicBackground1 = null;
    public AudioClip MusicBackground2 = null;
    
    // Fx Sounds
    public AudioClip FxHatOut = null;
    public AudioClip FxHatIn = null;
    
    public AudioClip FxMatchHit = null;
    
    public AudioClip FxCiggarettHit = null;
    
    public AudioClip FxIceHit = null;
    
    public AudioClip FxCocktailHit = null;
    
    public AudioClip FxEggBronzeHit = null;
    public AudioClip FxEggGoldenHit = null;
    
    public AudioClip FxFreezerHit = null;
    
    public AudioClip FxPipeFreeze = null;
    public AudioClip FxPipeExplode = null;
    
    public AudioClip FxLevelFail = null;
    public AudioClip FxLevelWin = null;
    
    
    // ---------------------- Fields ------------------
    private AudioClip[] _backgroundMusics = null;
    private AudioClip _selectedBgMusic = null;
    
    private bool _musicState = true;
    
    // --------------------- behaviours -----------------
    void Start()
    {
        _musicState = GameState.AudioMusic;
        
        _backgroundMusics = new[]
        {
            MusicBackground1,
            MusicBackground2
        };
    }
    
    void Update()
    {
        if (_musicState != GameState.AudioMusic)
        {
            _musicState = GameState.AudioMusic;
            if(_musicState)
            {
                PlayMusic();
            }else
            {
                StopMusic();
            }
        }
    }
    
    // --------------------------------------------------
    
    public float Volume 
    {
        get { return AudioListener.volume; }
        set { AudioListener.volume = value; }
    }
    
    public void PlayMusic()
    {
        if (GameState.AudioMusic)
        {
            if(_selectedBgMusic == null)
            {
                _selectedBgMusic = _backgroundMusics[Random.Range(0, _backgroundMusics.Length)];
            }
            AudioSource.PlayClipAtPoint(_selectedBgMusic, Vector3.zero);
        }
    }
    
    public void StopMusic()
    {
        AudioListener.pause = true;
    }
    
    public void PauseMusic()
    {
        AudioListener.pause = true;
    }
    
    public void ResumeMusic()
    {
        if (GameState.AudioMusic)
        {
            AudioListener.pause = false;
        }
    }
    
    public void PlayFx(GameActions action)
    {
        if (GameState.AudioFx)
        {
            var fxClip = GetFxClip(action);
            if(fxClip != null)
            {
                audio.PlayOneShot(fxClip);
            }
        }
    }
    
    // -------------------------- Privates ------------------------
    
    private AudioClip GetFxClip(GameActions action)
    {
        AudioClip ret = null;
        switch(action)
        {
            case GameActions.HatOut:
                ret = FxHatOut;
                break;
            case GameActions.HatIn:
                ret = FxHatIn;
                break;
                
            case GameActions.MatchHit:
                ret = FxMatchHit;
                break;
                
            case GameActions.CiggarettHit:
                ret = FxCiggarettHit;
                break;
                
            case GameActions.IceHit:
                ret = FxIceHit;
                break;
                
            case GameActions.CocktailHit:
                ret = FxCocktailHit;
                break;
                
            case GameActions.EggBronzeHit:
                ret = FxEggBronzeHit;
                break;
            case GameActions.EggGoldenHit:
                ret = FxEggGoldenHit;
                break;
                
            case GameActions.FreezerHit:
                ret = FxFreezerHit;
                break;
            case GameActions.PipeFreeze:
                ret = FxPipeFreeze;
                break;
            case GameActions.PipeExplode:
                ret = FxPipeExplode;
                break;
                
            case GameActions.LevelFail:
                ret = FxLevelFail;
                break;
            case GameActions.LevelWin:
                ret = FxLevelWin;
                break;
        }
        
        return ret;
    }
}

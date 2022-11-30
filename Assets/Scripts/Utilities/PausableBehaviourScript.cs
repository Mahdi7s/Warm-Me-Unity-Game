using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public abstract class PausableBehaviour : MonoBehaviour, IPausableObject
{
    protected bool _isInitialized = false;
    protected bool _isPaused = false;
    
    private Dictionary<GameObject, float> _animatorBeforPauseSpeeds = new Dictionary<GameObject, float>();
    private List<Animator> _animators = null;
    
    private Dictionary<GameObject, bool> _particlesBeforePauseState = new Dictionary<GameObject, bool>();
    private List<ParticleSystem> _particles = null;

    public bool PauseAnimations = true;
    public bool PauseParticles = true;
    
    public virtual void Initialize()
    {
        if (!_isInitialized)
        {
            _animators = GetComponentsInChildren<Animator>().ToList();
            _particles = GetComponentsInChildren<ParticleSystem>().ToList();
            
            _isInitialized = true;
        }
    }
    
    /*void OnApplicationPause()
    {
        OnPause();
    }

    void OnApplicationFocus()
    {
        OnResume();
    }*/
    
    public virtual void OnPause()
    {
        _isPaused = true;
        if (PauseAnimations)
        {
            FillAnimatorsSpeed(0.0f);
        }
        if (PauseParticles)
        {
            FillParticlesState(true);
        }
    }
    
    public virtual void OnResume()
    {
        _isPaused = false;
        if (PauseAnimations)
        {
            ResetAnimatorsSpeed();
        }
        if (PauseParticles)
        {
            ResetParticlesState();
        }
    }   
    
    protected virtual void Update()
    {
        if (!_isPaused)
        {
            PUpdate();
        }
    }
    
    protected virtual void FixedUpdate()
    {
        if (!_isPaused)
        {
            PFixedUpdate();
        }
    }
    
    protected virtual void OnGUI()
    {
        if (!_isPaused)
        {
            POnGUI();
        }
    }
    
    // -------------------- Privates ------------------------
    private void FillAnimatorsSpeed(float speed)
    {
        if (_animators != null)
        {
            foreach (var anim in _animators.Where(x=>x.gameObject != null))
            {
                _animatorBeforPauseSpeeds [anim.gameObject] = anim.speed;
                anim.speed = speed;
            }
        }
    }
    
    private void ResetAnimatorsSpeed()
    {
        if (_animators != null)
        { 
            try
            {
                foreach (var anim in _animators.Where(x=> x != null && x.gameObject != null))
                {
                    anim.speed = _animatorBeforPauseSpeeds [anim.gameObject];
                }
            } catch
            {
            }
        }
    }
    
    
    private void FillParticlesState(bool pause)
    {
        if (_particles != null)
        {
            foreach (var part in _particles.Where(x => x!= null && x.gameObject != null))
            {
                _particlesBeforePauseState [part.gameObject] = !part.isPlaying;
                if (pause && part.isPlaying)
                {
                    part.Pause(true);
                }
            }
        }
    }
    
    private void ResetParticlesState()
    {
        if (_particles != null)
        {
            try
            {
                foreach (var part in _particles.Where(x => x != null && x.gameObject != null))
                {
                    if (_particlesBeforePauseState [part.gameObject])
                    {
                        if (part.isPlaying)
                        {
                            part.Pause();
                        }
                    } else
                    {
                        if (!part.isPlaying)
                        {
                            part.Play(true);
                        }
                    }
                }
            } catch
            {
            }
        }
    }
    
    // -------------------- Replacement Methods ---------------------
    
    protected abstract void PUpdate();
    protected abstract void PFixedUpdate();
    protected abstract void POnGUI();
    
}
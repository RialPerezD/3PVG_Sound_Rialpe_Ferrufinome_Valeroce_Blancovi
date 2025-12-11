using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using System.Collections;


public class OneShotEmitter : MonoBehaviour
{
    [Header("FMOD")]
    /** <summary>
     * The FMOD event reference for the one-shot sound to be played.
     * </summary>
     */
    public EventReference soundEvent;
    [Header("FMOD Settings")]
    
    private EventInstance _soundInstance;

    /**
     * <summary>
     * The range of time intervals (in seconds) between one-shot sound plays.
     * </summary>
     */
    public Vector2 timeRange = new Vector2(3.0f, 7.0f);
    
    /**
     * <summary>
     * The minimum pitch variation for the one-shot sound.
     * </summary>
     */
    public float minPitch = 0.95f;
    /**
     * <summary>
     * The maximum pitch variation for the one-shot sound.
     * </summary>
     */
    public float maxPitch = 1.05f;
    
    
    void Start()
    {
        StartCoroutine(PlayRoutine());

    }

    /**
     * <summary>
     * Coroutine that plays the one-shot sound at random intervals.
     * </summary>
     */
    private IEnumerator PlayRoutine()
    {
        while (true)
        {
            float delay = Random.Range(timeRange.x, timeRange.y);
            yield return new WaitForSeconds(delay);

            PlayOneShot3D();
        }
    }
    
    /**
     * <summary>
     * Plays the one-shot sound in 3D space with random pitch variation.
     * </summary>
     */
    private void PlayOneShot3D()
    {
        _soundInstance =  RuntimeManager.CreateInstance(soundEvent);
        
        float randomPitch = Random.Range(minPitch, maxPitch);
        _soundInstance.setPitch(randomPitch);
        
        _soundInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
        _soundInstance.start();
        _soundInstance.release();
    }
 }


using UnityEngine;
using FMODUnity;
using FMOD.Studio;

[RequireComponent(typeof(BoxCollider))]
public class MusicCrossFader : MonoBehaviour
{
    /// <summary>
    /// Name of the FMOD parameter that controls the music blend.
    /// </summary>
    private const string BlendParameter = "Mezcla";

    /// <summary>
    /// FMOD event reference used for the music instance.
    /// </summary>
    [Header("FMOD Settings")]
    public EventReference musicEvent;

    /// <summary>
    /// Target blend value (0–1) toward which the system transitions.
    /// </summary>
    [Header("Fade Settings")]
    [Range(0f, 1f)]
    public float targetBlend = 0f;

    /// <summary>
    /// Speed at which the blend transitions toward the target value.
    /// </summary>
    [Tooltip("Transition Speed")]
    public float transitionSpeed = 1.0f;

    /// <summary>
    /// Optional reference to an EpicTrigger script to adjust its volume based on the blend value.
    /// </summary>
    [Header("References")]
    public EpicTrigger epicTriggerScript;

    /// <summary>
    /// Optional reference to a Layerer script to adjust its volume based on the blend value.
    /// </summary>
    public Layerer layererScript;

    /// <summary>
    /// Runtime FMOD event instance controlling the music.
    /// </summary>
    private EventInstance musicInstance;

    /// <summary>
    /// Current blend value (0–1), interpolated toward <see cref="targetBlend"/>.
    /// </summary>
    private float currentBlend = 0.0f;

    /// <summary>
    /// Initializes the FMOD music instance and sets initial parameter values.
    /// </summary>
    private void Start()
    {
        // Create and start FMOD event instance
        musicInstance = RuntimeManager.CreateInstance(musicEvent);
        musicInstance.start();

        // Initialize blend and volume
        musicInstance.setParameterByName(BlendParameter, 0f);
        musicInstance.setVolume(0f);

        currentBlend = 0f;
        targetBlend = 0f;
    }

    /// <summary>
    /// Updates the blend transition and handles input-based debug switching.
    /// </summary>
    private void Update()
    {
        // Smooth blend transition
        if (Mathf.Abs(currentBlend - targetBlend) > 0.001f)
        {
            currentBlend = Mathf.MoveTowards(currentBlend, targetBlend, transitionSpeed * Time.deltaTime);

            // Apply updated blend value to FMOD
            musicInstance.setParameterByName(BlendParameter, currentBlend);
            musicInstance.setVolume(currentBlend);

            // Update linked systems inversely
            if (epicTriggerScript)
                epicTriggerScript.SetVolume(1f - currentBlend);

            if (layererScript)
                layererScript.SetVolume(1f - currentBlend);
        }

        // Manual debug controls
        if (Input.GetKeyDown(KeyCode.I)) SwapToA();
        if (Input.GetKeyDown(KeyCode.O)) SwapToB();
    }

    /// <summary>
    /// Switches blend toward music A (sets target blend to 0).
    /// </summary>
    public void SwapToA()
    {
        targetBlend = 0f;
    }

    /// <summary>
    /// Switches blend toward music B (sets target blend to 1).
    /// </summary>
    public void SwapToB()
    {
        targetBlend = 1f;
    }

    /// <summary>
    /// Automatically swaps to B when an object enters the trigger zone.
    /// </summary>
    /// <param name="other">Collider entering the trigger.</param>
    private void OnTriggerEnter(Collider other)
    {
        SwapToB();
    }

    /// <summary>
    /// Automatically swaps back to A when an object exits the trigger zone.
    /// </summary>
    /// <param name="other">Collider exiting the trigger.</param>
    private void OnTriggerExit(Collider other)
    {
        SwapToA();
    }

    /// <summary>
    /// Ensures FMOD instance is properly stopped and released when this object is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        musicInstance.release();
    }
}

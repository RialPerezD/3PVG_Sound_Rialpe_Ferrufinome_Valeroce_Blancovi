using UnityEngine;
using FMODUnity;
using FMOD.Studio;

/// <summary>
/// Manages the smooth crossfade transition between two music states using FMOD.
/// </summary>
/// <remarks>
/// This component requires the assigned FMOD Event to have a parameter named "Blend".
/// The parameter controls the mix: 0.0f for Song A and 1.0f for Song B.
/// </remarks>
public class MusicCrossFader : MonoBehaviour
{
    /// <summary>
    /// The name of the parameter in FMOD Studio that controls the blend.
    /// </summary>
    private const string BlendParameter = "Blend";

    /// <summary>
    /// Reference to the specific FMOD music event.
    /// </summary>
    [Header("FMOD")]
    public EventReference musicEvent;

    /// <summary>
    /// The target value for the blend parameter.
    /// <list type="bullet">
    /// <item><description>0.0f = Song A</description></item>
    /// <item><description>1.0f = Song B</description></item>
    /// </list>
    /// </summary>
    [Header("Fade blend")]
    [Range(0f, 1f)]
    public float targetBlend = 0f;

    /// <summary>
    /// The speed at which the transition between songs occurs.
    /// A higher value results in a faster transition.
    /// </summary>
    [Tooltip("How fast the fade's blend goes")]
    public float transitionSpeed = 2.0f;

    /// <summary>
    /// Sets the blend target to 0 (Song A).
    /// </summary>
    public void SwapToA()
    {
        targetBlend = 0f;
    }

    /// <summary>
    /// Sets the blend target to 1 (Song B).
    /// </summary>
    public void SwapToB()
    {
        targetBlend = 1f;
    }

    /// <summary>
    /// The runtime instance of the FMOD event.
    /// </summary>
    private EventInstance musicInstance;

    /// <summary>
    /// The current value of the blend parameter (will interpolate towards <see cref="targetBlend"/>).
    /// </summary>
    private float currentBlend = 0.0f;

    /// <summary>
    /// Initializes the FMOD instance and starts playback.
    /// </summary>
    private void Start()
    {
        // Create and initialize music's instance
        musicInstance = RuntimeManager.CreateInstance(musicEvent);
        musicInstance.start();

        // Initialize blend parameter to 0 (Song A)
        musicInstance.setParameterByName(BlendParameter, 0f);
    }

    /// <summary>
    /// Updates the blend interpolation and handles debug input.
    /// </summary>
    private void Update()
    {
        // Linear interpolation (Lerp) of the blend value
        if (Mathf.Abs(currentBlend - targetBlend) > 0.001f)
        {
            currentBlend = Mathf.MoveTowards(currentBlend, targetBlend, transitionSpeed * Time.deltaTime);

            // Set FMOD parameter
            musicInstance.setParameterByName(BlendParameter, currentBlend);
        }

        if (Input.GetKeyDown(KeyCode.A)) SwapToA();
        if (Input.GetKeyDown(KeyCode.B)) SwapToB();
    }

    /// <summary>
    /// Stops and releases the FMOD instance when the object is destroyed to prevent memory leaks.
    /// </summary>
    private void OnDestroy()
    {
        musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        musicInstance.release();
    }
}
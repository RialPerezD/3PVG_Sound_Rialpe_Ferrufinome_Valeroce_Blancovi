using FMOD.Studio;
using FMODUnity;
using UnityEngine;

/// <summary>
///   <para>Trigger that implements branching (changes state when the player enters and exits).</para>
/// </summary>
[RequireComponent(typeof(BoxCollider))]
public class EpicTrigger : MonoBehaviour
{
    /// <summary>
    ///   <para>Instance of the event created in FMOD.</para>
    /// </summary>
    [Header("FMOD")]
    public EventReference musicEvent;

    /// <summary>
    ///   <para>FMOD parameter name to decrease the volume of ambience sounds.</para>
    /// </summary>
    [Header("FMOD Settings")] 
    public string globalParamName = "AMB_DUCK";

    /// <summary>
    ///   <para>Sets the volume of the branching event.</para>
    /// </summary>
    /// <param name="volume">Value between 0 and 1 that determines the volume of the branching event.</param>
    public void SetVolume(float volume)
    {
        _musicInstance.setVolume(volume);
    }

    private EventInstance _musicInstance;
    private const string StateParameter = "LitvarState";
    private const string StateSad = "Sad";
    private const string StateEpic = "Epic";

    private void Start()
    {
        _musicInstance = RuntimeManager.CreateInstance(musicEvent);
        _musicInstance.start();
    }

    private void OnDestroy()
    {
        _musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        _musicInstance.release();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Epic Music
        _musicInstance.setParameterByNameWithLabel(StateParameter, StateEpic);
        RuntimeManager.StudioSystem.setParameterByName(globalParamName, 1.0f);
    }

    private void OnTriggerExit(Collider other)
    {
        // Sad Music
        _musicInstance.setParameterByNameWithLabel(StateParameter, StateSad);
        RuntimeManager.StudioSystem.setParameterByName(globalParamName, 0.0f);
    }
}
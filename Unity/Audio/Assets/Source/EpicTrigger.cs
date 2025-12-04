using FMOD.Studio;
using FMODUnity;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class EpicTrigger : MonoBehaviour
{
    [Header("FMOD")] public EventReference musicEvent;

    private EventInstance _musicInstance;
    
    private const string StateParameter = "LitvarState";
    private const string StateSad = "Sad";
    private const string StateEpic = "Epic";

    private void Start()
    {
        _musicInstance = RuntimeManager.CreateInstance(musicEvent);
        _musicInstance.start();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Epic Music
        _musicInstance.setParameterByNameWithLabel(StateParameter, StateEpic);
    }

    private void OnTriggerExit(Collider other)
    {
        // Sad Music
        _musicInstance.setParameterByNameWithLabel(StateParameter, StateSad);
    }
}
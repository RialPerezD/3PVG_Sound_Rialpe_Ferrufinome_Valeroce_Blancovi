using FMOD.Studio;
using FMODUnity;
using UnityEngine;

/// <summary>
///   <para>Class to test branching in FMOD using left and right arrows to change between states.</para>
/// </summary>
public class Brancher : MonoBehaviour
{
    [Header("FMOD")] public EventReference musicEvent;

    /// <summary>
    ///   <para>Updates the state of branching to exploration.</para>
    /// </summary>
    public void BranchToExploration()
    {
        _musicInstance.setParameterByNameWithLabel(StateParameter, StateExploration);
    }

    /// <summary>
    ///   <para>Updates the state of branching to combat.</para>
    /// </summary>
    public void BranchToCombat()
    {
        _musicInstance.setParameterByNameWithLabel(StateParameter, StateCombat);
    }

    private const string StateParameter = "State";
    private const string StateExploration = "Exploration";
    private const string StateCombat = "Combat";
    private EventInstance _musicInstance;

    private void Start()
    {
        _musicInstance = RuntimeManager.CreateInstance(musicEvent);
        _musicInstance.start();
        BranchToExploration();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            BranchToExploration();
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            BranchToCombat();
    }
}
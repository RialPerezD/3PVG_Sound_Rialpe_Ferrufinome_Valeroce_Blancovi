using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using System.Collections.Generic;

public class Layerer : MonoBehaviour
{
    [Header("FMOD")]
    /** <summary>
     * The FMOD event reference for the music event to be layered.
     * </summary>
     */
    public EventReference musicEvent;

    /** <summary>
     * The FMOD event instance for the music event.
     * </summary>
     */
    public EventInstance _musicInstance;

    /** <summary>
     * Represents a music layer with its name, parameter label, current value, and target value.
     * </summary>
     */
    [System.Serializable]
    public struct Layer
    {
        /** <summary>
         * The name of the music layer.
         * </summary>
         */
        public string name;
        /** <summary>
         * The FMOD parameter label associated with this layer.
         * </summary>
         */
        public string parameterLabel;
        /** <summary>
         * The current value of the layer's parameter.
         * </summary>
         */
        [HideInInspector] public float currentValue;
        /** <summary>
         * The target value for the layer's parameter.
         * </summary>
         */
        [HideInInspector] public float targetValue;
    }

    /** <summary>
     * An array of music layers to be managed.
     * </summary>
     */
    public Layer[] layers;
    /** <summary>
     * The speed at which layers fade in and out.
     * </summary>
     */
    public float fadeSpeed = 3.0f;

    #region LayerToggling

    /**
     * <summary>
     * Toggles the specified music layer on or off.
     * </summary>
     * 
     * <param name = "layerName">
     * The name of the layer to toggle.
     * </param>
     */
    public void ToggleLayer(string layerName)
    {
        for (int i = 0; i < layers.Length; i++)
        {
            if (layers[i].parameterLabel != layerName) continue;

            Layer layer = layers[i];
            layer.targetValue = (layer.targetValue == 0.0f) ? 1.0f : 0.0f;
            layers[i] = layer;
            return;
        }
    }

    #endregion

    /**
     * <summary>
     * Sets the overall volume of the music instance.
     * This is somewhat legacy since this class was more for testing.
     * </summary>
     * 
     * <param name = "volume">
     * The desired volume level (0.0f to 1.0f).
     * </param>
     */
    public void SetVolume(float volume)
    {
        _musicInstance.setVolume(volume);
    }

    private void Start()
    {
        _musicInstance = RuntimeManager.CreateInstance(musicEvent);
        _musicInstance.start();

        for (int i = 0; i < layers.Length; i++)
        {
            layers[i].currentValue = 0.0f;
            layers[i].targetValue = 1.0f;
        }

        layers[1].targetValue = 0.0f;
    }

    private void Update()
    {
        for (int i = 0; i < layers.Length; i++)
        {
            Layer layer = layers[i];

            if (layer.currentValue < layer.targetValue)
            {
                layer.currentValue =
                    Mathf.MoveTowards(layer.currentValue, layer.targetValue, Time.deltaTime * fadeSpeed);

                _musicInstance.setParameterByName(layer.parameterLabel, layer.currentValue);
                layers[i] = layer;
            }
        }
    }
}
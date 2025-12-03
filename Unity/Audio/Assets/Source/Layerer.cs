using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using System.Collections.Generic;

public class Layerer : MonoBehaviour
{
    [Header("FMOD")] public EventReference musicEvent;

    public EventInstance _musicInstance;

    private Dictionary<string, bool> _layerStates = new Dictionary<string, bool>();
    
    [System.Serializable]
    public struct Layer
    {
        public string name;
        public string parameterLabel;
        [HideInInspector] public float currentValue;
        [HideInInspector] public float targetValue;    
    }

    public Layer[] layers;
    public float fadeSpeed = 3.0f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _musicInstance = RuntimeManager.CreateInstance(musicEvent); 
        _musicInstance.start();
        
        for (int i = 0; i < layers.Length; i++) {
            //layers[i].currentValue = 0.0f;
            layers[i].targetValue = 0.0f;
            _layerStates.Add(layers[i].parameterLabel, false);
        }
        
        
    }
    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < layers.Length; i++)
        {
            Layer layer = layers[i];

            if (layer.currentValue < layer.targetValue) {
                layer.currentValue = Mathf.MoveTowards(layer.currentValue, layer.targetValue, Time.deltaTime * fadeSpeed);
                
                _musicInstance.setParameterByName(layer.parameterLabel, layer.currentValue);
                layers[i] = layer;
            }
        }

        if (layers.Length > 0) {
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                ToggleLayer(layers[0].parameterLabel);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                ToggleLayer(layers[1].parameterLabel);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3)) {
                ToggleLayer(layers[2].parameterLabel);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4)) {
                ToggleLayer(layers[3].parameterLabel);
            }
        }
    }

    #region LayerToggling

    public void ToggleLayer(string layerName) {
        if (_layerStates.ContainsKey(layerName))
        {
            _layerStates[layerName] = !_layerStates[layerName];
            float newValue = _layerStates[layerName] ? 1.0f : 0.0f;
            
            _musicInstance.setParameterByName(layerName, newValue);
        } else {
            Debug.LogError($"Parameter '{layerName}' not found in layer states.");
        }
    }
    
    private void SetLayerTarget(string layerName, float targetValue) {
        for (int i = 0; i < layers.Length; i++) {
            if (layers[i].parameterLabel == layerName) {
                layers[i].targetValue = targetValue;
                return;
            }
        }
    }

    #endregion
}

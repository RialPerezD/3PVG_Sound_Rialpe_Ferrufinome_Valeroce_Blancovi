using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RandomLayerToggling : MonoBehaviour
{
    [Header("Refrences")] public Layerer layersController;

    [Header("Settings")] 
    /** <summary>
     * The names of the layers to be toggled randomly.
     * </summary>
     */
    public string[] layerNames;
    /** <summary>
     * The range of time intervals (in seconds) for which a layer stays off.
     * </summary>
     */
    public Vector2 timeOffRange = new Vector2(10.0f, 15.0f);
    /** <summary>
     * The range of time intervals (in seconds) for which a layer stays on.
     * </summary>
     */
    public Vector2 timeOnRange = new Vector2(8.0f, 10.0f);
    
    
    private void Start()
    {
        StartCoroutine(RandomToggleRoutine());
    }

    /**
     * <summary>
     * Coroutine that randomly toggles a specified layer on and off at random intervals.
     * </summary>
     */
    IEnumerator RandomToggleRoutine()
    {
        while (true)
        {
            float waitOff = Random.Range(timeOffRange.x, timeOffRange.y);
            yield return new WaitForSeconds(waitOff);
            if (layersController)
            {
                layersController.ToggleLayer(layerNames[0]);
            }
            
            float waitOn = Random.Range(timeOnRange.x, timeOnRange.y);
            yield return new WaitForSeconds(waitOn);
            if (layersController)
            {
                layersController.ToggleLayer(layerNames[0]);
            }
        }
    }
    
}

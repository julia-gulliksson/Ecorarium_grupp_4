using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    //Refrences
    [SerializeField] private Light sun;
    [SerializeField] private LightPreset preset;
    [SerializeField] private Material skyBox;
    private Shader skyBoxShader;
    //Variables
    [SerializeField, Range(0, 1)] private float dayNightTime;
    [SerializeField] private float timeResolution;
    [SerializeField] private float duskDawnTime;

    private void Start()
    {
        GameEventsManager.current.OnDay += Day;
        GameEventsManager.current.OnNight += Night;
    }
    private void OnDisable()
    {
        GameEventsManager.current.OnDay -= Day;
        GameEventsManager.current.OnNight -= Night;
    }
    private void Update()
    {
        /*
        if (preset == null)
            return;
        if (Application.isPlaying)
        {
            UpdateLight(dayNightTime);
        }
        else
        {
            UpdateLight(dayNightTime);
        }*/
    }

    private void Day()
    {
        
        StartCoroutine(Dawn(duskDawnTime));
    }
    private void Night()
    {
        
        StartCoroutine(Dusk(duskDawnTime));
    }
    private void UpdateLight(float time)
    {
        RenderSettings.ambientLight = preset.ambientColor.Evaluate(time);
        RenderSettings.fogColor = preset.fogColor.Evaluate(time);
        if(sun != null)
        {
            sun.color = preset.directionalColor.Evaluate(time);
        }
        if (skyBox != null)
        {
            RenderSettings.skybox.SetColor("_SkyTint", preset.skyBoxTint.Evaluate(time));
            RenderSettings.skybox.SetFloat("_AtmosphereThickness",preset.atmoThickness-time+(time*0.4f));
        }
       
    }

    private void OnValidate()
    {
        if (sun != null && skyBox != null)
            return;

        if (RenderSettings.skybox != null)
        {
            skyBox = RenderSettings.skybox;
        }
        if (RenderSettings.sun != null)
        {
            sun = RenderSettings.sun;
        }
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach(Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    sun = light;
                    return;
                }
            }
        }
        
       
    }

    private IEnumerator Dawn(float fadeTime)
    {
        float increment = 1 / fadeTime;
        for (int i = Mathf.RoundToInt(fadeTime); i >= 0; i--)
        {
            dayNightTime = increment * i;
            UpdateLight(dayNightTime);
            yield return new WaitForSeconds(timeResolution);
        }
    }
    private IEnumerator Dusk(float fadeTime)
    {
        float increment = 1 / fadeTime;
        for (int i = 0; i <= fadeTime; i++)
        {
            dayNightTime = increment * i;
            UpdateLight(dayNightTime);
            yield return new WaitForSeconds(timeResolution);
        }
    }
}

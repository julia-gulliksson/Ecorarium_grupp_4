using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Light Preset", menuName = "Scriptable/ Light Preset", order = 1)]
public class LightPreset : ScriptableObject
{
    public Gradient ambientColor;
    public Gradient directionalColor;
    public Gradient fogColor;
    public Gradient skyBoxTint;
    public Gradient skyBoxGround;
    [Range(0.8f, 0.25f)] public float atmoThickness;
}

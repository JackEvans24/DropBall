using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundObject", menuName = "ScriptableObjects/SoundObject", order = 2)]
public class SoundOption : ScriptableObject
{
    public string DisplayName;
    public AudioClip Sound;
}

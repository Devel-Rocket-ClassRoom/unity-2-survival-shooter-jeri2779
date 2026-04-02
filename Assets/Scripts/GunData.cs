using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "Scriptable Objects/GunData")]
public class GunData : ScriptableObject
{
    public AudioClip shotClip;

    public float shotInterval = 0.12f; //연사 속도
    public float damage = 25f;
}
 
 
 

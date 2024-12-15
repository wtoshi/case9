using UnityEngine;


[CreateAssetMenu(fileName = "NewEffect", menuName = "Magical Tower/Effects/New Efffect")]
public class Effect : ScriptableObject
{
    public GameEntries.EFFECT_TYPE effectType;

    public int MinDamage;
    public int MaxDamage;
}
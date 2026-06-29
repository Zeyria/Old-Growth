using UnityEngine;

[CreateAssetMenu(fileName = "ActionScriptableObject", menuName = "Scriptable Objects/ActionScriptableObject")]
public class ActionScriptableObject : ScriptableObject
{
    public int targetingType; // 0- Enemy 1- Ally 2- Open
    public float damageMult;
    public bool stun;
    public bool slow;
    public bool summon;
    public int range;
    public int minRange;
    public int AP;
    public GameObject soundPrefab;
    public GameObject animationPrefab;
    public GameObject lastingEffectPrefab;
    public Sprite iconSprite;
    public bool AIUseWhenWander;
    public string description;
}

using UnityEngine;

public class EffectVisual : MonoBehaviour
{
    public int roundLifeSpan;
    private int rounds = 0;
    public void RoundTick()
    {
        rounds++;
        if (rounds >= roundLifeSpan)
        {
            Destroy(this.gameObject);
        }
    }
}

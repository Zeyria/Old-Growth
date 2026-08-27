using UnityEngine;
using TMPro;
public class WeekText : MonoBehaviour
{
    public TMP_Text text;
    void Update()
    {
        text.text = "Week " + (1 + TownResources.week).ToString();
    }
}

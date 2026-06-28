using UnityEngine;
using TMPro;

public class CapText : MonoBehaviour
{
    public GameObject roster;
    public int cap;
    public TMP_Text text;
    private void Update()
    {
        text.text = roster.transform.childCount.ToString() + " / " + cap.ToString();
    }
}

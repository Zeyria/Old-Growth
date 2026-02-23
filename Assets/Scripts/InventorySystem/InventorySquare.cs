using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySquare : MonoBehaviour
{
    public int x;
    public int y;
    public int value;
    /* For Debuging
    private void Update()
    {
        if(value == 0)
        {
            gameObject.GetComponent<Image>().color = Color.white;
        }
        else
        {
            gameObject.GetComponent<Image>().color = Color.red;
        }
    }
    */
    public override string ToString()
    {
        return x + "," + y;
    }
}

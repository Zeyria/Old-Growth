using UnityEngine;

public class SetKidScale : MonoBehaviour
{
    void Update()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).localScale = new Vector3(1, 1, 1);
        }
    }
}

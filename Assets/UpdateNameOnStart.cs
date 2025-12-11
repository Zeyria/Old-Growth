using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateNameOnStart : MonoBehaviour
{

    void Start()
    {
        transform.GetChild(2).GetComponent<TMP_Text>().text = transform.parent.GetComponent<UnitStats>().unitName;
    }
}

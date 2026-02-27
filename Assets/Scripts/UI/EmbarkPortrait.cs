using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
public class EmbarkPortrait : MonoBehaviour, IPointerClickHandler
{
    public int slotNum;
    public FillSelect fillSelect;
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (pointerEventData.button == PointerEventData.InputButton.Right)
        {
            ClearSlot();
        }
    }
    public void ClearSlot()
    {
        UnitSelectButton.slotTracker[slotNum] = 0;
        UnitSelectButton.slotsPortraits[slotNum].GetComponent<RawImage>().texture = null;
        UnitSelectButton.slotsPortraits[slotNum].GetComponent<RawImage>().color = Color.clear;

        int layerObject = 1;
        Vector2 ray = UnitSelectButton.slots[slotNum].transform.position;
        RaycastHit2D hit = Physics2D.Raycast(ray, ray, layerObject);


        if (hit.collider != null)
        {
            for (int x = 0; x <= fillSelect.availableUnits.Count - 1; x++)
            {
                if (hit.collider.gameObject.name == fillSelect.transform.GetChild(x).transform.GetChild(4).GetChild(1).GetComponent<TMP_Text>().text)
                {
                    fillSelect.transform.GetChild(x).GetComponent<UnitSelectButton>().amount += 1;
                    fillSelect.transform.GetChild(x).GetChild(6).gameObject.SetActive(false);
                }
            }
            Destroy(hit.collider.gameObject);
        }
    }
}

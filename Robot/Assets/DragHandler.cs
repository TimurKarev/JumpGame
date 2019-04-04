using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private const string DRAGGBLE_PREFAB_PATH = "UI/CommandPanel/DraggblePlane";

    private const string UI_EDIT_PANEL_BEFORE = "UIEditPanelBefore";
    private const string UI_EDIT_PANEL_AFTER = "UIEditPanelAfter";
    private const string UI_EDIT_PANEL_CENTER = "UIEditPanelCenter";

    private Color32 testColor = new Color32(160, 255, 166, 255);

    Vector3 startPosition;

    private bool isDragging = false;
    private GameObject draggingObject = null;

    void Start()
    {
        startPosition = transform.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging == false)
        {
            draggingObject = Instantiate(Resources.Load(DRAGGBLE_PREFAB_PATH)) as GameObject;
            if (draggingObject != null)
            {
                isDragging = true;

                draggingObject.transform.SetParent(this.transform);

                Sprite sprite = this.transform.GetChild(0).GetComponent<Image>().sprite;
                draggingObject.transform.GetChild(0).GetComponent<Image>().sprite = sprite;
            }  
        }

        if (draggingObject == null)
        {
            return;
        }

        draggingObject.transform.position = Input.mousePosition;
        GameObject gObject = GetObjectUnderMouse();
        if (gObject != null)
        {
            gObject.GetComponent<Image>().color = testColor;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;

        GameObject gameObject = GetObjectUnderMouse();
        
        if (draggingObject != null || gameObject == null)
        {
            Destroy(draggingObject);
        }
    }

    private GameObject GetObjectUnderMouse()
    {
        var pointer = new PointerEventData(EventSystem.current);

        pointer.position = Input.mousePosition;

        List<RaycastResult> hitObjects = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, hitObjects);

        if (hitObjects.Count <= 0) return null;

        foreach (RaycastResult hitObject in hitObjects)
        {
            if (hitObject.gameObject.tag == UI_EDIT_PANEL_AFTER)
            {
                Debug.Log(UI_EDIT_PANEL_AFTER);
                return hitObject.gameObject;
            }
            if (hitObject.gameObject.tag == UI_EDIT_PANEL_BEFORE)
            {
                Debug.Log(UI_EDIT_PANEL_BEFORE);
                return hitObject.gameObject;
            }
            if (hitObject.gameObject.tag == UI_EDIT_PANEL_CENTER)
            {
                Debug.Log(UI_EDIT_PANEL_CENTER);
                return hitObject.gameObject;
            }
        }
        return null;
    }
}
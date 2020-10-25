using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImageDrap : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    public BallController ballcontroller;

    public RectTransform boudingbox;

    Vector3 offsetPos;

    void Start()
    {

    }

    void Update()
    {
        float x0 = boudingbox.position.x - boudingbox.rect.width / 2;
        float y0 = boudingbox.position.y - boudingbox.rect.height / 2;
        float xratio = (transform.position.x - x0) / boudingbox.rect.width;
        float yratio = (transform.position.y - y0) / boudingbox.rect.height;

        ballcontroller.ChangeXZ(xratio, yratio);
    }

    private void FixedUpdate()
    {
        Vector3 pos = transform.position;
        if (Input.GetKey(KeyCode.W))
        {
            pos.y += 1;
        }

        //s+Êó±êÓÒ¼ü³¡¾°ÂþÓÎ
        if (Input.GetKey(KeyCode.S))
        {
            pos.y -= 1;
        }
        //a+Êó±êÓÒ¼ü³¡¾°ÂþÓÎ
        if (Input.GetKey(KeyCode.A))
        {
            pos.x -= 1;

        }
        //d+Êó±êÓÒ¼ü³¡¾°ÂþÓÎ
        if (Input.GetKey(KeyCode.D))
        {
            pos.x += 1;
        }
       

        if (InBoundingBox(pos))
        {
            transform.position = pos;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 newpos = new Vector3(Mathf.Clamp(Input.mousePosition.x, 0, Screen.width), Mathf.Clamp(Input.mousePosition.y, 0, Screen.height), 0) + offsetPos;
        if (InBoundingBox(newpos))
        {
            transform.position = newpos;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        offsetPos = transform.position - Input.mousePosition;
    }

    bool InBoundingBox(Vector3 newpos)
    {
        bool xvalid = newpos.x > (boudingbox.position.x - boudingbox.rect.width / 2) && newpos.x <= (boudingbox.position.x + boudingbox.rect.width / 2);
        bool yvalid = newpos.y > (boudingbox.position.y - boudingbox.rect.height / 2) && newpos.y <= (boudingbox.position.y + boudingbox.rect.height / 2);
        return xvalid && yvalid;
    }
}




using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StartImage : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        GameController.Instance.startGame = true;
        GameController.Instance.startImage.gameObject.transform.parent.gameObject.SetActive(false);
        GameController.Instance.direction = 0;
    }
}

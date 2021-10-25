using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointsInfoPanels : MonoBehaviour
{
    public void TurnOffCheckpointInfoPanel()
    {
        Time.timeScale = 1.0f;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}

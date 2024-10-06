using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySceneUI : MonoBehaviour
{
    public GameObject[] ninjas;

    private void Awake() {
        Application.targetFrameRate = 60;
        //PlayerPrefs.SetInt(StringManager.ninja3, 1);
        if(PlayerPrefs.GetInt(StringManager.ninja1) == 1)
            ninjas[0].SetActive(true);
        if(PlayerPrefs.GetInt(StringManager.ninja2) == 1)
            ninjas[1].SetActive(true);
        if(PlayerPrefs.GetInt(StringManager.ninja3) == 1)
            ninjas[2].SetActive(true);
        if(PlayerPrefs.GetInt(StringManager.ninja4) == 1)
            ninjas[3].SetActive(true);
        if(PlayerPrefs.GetInt(StringManager.ninja5) == 1)
            ninjas[4].SetActive(true);
    }
}

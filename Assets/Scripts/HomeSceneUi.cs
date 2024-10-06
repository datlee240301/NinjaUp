using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeSceneUi : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {

    }

    /// Button

    public void LoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public void UseNinja(int ninjaId) {
        if (ninjaId == 1) {
            PlayerPrefs.SetInt(StringManager.ninja1, 1);
            PlayerPrefs.SetInt(StringManager.ninja2, 0);
            PlayerPrefs.SetInt(StringManager.ninja3, 0);
            PlayerPrefs.SetInt(StringManager.ninja4, 0);
            PlayerPrefs.SetInt(StringManager.ninja5, 0);
        } else if (ninjaId == 2) {
            PlayerPrefs.SetInt(StringManager.ninja1, 0);
            PlayerPrefs.SetInt(StringManager.ninja2, 1);
            PlayerPrefs.SetInt(StringManager.ninja3, 0);
            PlayerPrefs.SetInt(StringManager.ninja4, 0);
            PlayerPrefs.SetInt(StringManager.ninja5, 0);
        } else if (ninjaId == 3) {
            PlayerPrefs.SetInt(StringManager.ninja1, 0);
            PlayerPrefs.SetInt(StringManager.ninja2, 0);
            PlayerPrefs.SetInt(StringManager.ninja3, 1);
            PlayerPrefs.SetInt(StringManager.ninja4, 0);
            PlayerPrefs.SetInt(StringManager.ninja5, 0);
        } else if (ninjaId == 4) {
            PlayerPrefs.SetInt(StringManager.ninja1, 0);
            PlayerPrefs.SetInt(StringManager.ninja2, 0);
            PlayerPrefs.SetInt(StringManager.ninja3, 0);
            PlayerPrefs.SetInt(StringManager.ninja4, 1);
            PlayerPrefs.SetInt(StringManager.ninja5, 0);
        } else if (ninjaId == 5) {
            PlayerPrefs.SetInt(StringManager.ninja1, 0);
            PlayerPrefs.SetInt(StringManager.ninja2, 0);
            PlayerPrefs.SetInt(StringManager.ninja3, 0);
            PlayerPrefs.SetInt(StringManager.ninja4, 0);
            PlayerPrefs.SetInt(StringManager.ninja5, 1);
        }
    }
}

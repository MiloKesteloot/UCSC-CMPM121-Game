using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public List<Screen> screens = new();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Screen s in screens) {
            if (GameManager.Instance.state == s.gameState) {
                s.ui.SetActive(true);
            } else {
                s.ui.SetActive(false);
            }
        }
    }

    [System.Serializable]
    public class Screen {
        public GameManager.GameState gameState;
        public GameObject ui;
    }
}


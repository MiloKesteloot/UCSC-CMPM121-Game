using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public List<Screen> screens = new();

    void Update() {
        foreach (Screen s in screens) {
            
            bool active = GameManager.Instance.state == s.gameState;
            
            if (active == s.ui.activeInHierarchy) continue; // If the screen is already in it's current state, continue

            if (active) {
                SetStats(s);
                s.ui.SetActive(true);
            } else {
                s.ui.SetActive(false);
            }
        }
    }

    void SetStats(Screen screen) {
        Transform descendantTransform = screen.ui.transform.Find("Stats");
        if (descendantTransform == null) return;
        if (!descendantTransform.gameObject.TryGetComponent(out TMP_Text textAsset)) return;
        string stats = "";
        switch (GameManager.Instance.state) {
            case GameManager.GameState.WAVEEND:
                stats = StatsManager.Instance.GetFormattedWaveStats();
                break;
            case GameManager.GameState.GAMEOVER:
                stats = StatsManager.Instance.GetFormattedGameStats();
                break;
        }
        textAsset.text = stats;
    }

    [System.Serializable]
    public class Screen {
        public GameManager.GameState gameState;
        public GameObject ui;
    }

    public void ReturnToMenu() {
        GameManager.Instance.state = GameManager.GameState.PREGAME;
    }
}


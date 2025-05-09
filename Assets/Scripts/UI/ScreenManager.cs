using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public List<Screen> screens = new();
    public Spell spell = null;

    void Update() {
        foreach (Screen s in screens) {
            
            bool active = GameManager.Instance.state == s.gameState;
            
            if (active == s.ui.activeInHierarchy) continue; // If the screen is already in it's current state, continue

            if (active) {
                SetSpell(s);
                SetStats(s);
                s.ui.SetActive(true);
            } else {
                s.ui.SetActive(false);
            }
        }
    }

    public void AcceptSpell(GameObject acceptButton) {
        SpellUI[] spelluis = GameManager.Instance.playerController.spelluis;
        foreach (SpellUI spellui in spelluis) {
            if (spellui.spell != null) continue;
            spellui.SetSpell(spell);
            acceptButton.SetActive(false);
            return;
        }
    }

    void SetSpell(Screen screen) {
        Transform spellTextTransform = screen.ui.transform.Find("spellinfo");
        Transform spellIconTransform = screen.ui.transform.Find("spell1");
        Transform acceptTransform = screen.ui.transform.Find("Accept");
        if (spellTextTransform == null || spellIconTransform == null) return;
        if (!spellTextTransform.gameObject.TryGetComponent(out TMP_Text textAsset)) return;
        if (!spellIconTransform.gameObject.TryGetComponent(out SpellUI iconAsset)) return;
        acceptTransform.gameObject.SetActive(true);

        spell = SpellBuilder.BuildRandom(GameManager.Instance.playerController.spellcaster);

        string text = spell.GetBaseSpell().GetName() + "\n - " + spell.GetBaseSpell().GetDescription();
        
        Spell s = spell;

        while (s is ModifierSpell) {
            text += "\n" + s.GetName() + "\n - " + s.GetDescription();
            s = ((ModifierSpell)s).subSpell;
        }

        textAsset.text = text;
        iconAsset.SetSpell(spell);
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


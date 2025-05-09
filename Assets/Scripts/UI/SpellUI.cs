using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpellUI : MonoBehaviour
{
    public GameObject icon;
    public RectTransform cooldown;
    public TextMeshProUGUI manacost;
    public TextMeshProUGUI damage;
    public GameObject highlight;
    public Spell spell;
    float last_text_update;
    const float UPDATE_DELAY = 1;
    public GameObject dropbutton;

    void Start() {
        last_text_update = 0;
        if (dropbutton != null) {
            dropbutton.GetComponent<Button>().onClick.AddListener(Drop);
        }
    }

    public void Drop() {
        this.SetSpell(null);
    }

    public void SetSpell(Spell spell) {
        if (GameManager.Instance.playerController.spellcaster.spell == this.spell) {
            GameManager.Instance.playerController.spellcaster.spell = spell;
        }
        this.spell = spell;
        if (spell != null) {
            GameManager.Instance.spellIconManager.PlaceSprite(spell.GetIcon(), icon.GetComponent<Image>());
        } else {
            icon.GetComponent<Image>().sprite = null;
        }

    }

    void Update() {
        if (spell == null) return;
        if (Time.time > last_text_update + UPDATE_DELAY)
        {
            manacost.text = spell.GetManaCost().ToString();
            damage.text = spell.GetDamage().ToString();
            last_text_update = Time.time;
        }
        
        float since_last = Time.time - spell.last_cast;
        float perc;
        if (since_last > spell.GetCooldown())
        {
            perc = 0;
        }
        else
        {
            perc = 1-since_last / spell.GetCooldown();
        }
        cooldown.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 48 * perc);
    }
}

using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class WinTextManager : MonoBehaviour
{
    TMP_Text text;
    void Start() {
        text = GetComponent<TMP_Text>();
    }

    void Update() {
        // This code is actually diabolicaly terrible. It's 4:07am on the day of the deadline and I'm SO TIRED.
        if (GameManager.Instance.player == null) return;
        if (GameManager.Instance.player.GetComponent<PlayerController>() == null) return;
        if (GameManager.Instance.player.GetComponent<PlayerController>().hp == null) return;
        if (GameManager.Instance.player.GetComponent<PlayerController>().hp.hp > 0) {
            text.text = "You win!";
        } else {
            text.text = "You lose!";
        }
    }
}

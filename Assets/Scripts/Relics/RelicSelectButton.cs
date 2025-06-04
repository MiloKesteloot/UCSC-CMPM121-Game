using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RelicSelectButton : MonoBehaviour
{

    public Image icon;
    public GameObject highlight;
    public Button selectButton;
    public TMP_Text descriptionText;
    public RelicManager.RelicType relic;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        selectButton?.onClick.AddListener(() =>
        {
            RelicUIManager.Instance.Add(this.relic);
            RewardsRelicManager.Instance?.Close();
        });
    }
}

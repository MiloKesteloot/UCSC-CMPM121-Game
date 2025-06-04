using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RewardsRelicManager : MonoBehaviour
{
    public static RewardsRelicManager Instance = null;

    public List<RelicSelectButton> relicSelectButtons = new();
    public int mostRecentWave = -1;

    public GameObject relicsHolder;

    // Update is called once per frame
    void Update()
    {
        if (Instance == null)
        {
            RewardsRelicManager.Instance = this;
        }
        {
            if (mostRecentWave != GameManager.Instance.wave)
            {
                mostRecentWave = GameManager.Instance.wave;
                if (true || GameManager.Instance.wave % 3 == 2)
                {
                    UpdateRelics();
                }
            }
        }
    }

    public void Close()
    {

        foreach (RelicSelectButton rsb in relicSelectButtons)
        {
            rsb.gameObject.SetActive(false);
        }
    }

    private void UpdateRelics()
    {
        Debug.Log("updated relics!");
        List<RelicManager.RelicType> relicTypes = RelicManager.Instance.GetXRandomRelics(3);
        int i = 0;
        foreach (RelicSelectButton rsb in relicSelectButtons)
        {
            rsb.gameObject.SetActive(true);
            rsb.icon.sprite = GameManager.Instance.relicIconManager.Get(relicTypes[i].sprite);
            rsb.relic = relicTypes[i];
            rsb.descriptionText.text = relicTypes[i].trigger.description + " " + relicTypes[i].effect.description;
            i++;
        }
    }
}

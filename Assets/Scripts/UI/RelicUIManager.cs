using UnityEngine;

public class RelicUIManager : MonoBehaviour
{
    public static RelicUIManager Instance = null;

    public GameObject relicUIPrefab;
    public PlayerController player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //EventBus.Instance.OnRelicPickup += OnRelicPickup;
    }

    // Update is called once per frame
    void Update()
    {
        if (RelicUIManager.Instance == null) RelicUIManager.Instance = this;
    }

    public void Add(RelicManager.RelicType relicType)
    {
        new Relic(GameManager.Instance.playerController, relicType);
        GameObject n = Instantiate(relicUIPrefab);
        RelicSelectButton r = n.GetComponent<RelicSelectButton>();
        r.icon.sprite = GameManager.Instance.relicIconManager.Get(relicType.sprite);
        r.transform.SetParent(gameObject.transform);
    }

    /*public void OnRelicPickup(Relic r)
    {
        // make a new Relic UI representation
        GameObject rui = Instantiate(relicUIPrefab, transform);
        rui.transform.localPosition = new Vector3(-450 + 40 * (player.relics.Count - 1), 0, 0);
        RelicUI ruic = rui.GetComponent<RelicUI>();
        ruic.player = player;
        ruic.index = player.relics.Count - 1;
        
    }*/
}

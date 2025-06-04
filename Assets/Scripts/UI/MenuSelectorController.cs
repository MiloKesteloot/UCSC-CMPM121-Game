using UnityEngine;
using TMPro;

public class MenuSelectorController : MonoBehaviour
{
    public TextMeshProUGUI label;
    public string level;
    public EnemySpawner spawner = null;
    public ClassManager.ClassType classType = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetLevel(string text)
    {
        level = text;
        label.text = text;
    }

    public void SetClass(ClassManager.ClassType classType)
    {
        this.classType = classType;
        label.text = classType.name;
    }

    public void StartLevel()
    {
        if (spawner != null)
        {
            spawner.StartLevel(level);
        }
        else
        {
            GameManager.Instance.playerController.classType = classType;
            GameManager.Instance.state = GameManager.GameState.PREGAME2;
            GameManager.Instance.playerController.transform.Find("SpriteGameObject").gameObject.GetComponent<SpriteRenderer>().sprite = GameManager.Instance.playerSpriteManager.Get(classType.sprite);
        }
    }
}

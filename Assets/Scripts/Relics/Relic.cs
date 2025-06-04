using UnityEngine;
using System.Collections;
using RelicType = RelicManager.RelicType;
using System;

public class Relic
{
    public PlayerController player;
    public RelicType relicType;

    public Relic(PlayerController player, RelicType relicType)
    {
        this.player = player;
        this.relicType = relicType;

        RelicManager.TriggerInfo triggerInfo = relicType.trigger;
        RelicManager.EffectInfo effectInfo = relicType.effect;

        PlayerController pc = GameManager.Instance.playerController;

        Action effect = effectInfo.type switch
        {
            "gain-mana" => () =>
                {
                    Debug.Log("Gained mana!");
                    pc.spellcaster.AddMana(
                        (int)RPN.Eval(effectInfo.amount, null)
                    );
                }

            ,
            "gain-health" => () =>
                {
                    Debug.Log("Gained health!");
                    pc.hp.GiveHealth(
                        (int)RPN.Eval(effectInfo.amount, null)
                    );
                }

            ,
            "gain-spellpower" => () =>
                {
                    Debug.Log("Gained spellpower!");
                    pc.spellPower += RPN.Eval(effectInfo.amount, null);
                }
            ,
            _ => () => { }
        };
        effect();
        Debug.Log("hi!");
        switch (triggerInfo.type)
        {
            case "take-damage":
                Debug.Log("added event to OnDamageEmpty!");
                EventBus.Instance.OnDamageEmpty += effect;
                break;
            case "stand-still":
                float time = RPN.Eval(triggerInfo.amount, null);
                EventBus.Instance.fixedCustomEvents.Add(
                    (
                        () =>
                        {
                            return (pc.unit.timeSinceLastMoved % time) < (pc.unit.lastTimeSinceLastMoved % time);
                        },
                        effect
                        )
                );
                break;
            case "on-kill":
                EventBus.Instance.OnKill += effect;
                break;
            case "on-miss":
                EventBus.Instance.OnMiss += effect;
                break;
            case "start-of-wave":
                EventBus.Instance.OnWave += effect;
                break;
        }
    }
}

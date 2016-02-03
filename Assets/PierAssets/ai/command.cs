using UnityEngine;
using System.Collections;
// use this to handle all input exept movement
public abstract class command : MonoBehaviour, IActivatableAbility {
    protected bool isDone;
    public bool getIsDone()
    {
        return isDone;
    }
    public abstract void onActivate(IGameUnit caster);

    public abstract void onChanel(IGameUnit caster);

    public abstract void onRelease(IGameUnit caster);
}

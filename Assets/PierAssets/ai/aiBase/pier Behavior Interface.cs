using UnityEngine;
using System.Collections;


public interface IActivatableAbility
{   //activate
    void onActivate(IGameUnit caster);
    //chanel
    void onChanel(IGameUnit caster);
    //release
    void onRelease(IGameUnit caster);

}

public interface IGameUnit
{

     void AttackTrue();
     void AttackFalse();
     bool GetIsAttacking();
     IEnumerator Attack(float range, float delay);
     IEnumerator AttackSphere(float range, float delay);
     float getCurrentHealth();
     bool changeHealth(float damage);
 
     bool CheckIfAlive();
     float GetDamage();
    GameObject GetGameObject();
    Transform GetTransform();
}
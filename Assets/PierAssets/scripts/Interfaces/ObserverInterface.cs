using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IObsevable<T>
{
    void Register(IObserver<T> observer);
    void Unregister(IObserver<T> observer);
}

public interface IObserver<T>
{

    void onUpdate(T value);

}

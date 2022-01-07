using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class SignalListner : MonoBehaviour
{
    public signal signal;
    public UnityEvent signalEvent;
    public void OnSignalRaised()
    {
        signalEvent.Invoke();
    }
    private void OnEnable()
    {
        signal.RegisterListner(this);
    }
    private void OnDisable()
    {
        signal.DeRegisterListner(this);
    }
}

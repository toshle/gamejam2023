using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Interactable : MonoBehaviour
{
    [SerializeField] protected GameObject _interactableObject;
    [SerializeField] protected Collider _triggerCollider;

    bool _pressed = false;
    bool _canBePressed = false;

    void Update()
    {
        if(!_pressed && Input.GetKey(KeyCode.E))
        {
            Press();

            Invoke(nameof(ResetPress), 0.25f);
        }
    }

    protected virtual void Activate()
    {

    }

    protected virtual void Deactivate()
    {

    }


    private void Press()
    {
        if(_canBePressed) {
            _pressed = true;
            Activate();
        }
    }

    private void ResetPress()
    {
        _pressed = false;
        Deactivate();
    }

    private void OnTriggerEnter(Collider other)
    {
        _canBePressed = true;
        Debug.Log("Button is pressable");
    }

    private void OnTriggerExit(Collider other)
    {
        _canBePressed = false;
        Debug.Log("Button is NOT pressable");
    }
}

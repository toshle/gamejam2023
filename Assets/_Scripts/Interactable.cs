using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Interactable : MonoBehaviour
{
    [SerializeField] protected GameObject _interactableObject;
    [SerializeField] protected Collider _triggerCollider;
    [SerializeField] protected AudioSource _buttonAudioSource;
    [SerializeField] protected AudioClip _buttonAudioClip;

    protected bool _pressed = false;
    protected bool _canBePressed = false;

    protected virtual void Update()
    {
        if(_canBePressed && !_pressed && Input.GetKey(KeyCode.E))
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
        _pressed = true;
        Activate();
    }

    private void ResetPress()
    {
        _pressed = false;
        Deactivate();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if(GameObject.FindGameObjectWithTag("Player").transform.position == other.transform.position) { 
            _canBePressed = true;
            Debug.Log("Button is pressable");
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (GameObject.FindGameObjectWithTag("Player").transform.position == other.transform.position)
        {
            _canBePressed = false;
            Debug.Log("Button is NOT pressable");
        }
    }
}

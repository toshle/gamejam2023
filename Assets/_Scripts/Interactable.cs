using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Interactable : MonoBehaviour
{
    [SerializeField] private GameObject _interactableObject;
    [SerializeField] private Collider _triggerCollider;

    bool _pressed = false;

    void Update()
    {
        if(!_pressed && Input.GetKey(KeyCode.E))
        {
            Press();

            Invoke(nameof(ResetPress), 0.25f);
        }
    }

    private void Press()
    {
        _pressed = true;
        var position = _interactableObject.transform.transform.position;
        _interactableObject.transform.transform.position = new Vector3(position.x, position.y - 0.01f, position.z);

        GameManager.Instance.UpdateGameState(GameManager.GameState.Win);
    }

    private void ResetPress()
    {
        _pressed = false;
        var position = _interactableObject.transform.transform.position;
        _interactableObject.transform.transform.position = new Vector3(position.x, position.y + 0.01f, position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Button is pressable");
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Button is NOT pressable");
    }
}

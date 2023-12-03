using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedButton : Interactable
{
    protected override void Activate()
    {
        var position = _interactableObject.transform.transform.position;
        _interactableObject.transform.transform.position = new Vector3(position.x, position.y - 0.01f, position.z);
        GameManager.Instance.UpdateGameState(GameManager.GameState.Win);
        _buttonAudioSource.PlayOneShot(_buttonAudioClip);
    }

    protected override void Deactivate()
    {
        var position = _interactableObject.transform.transform.position;
        _interactableObject.transform.transform.position = new Vector3(position.x, position.y + 0.01f, position.z);
    }
}

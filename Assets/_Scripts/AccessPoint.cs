using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AccessPoint : MonoBehaviour
{
    [SerializeField] private float _hackTime;
    [SerializeField] private Image _loadingImage;
    [SerializeField] private TMP_Text _loadingText;

    [SerializeField] bool _completed = false;
    [SerializeField] bool _canBePressed = false;

    [SerializeField] private bool _active;
    [SerializeField] private bool _activated = false;
    [SerializeField] private AccessPoint _nextPoint;
    [SerializeField] private TMP_Text _orderText;
    [SerializeField] private GameObject _objectToRemove;
    private int _index = 0;
    // Start is called before the first frame update
    void Start()
    {
        _loadingImage.fillAmount = 0;
    }

    public void Activate(AccessPoint nextPoint = null)
    {
        _nextPoint = nextPoint;
        _activated = true;


    }

    public void ShowActivation(int? order = null)
    {
        if(_activated && order != null)
        {
            _index = order.Value;
            _orderText.text = _index.ToString();
        }
    }

    protected void Update()
    {
        if (_activated && !_completed) { 

            if (_canBePressed && Input.GetKeyDown(KeyCode.E))
            {
                _active = true;
            }

            if (_canBePressed && Input.GetKeyUp(KeyCode.E))
            {
                _active = false;
                _loadingImage.fillAmount = 0f;
                _loadingText.text = "";
            }

            if (_active)
            {
                _loadingImage.fillAmount += Time.deltaTime / _hackTime;
                _loadingText.text = ((int)(_loadingImage.fillAmount * 100)).ToString() + "%";
                if (_loadingImage.fillAmount >= 1)
                {
                    _active = false;
                    _completed = true;
                    if(_nextPoint != null)
                        _nextPoint.ShowActivation(_index + 1);
                }
            }
        }
    }
    protected void OnTriggerEnter(Collider other)
    {
        _canBePressed = true;
    }

    protected void OnTriggerExit(Collider other)
    {
        _canBePressed = false;
    }
}

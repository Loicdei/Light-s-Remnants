using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

[RequireComponent(typeof(PlayerInput))]

public class MenuInput : MonoBehaviour
{
    public enum Side {  Left, Right }
    private PlayerInput inputs;
    private UnityEvent backEvent;
    private UnityEvent RightShoulderEvent, LeftShoulderEvent;
    [SerializeField] private TMP_Dropdown resolutionDropDown;

    public void Awake()
    {
        inputs = GetComponent<PlayerInput>();
        backEvent = new UnityEvent();
        RightShoulderEvent = new UnityEvent();
        LeftShoulderEvent = new UnityEvent();
        if (resolutionDropDown == null)
        {
            Debug.LogWarning("Resolution dropdown is not assigned in the inspector.");
        }
    }

    private void OnBack()
    {
        resolutionDropDown.Hide();
        backEvent.Invoke();
    }
    private void OnRightShoulder() { RightShoulderEvent.Invoke(); }
    private void OnLeftShoulder() { LeftShoulderEvent.Invoke(); }

    public void SetBackListener(UnityAction _call)
    {
        backEvent.RemoveAllListeners();
        if (_call != null) backEvent.AddListener(_call);
    }
    public void SetBackListener() { backEvent.RemoveAllListeners(); }

    public void SetShoulderListener(Side _side, UnityAction _call, UnityAction _selection)
    {
        UnityEvent _event = _side == Side.Left ? LeftShoulderEvent : RightShoulderEvent;

        _event.RemoveAllListeners();
        _event.AddListener(_selection);
        _event.AddListener(_call);
    }

    public void SetShoulderListener(Side _side)
    {
        UnityEvent _event = _side == Side.Left ? LeftShoulderEvent : RightShoulderEvent;

        _event.RemoveAllListeners();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class PasswordInputField : MonoBehaviour {

    [SerializeField]
    TMP_InputField _passwordInput;

    [SerializeField]
    ToggleButton _showHideToggleButton;

    bool _startToggleIsOn;

    public string text{
        get{
            return _passwordInput.text;
        }set{
            _passwordInput.text = value;
        }
    }

    void Start() {
        if (_showHideToggleButton != null) {
            _startToggleIsOn = _showHideToggleButton.isOn;
        }
    }

    void OnEnable() {
        if(_showHideToggleButton != null){
            _showHideToggleButton.OnToggle += OnShowHideToggle;
        }
       
    }

    void OnDisable() {
        if (_showHideToggleButton != null) {
            _showHideToggleButton.OnToggle -= OnShowHideToggle;
        }
    }

    void OnShowHideToggle(ToggleButton pButton, bool pShowPassword){

        _passwordInput.contentType = pShowPassword ? TMP_InputField.ContentType.Standard : 
            TMP_InputField.ContentType.Password;

        //refocus on the input field and force a refresh
        _passwordInput.ActivateInputField();
        _passwordInput.Select();

        StartCoroutine(MoveCaretAfterFrame());

    }

    public void Reset() {
        if (_showHideToggleButton != null) {
            _showHideToggleButton.SetOn(_startToggleIsOn);
        }

        _passwordInput.text = "";
    }

    IEnumerator MoveCaretAfterFrame(){
        yield return new WaitForEndOfFrame();
        //need to wait a frame before doing this, otherwise it doesn't work
        _passwordInput.MoveTextEnd(false);
    }
}

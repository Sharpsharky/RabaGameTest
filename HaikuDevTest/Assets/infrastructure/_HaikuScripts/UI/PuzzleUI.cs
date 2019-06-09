using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PuzzleUI : MonoBehaviour {

    [SerializeField]
    PuzzleController _puzzleController;

    [SerializeField]
    string _puzzleId;

    //Skip
    [SerializeField]
    GameObject _confirmSkipPanel;

    [SerializeField]
    GameObject _skipButton;

    [SerializeField]
    TextMeshProUGUI _skipCostText;

    [SerializeField]
    TextMeshProUGUI _confirmSkipCostText;

    [SerializeField]
    Transform _skipMoteFlyToLocation;

    [SerializeField]
    public GameObject _skipHitFxPrefab;

    //Reset
    [SerializeField]
    GameObject _resetButton;

    //Back
    [SerializeField]
    GameObject _backButton;

    [SerializeField]
    bool _hideBackButton = true;

    [SerializeField]
    int _backRoomId;

    bool _waitingForSkipAnimation;
    bool _waitingForSkipTransaction;
    bool _isSkipError;
    int _skipCost;
    bool _isPuzzlePreviouslySkipped;
    string _episodeId;

    bool _isActive;

    void Awake() {
        if (_skipButton != null) {
            if (_skipButton.activeSelf) {
                _skipButton.SetActive(false);
            } else {
                _skipButton = null;
            }

        }

        if (_resetButton != null) {
            if (_resetButton.activeSelf) {
                _resetButton.SetActive(false);
            } else {
                _resetButton = null;
            }

        }

        if (_confirmSkipPanel != null) {
            _confirmSkipPanel.SetActive(false);
        }
    }

    public void Deactivate() {
        _isActive = false;
        if (_skipButton != null) {
            _skipButton.SetActive(false);
        }

        if (_backButton != null && _hideBackButton) {
            _backButton.SetActive(false);
        }

        if (_resetButton != null) {
            _resetButton.SetActive(false);
        }
    }

    public void Activate() {
        _isActive = true;
        if (_skipButton != null) {
            _skipButton.SetActive(true);





            _isPuzzlePreviouslySkipped = true;

            if (_isPuzzlePreviouslySkipped) {
                _skipCost = 0;
            }

            if (_skipCostText != null) {
                _skipCostText.text = _skipCost.ToString();
            }

            if(_confirmSkipCostText != null){
                _confirmSkipCostText.text = _skipCost.ToString();
            }
        }

        if (_backButton != null) {
            _backButton.SetActive(true);
        }

        if (_resetButton != null) {
            _resetButton.SetActive(true);
        }
    }

    public bool OnAndroidBack() {
        if (_isActive) {
            OnTapBack();
            return true;
        } else {
            return false;
        }
    }

    void OnTapReset() {
        _puzzleController.ResetPuzzle();
    }

    void OnTapSkip() {
        if (_confirmSkipPanel != null) _confirmSkipPanel.SetActive(false);
        _puzzleController.Skip();
    }

    void OnTapBack() {
        ChapterSceneManager.instance.FocusRoom(_backRoomId);
    }

    void OnCancelSkip() {

        if (_confirmSkipPanel != null) _confirmSkipPanel.SetActive(false);
        _puzzleController.SkipPanelCanceled();
    }

    void OnConfirmSkip() {
        _isSkipError = false;
        _isPuzzlePreviouslySkipped = true;
        if (_isPuzzlePreviouslySkipped) {
            if (_confirmSkipPanel != null) _confirmSkipPanel.SetActive(false);
            _puzzleController.Skip();
        } else {

            // never happens

        }

    }

    void OnAnimationComplete() {
        _waitingForSkipAnimation = false;
        if (!_waitingForSkipTransaction) {
            Finish();
        } else {
            ChapterUIManager.instance.starCountDisplay.ShowLoadingState();
        }
    }



    void Finish() {
        ChapterUIManager.instance.EnableTapBlocker(false);

        _isSkipError = false;
        if (_isSkipError) {
           
        } else {
            _puzzleController.Skip();
        }

        _isSkipError = false;
    }
}

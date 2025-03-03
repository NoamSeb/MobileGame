using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;

public class ButtonColorManager : MonoBehaviour
{
    Image _image;
    Button _button;

    [SerializeField] Color _unfinishedColor, _finishedColor;

    bool _isButtonForCurrentLevel;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _button = GetComponent<Button>();

        GridExit.OnLevelEnd += ChangeColorFinished;
    }

    private void Start()
    {
        ChangeColorUnfinished();
        _button.onClick.AddListener(WaitForLevelFinishEvent);
    }

    void ChangeColorUnfinished()
    {
        _image.color = _unfinishedColor;
    }

    public void ChangeColorFinished()
    {
        if (_isButtonForCurrentLevel) { _image.color = _finishedColor; }
    }

    void WaitForLevelFinishEvent()
    {
        _isButtonForCurrentLevel = true;
    }
}

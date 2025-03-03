using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;

public class ButtonColorManager : MonoBehaviour
{
    Image _image;
    Button _button;

    [field : SerializeField] public Color _unfinishedColor { get; private set; }
    [field : SerializeField] public Color _finishedColor{ get; private set; }

    bool _isButtonForCurrentLevel;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _button = GetComponent<Button>();

        GridExit.OnLevelEnd += ChangeColorFinished;
    }

    private void Start()
    {
        //Debug.LogWarning("ButtonColorManager called");
        //ChangeColorUnfinished();
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

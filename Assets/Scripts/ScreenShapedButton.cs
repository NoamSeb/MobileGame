using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class ScreenShapedButton : MonoBehaviour
{
    Image _image;
    Button _button;

    [SerializeField] private Color _unfinishedColor;
    [SerializeField] private Color _finishedColor;

    public Color UnfinishedColor { get {  return _unfinishedColor; } }
    public Color FinishedColor { get { return _finishedColor; } }

    bool _isButtonForCurrentLevel;

    public bool IsFinished { get { return _image.color == FinishedColor; } }

    private void Awake()
    {
        _image = GetComponent<Image>();
        _button = GetComponent<Button>();

        GridExit.OnLevelEnd += ChangeColorFinished;
        LevelController.OnFirstLevelLoad += Activate;
        LevelController.OnLevelUnload += Activate;
    }

    private void Start()
    {
        //Debug.LogWarning("ButtonColorManager called");
        //ChangeColorUnfinished();
        _button.onClick.AddListener(WaitForLevelFinishEvent);
    }

    public void Deactivate()
    {
        _image.enabled = false;
        _button.interactable = false;
    }

    public void Activate(ScreenShapedButton button)
    {
        if (button == this)
        {
            _image.enabled = true;
            _button.interactable = true;
        }
    }

    public void ChangeColorUnfinished()
    {
        _image.color = UnfinishedColor;
    }

    public void ChangeColorFinished()
    {
        if (_isButtonForCurrentLevel) { _image.color = FinishedColor; }
    }

    void WaitForLevelFinishEvent()
    {
        _isButtonForCurrentLevel = true;
    }
}

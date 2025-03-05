using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;

public class ScreenShapedButton : MonoBehaviour
{
    Image _image;
    Button _button;

    [SerializeField] private Color _unfinishedColor;
    [SerializeField] private Color _finishedColor;

    public Color UnfinishedColor { get {  return _unfinishedColor; } }
    public Color FinishedColor { get { return _finishedColor; } }

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

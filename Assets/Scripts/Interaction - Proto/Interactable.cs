using System;
using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactable : MonoBehaviour 
{
    public static event Action<Interactable> OnInteraction;
    private InputAction _touchPress, _touchPos;

    private void Awake()
    {
        _touchPress = InteractableManager.Instance.TouchPress;
        _touchPos = InteractableManager.Instance.TouchPos;
    }

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE 
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (IsClickOnTheSpot(mousePos))
            {
                Interaction();
            }
        }
#endif

#if UNITY_ANDROID 
        if (_touchPress.WasPressedThisFrame())
        {
            Vector2 touchPos = _touchPos.ReadValue<Vector2>();
            if (IsClickOnTheSpot(touchPos))
            {
                Interaction();
            }
        }
#endif
    }

    protected virtual bool IsClickOnTheSpot(Vector2 pos)
    {
        return Vector2.Distance(pos, transform.position) < InteractableManager.Instance.ItemTouchRange;
    }
    
    protected virtual void Interaction()
    {
        OnInteraction?.Invoke(this);
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, InteractableManager.Instance.ItemTouchRange);
        }
    }
}

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
            if (Vector2.Distance(mousePos, transform.position) < InteractableManager.Instance.ItemTouchRange)
            {
                if (!_isCoroutineRunning)
                {
                    StartCoroutine(StartInteraction());
                }
            }
        }
#endif

#if UNITY_ANDROID 
        if (_touchPress.WasPressedThisFrame())
        {
            Vector2 touchPos = _touchPos.ReadValue<Vector2>();
            if (Vector2.Distance(touchPos, transform.position) < InteractableManager.Instance.ItemTouchRange)
            {
                if (!_isCoroutineRunning)
                {
                    StartCoroutine(StartInteraction());
                }
            }
        }
#endif
    }

    float RangeToPlayer => Vector2.Distance(InteractableManager.Instance.Player.transform.position, transform.position);
    bool IsPlayerInRange => RangeToPlayer < InteractableManager.Instance.ItemInteractionRange;
    bool _isCoroutineRunning;
    IEnumerator StartInteraction()
    {
        _isCoroutineRunning = true;
        while (!IsPlayerInRange)
        {
            yield return new WaitForEndOfFrame();
        }
        Interaction();
        _isCoroutineRunning = false;
    }

    protected virtual void Interaction()
    {
        OnInteraction?.Invoke(this);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        _isCoroutineRunning = false;
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, InteractableManager.Instance.ItemTouchRange);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, InteractableManager.Instance.ItemInteractionRange);
        }
    }
}

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class TeleportController : MonoBehaviour
{
    public GameObject teleportationGameObject;

    public InputActionReference teleportActivationReference;

    public UnityEvent onTeleportActivate;
    public UnityEvent onTeleportCancel;

    private void OnEnable()
    {
        teleportActivationReference.action.performed += TeleportModeActivate;
        teleportActivationReference.action.canceled += TeleportModeCancel;
    }

    private void OnDisable()
    {
        teleportActivationReference.action.performed -= TeleportModeActivate;
        teleportActivationReference.action.canceled -= TeleportModeCancel;
    }

    private void TeleportModeCancel(InputAction.CallbackContext obj) => Invoke("DeactivateTeleporter", .1f);

    void DeactivateTeleporter() => onTeleportCancel.Invoke();

    private void TeleportModeActivate(InputAction.CallbackContext obj) => onTeleportActivate.Invoke();


}

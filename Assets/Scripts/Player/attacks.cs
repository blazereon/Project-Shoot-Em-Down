using UnityEngine;
using UnityEngine.InputSystem;
//4/14/25-nathan
//if u find bugs or any undesirable trait feel free to debug or message me
public class attacks : MonoBehaviour
{
    public InputActionReference melee;
    private void OnEnable()
    {
        melee.action.started += Melee;
    }

    private void OnDisable()
    {
        melee.action.started -= Melee;
    }

    private void Melee(InputAction.CallbackContext context)
    {
        Debug.Log("melee attack");
    }
}

using UnityEngine;
using UnityEngine.UI;

public class ToggleConfig : MonoBehaviour
{
    [SerializeField] private Toggle _toggle;

    public void ToggleIsOn()
    {
        _toggle.isOn = !_toggle.isOn;
    }
}

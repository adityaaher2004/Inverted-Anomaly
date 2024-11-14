using TMPro;
using UnityEngine;
[DisallowMultipleComponent]
[RequireComponent(typeof(TextMeshProUGUI))]
public class AmmoDisplayer : MonoBehaviour
{
    [SerializeField]
    private PlayerGunSelector gunSelector;
    private TextMeshProUGUI AmmoText;
    private void Awake()
    {
        AmmoText = GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        AmmoText.SetText(
           $"{gunSelector.ActiveGun.AmmoConfig.currentClipAmmo} / "
           + $"{gunSelector.ActiveGun.AmmoConfig.currentAmmo}"
       );
    }

}
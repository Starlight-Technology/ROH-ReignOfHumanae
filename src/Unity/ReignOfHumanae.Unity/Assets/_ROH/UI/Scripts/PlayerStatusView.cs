using UnityEngine;
using UnityEngine.UIElements;

public class PlayerStatusView : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;

    private VisualElement _root;
    private VisualElement _hpFill;
    private VisualElement _manaFill;
    private VisualElement _staminaFill;

    private void Awake()
    {
        _root = uiDocument.rootVisualElement;

        _hpFill = _root.Q<VisualElement>("hp-fill");
        _manaFill = _root.Q<VisualElement>("mana-fill");
        _staminaFill = _root.Q<VisualElement>("stamina-fill");
    }

    public void SetHealth(float current, float max)
    {
        _hpFill.style.width = Length.Percent(current / max * 100f);
    }

    public void SetMana(float current, float max)
    {
        _manaFill.style.width = Length.Percent(current / max * 100f);
    }

    public void SetStamina(float current, float max)
    {
        _staminaFill.style.width = Length.Percent(current / max * 100f);
    }
}

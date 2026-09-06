using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Unity.Mathematics;
using Unity.Transforms;

using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Player
{
    public class PlayerStatusHUDController : MonoBehaviour
    {
        [SerializeField] private UIDocument uiDocument;

        private VisualElement _hpFill;
        private Label _hpPercent;
        private VisualElement _manaFill;
        private Label _manaPercent;
        private VisualElement _staminaFill;
        private Label _staminaPercent;

        public float _hp = 100f;
        public float _mana = 100f;
        public float _stamina = 100f;

        private const float Max = 100f;

        private void Awake()
        {
            var root = uiDocument.rootVisualElement;

            _hpFill = root.Q<VisualElement>("hp-fill");
            _hpPercent = root.Q<Label>("hp-percent");
            _manaFill = root.Q<VisualElement>("mana-fill");
            _manaPercent = root.Q<Label>("mana-percent");
            _staminaFill = root.Q<VisualElement>("stamina-fill");
            _staminaPercent = root.Q<Label>("stamina-percent");

            RefreshAll();
        }

        private void Update()
        {
            ClampValues();
            RefreshAll();
        }


        private void ClampValues()
        {
            _hp = Mathf.Clamp(_hp, 0, Max);
            _mana = Mathf.Clamp(_mana, 0, Max);
            _stamina = Mathf.Clamp(_stamina, 0, Max);
        }

        private void RefreshAll()
        {
            SetBarAndPercentage(_hpFill, _hpPercent, _hp);
            SetBarAndPercentage(_manaFill, _manaPercent, _mana);
            SetBarAndPercentage(_staminaFill, _staminaPercent, _stamina);
        }

        private void SetBarAndPercentage(VisualElement bar, Label barLabel, float value)
        {
            SetBar(bar, value);
            SetBarLabel(barLabel, value);

        }

        private void SetBar(VisualElement bar, float value)
        {
            bar.style.width = Length.Percent(value / Max * 100f);
        }

        private void SetBarLabel(Label percentageText, float value)
        {
            percentageText.text = $"{Math.Round(value, 2, MidpointRounding.AwayFromZero)} %";
        }
    }
}

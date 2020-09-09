using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Thrusters
{
    public class ThrustersUI : MonoBehaviour
    {
        [SerializeField] Slider _ammoSlider;
        [SerializeField] Gradient _ammoBarGradient;

        public void SetMaxAmmo(int ammo)
        {
            _ammoSlider.maxValue = ammo;
            _ammoSlider.value = ammo;
        }
        public void SetAmmo(int ammo)
        {
            _ammoSlider.value = ammo;
        }
    }
}

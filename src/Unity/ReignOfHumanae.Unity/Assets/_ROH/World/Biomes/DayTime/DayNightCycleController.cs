using System;

namespace Assets.Scripts.DayTime
{
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.HighDefinition;

    public class DayNightCycleController : MonoBehaviour
    {
        [Header("Volume Settings")]
        public Volume globalVolume;

        [Header("Sun Light")]
        public Light sunLight;
        public float maxSunIntensity = 130000f;
        public float minSunIntensity = 0f;

        [Header("Cycle Settings")]
        [Tooltip("Duration of one full day-night cycle in minutes.")]
        public float cycleDurationMinutes = 10f;

        private HDRISky hdriSky;
        private float cycleTimer = 0f;

        void Start()
        {
            if (globalVolume == null || sunLight == null)
            {
                Debug.LogError("Volume or SunLight not assigned.");
                enabled = false;
                return;
            }

            if (!globalVolume.profile.TryGet(out hdriSky))
            {
                Debug.LogError("HDRI Sky not found in volume profile.");
                enabled = false;
                return;
            }
        }

        void Update()
        {
            cycleTimer += Time.deltaTime;
            float progress = (cycleTimer / (cycleDurationMinutes * 60f)) % 1f;

            // Simulate sun angle from 0° (sunrise) to 360°
            float sunAngle = Mathf.Lerp(0f, 360f, progress);
            hdriSky.rotation.value = sunAngle;

            // Rotate directional light (simulating sun movement)
            sunLight.transform.rotation = Quaternion.Euler(sunAngle - 90f, 170f, 0f);

            // Simulate light intensity (sunrise to midday to night)
            float dot = Mathf.Clamp01(Vector3.Dot(sunLight.transform.forward, Vector3.down));
            float targetIntensity = Mathf.Lerp(minSunIntensity, maxSunIntensity, dot);
            sunLight.intensity = Mathf.Max(targetIntensity, 100f); // nunca deixa abaixo de 100 lux

            // Optional: Update exposure (if set to manual)
            if (globalVolume.profile.TryGet(out Exposure exposure))
            {
                exposure.fixedExposure.value = Mathf.Lerp(-2f, 14f, dot);
            }
        }
    }
}

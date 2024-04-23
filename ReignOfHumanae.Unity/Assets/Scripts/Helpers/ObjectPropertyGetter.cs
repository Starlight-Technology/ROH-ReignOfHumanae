using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Helpers
{
    public class ObjectPropertyGetter<T> where T : Component
    {
        public string ObjectName { get; set; }

        public void SetValue(string value)
        {
            GameObject obj = GameObject.Find(ObjectName);
            if (obj != null)
            {
                T component = obj.GetComponent<T>();

                if (component is Text textComponent)
                {
                    textComponent.text = value;
                }
            }
            else
            {
                Debug.LogError("Object with name " + ObjectName + " not found.");
            }
        }

        public void SetFontSize(int size)
        {
            GameObject obj = GameObject.Find(ObjectName);
            if (obj != null)
            {
                T component = obj.GetComponent<T>();

                if (component is Text textComponent)
                {
                    textComponent.fontSize = size;
                }
            }
            else
            {
                Debug.LogError("Object with name " + ObjectName + " not found.");
            }
        }

        public void SetFontColor(Color color)
        {
            GameObject obj = GameObject.Find(ObjectName);
            if (obj != null)
            {
                T component = obj.GetComponent<T>();

                if (component is Text textComponent)
                {
                    textComponent.color = color;
                }
            }
            else
            {
                Debug.LogError("Object with name " + ObjectName + " not found.");
            }
        }
    }
}


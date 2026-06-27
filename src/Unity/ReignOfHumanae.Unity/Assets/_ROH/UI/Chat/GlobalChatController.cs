using Assets.Scripts.Models.Websocket;

using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Chat
{
    public class GlobalChatController : MonoBehaviour
    {
        const int MaximumVisibleMessages = 200;

        readonly List<string> _lines = new();
        Action<string> _send;
        InputField _input;
        ScrollRect _scrollRect;
        Text _messages;

        public void AppendMessage(ChatMessageModel message)
        {
            string timestamp = message.CreatedAtUtc.ToLocalTime().ToString("HH:mm");
            AddLine($"[{timestamp}] {message.SenderDisplayName}: {message.Message}");
        }

        public void Bind(Action<string> send)
        {
            _send = send;
            EnsureUi();
        }

        public void LoadHistory(ChatMessageModel[] messages)
        {
            _lines.Clear();
            _messages.text = string.Empty;

            if (messages == null)
                return;

            foreach (ChatMessageModel message in messages)
                AppendMessage(message);
        }

        public void ShowSystemMessage(string message) => AddLine($"[Sistema] {message}");

        void Awake() => EnsureUi();

        void Update()
        {
            if (_input != null && _input.isFocused && Input.GetKeyDown(KeyCode.Return))
                Submit();
        }

        void AddLine(string line)
        {
            EnsureUi();
            _lines.Add(line);
            if (_lines.Count > MaximumVisibleMessages)
                _lines.RemoveAt(0);

            _messages.text = string.Join("\n", _lines);
            Canvas.ForceUpdateCanvases();
            _scrollRect.verticalNormalizedPosition = 0;
        }

        void EnsureUi()
        {
            if (_input != null)
                return;

            Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            GameObject canvasObject = new("GlobalChatCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasObject.transform.SetParent(transform, false);

            Canvas canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 100;

            CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);

            GameObject panel = CreateUiObject("Panel", canvasObject.transform, typeof(Image));
            RectTransform panelRect = panel.GetComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0, 0);
            panelRect.anchorMax = new Vector2(0, 0);
            panelRect.pivot = new Vector2(0, 0);
            panelRect.anchoredPosition = new Vector2(20, 20);
            panelRect.sizeDelta = new Vector2(560, 300);
            panel.GetComponent<Image>().color = new Color(0, 0, 0, 0.72f);

            GameObject viewport = CreateUiObject("Viewport", panel.transform, typeof(Image), typeof(Mask), typeof(ScrollRect));
            RectTransform viewportRect = viewport.GetComponent<RectTransform>();
            viewportRect.anchorMin = new Vector2(0, 0);
            viewportRect.anchorMax = new Vector2(1, 1);
            viewportRect.offsetMin = new Vector2(12, 58);
            viewportRect.offsetMax = new Vector2(-12, -12);
            viewport.GetComponent<Image>().color = new Color(0, 0, 0, 0.18f);
            viewport.GetComponent<Mask>().showMaskGraphic = false;

            GameObject content = CreateUiObject("Content", viewport.transform, typeof(Text), typeof(ContentSizeFitter));
            RectTransform contentRect = content.GetComponent<RectTransform>();
            contentRect.anchorMin = new Vector2(0, 1);
            contentRect.anchorMax = new Vector2(1, 1);
            contentRect.pivot = new Vector2(0.5f, 1);
            contentRect.anchoredPosition = Vector2.zero;
            contentRect.sizeDelta = Vector2.zero;

            _messages = content.GetComponent<Text>();
            _messages.font = font;
            _messages.fontSize = 18;
            _messages.alignment = TextAnchor.UpperLeft;
            _messages.color = Color.white;
            _messages.supportRichText = false;
            _messages.horizontalOverflow = HorizontalWrapMode.Wrap;
            _messages.verticalOverflow = VerticalWrapMode.Overflow;

            ContentSizeFitter fitter = content.GetComponent<ContentSizeFitter>();
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            _scrollRect = viewport.GetComponent<ScrollRect>();
            _scrollRect.content = contentRect;
            _scrollRect.horizontal = false;
            _scrollRect.vertical = true;
            _scrollRect.viewport = viewportRect;

            GameObject inputObject = CreateUiObject("Input", panel.transform, typeof(Image), typeof(InputField));
            RectTransform inputRect = inputObject.GetComponent<RectTransform>();
            inputRect.anchorMin = new Vector2(0, 0);
            inputRect.anchorMax = new Vector2(1, 0);
            inputRect.offsetMin = new Vector2(12, 12);
            inputRect.offsetMax = new Vector2(-102, 48);
            inputObject.GetComponent<Image>().color = new Color(1, 1, 1, 0.95f);

            Text inputText = CreateText("Text", inputObject.transform, font, string.Empty, Color.black);
            SetStretch(inputText.rectTransform, 8, 4, -8, -4);

            Text placeholder = CreateText("Placeholder", inputObject.transform, font, "Mensagem global...", Color.gray);
            SetStretch(placeholder.rectTransform, 8, 4, -8, -4);

            _input = inputObject.GetComponent<InputField>();
            _input.textComponent = inputText;
            _input.placeholder = placeholder;
            _input.characterLimit = 300;
            _input.lineType = InputField.LineType.SingleLine;
            inputText.supportRichText = false;

            GameObject buttonObject = CreateUiObject("Send", panel.transform, typeof(Image), typeof(Button));
            RectTransform buttonRect = buttonObject.GetComponent<RectTransform>();
            buttonRect.anchorMin = new Vector2(1, 0);
            buttonRect.anchorMax = new Vector2(1, 0);
            buttonRect.pivot = new Vector2(1, 0);
            buttonRect.anchoredPosition = new Vector2(-12, 12);
            buttonRect.sizeDelta = new Vector2(82, 36);
            buttonObject.GetComponent<Image>().color = new Color(0.18f, 0.42f, 0.68f, 1);

            Text buttonText = CreateText("Text", buttonObject.transform, font, "Enviar", Color.white);
            buttonText.alignment = TextAnchor.MiddleCenter;
            SetStretch(buttonText.rectTransform, 0, 0, 0, 0);
            buttonObject.GetComponent<Button>().onClick.AddListener(Submit);

            if (EventSystem.current == null)
                new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
        }

        void Submit()
        {
            string value = _input.text;
            if (string.IsNullOrWhiteSpace(value))
                return;

            _input.text = string.Empty;
            _input.ActivateInputField();
            _send?.Invoke(value);
        }

        static GameObject CreateUiObject(string name, Transform parent, params Type[] components)
        {
            GameObject value = new(name, typeof(RectTransform));
            value.transform.SetParent(parent, false);
            foreach (Type component in components)
                value.AddComponent(component);
            return value;
        }

        static Text CreateText(string name, Transform parent, Font font, string value, Color color)
        {
            GameObject textObject = CreateUiObject(name, parent, typeof(Text));
            Text text = textObject.GetComponent<Text>();
            text.font = font;
            text.fontSize = 18;
            text.text = value;
            text.color = color;
            return text;
        }

        static void SetStretch(RectTransform rect, float left, float bottom, float right, float top)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = new Vector2(left, bottom);
            rect.offsetMax = new Vector2(right, top);
        }
    }
}

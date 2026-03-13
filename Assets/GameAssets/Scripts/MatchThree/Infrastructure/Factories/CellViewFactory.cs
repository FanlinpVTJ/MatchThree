using MatchThree.Presentation.Views;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MatchThree.Infrastructure.Factories
{
    public class CellViewFactory
    {
        private readonly IInstantiator _instantiator;

        public CellViewFactory(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }

        public CellView CreateCellView(Transform parentTransform)
        {
            CellView cellView = _instantiator.InstantiateComponentOnNewGameObject<CellView>("CellView");
            RectTransform cellRectTransform = EnsureRectTransform(cellView.gameObject);
            Image backgroundImage = EnsureBackgroundImage(cellView.gameObject);
            Button button = EnsureButton(cellView.gameObject, backgroundImage);
            TextMeshProUGUI labelText = CreateLabelText(cellView.transform);

            cellRectTransform.SetParent(parentTransform, false);
            button.targetGraphic = backgroundImage;
            cellView.Initialize(button, backgroundImage, labelText);

            return cellView;
        }

        private RectTransform EnsureRectTransform(GameObject gameObject)
        {
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();

            if (rectTransform == null)
            {
                rectTransform = gameObject.AddComponent<RectTransform>();
            }

            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.localScale = Vector3.one;

            return rectTransform;
        }

        private Image EnsureBackgroundImage(GameObject gameObject)
        {
            Image backgroundImage = gameObject.GetComponent<Image>();

            if (backgroundImage == null)
            {
                backgroundImage = gameObject.AddComponent<Image>();
            }

            return backgroundImage;
        }

        private Button EnsureButton(GameObject gameObject, Image backgroundImage)
        {
            Button button = gameObject.GetComponent<Button>();

            if (button == null)
            {
                button = gameObject.AddComponent<Button>();
            }

            button.targetGraphic = backgroundImage;

            return button;
        }

        private TextMeshProUGUI CreateLabelText(Transform cellTransform)
        {
            GameObject labelGameObject = _instantiator.CreateEmptyGameObject("Label");
            RectTransform labelRectTransform = labelGameObject.AddComponent<RectTransform>();
            labelRectTransform.SetParent(cellTransform, false);
            labelRectTransform.anchorMin = Vector2.zero;
            labelRectTransform.anchorMax = Vector2.one;
            labelRectTransform.offsetMin = Vector2.zero;
            labelRectTransform.offsetMax = Vector2.zero;
            TextMeshProUGUI labelText = labelGameObject.AddComponent<TextMeshProUGUI>();
            labelText.alignment = TextAlignmentOptions.Center;
            labelText.fontSize = 28f;
            labelText.color = Color.white;

            return labelText;
        }
    }
}

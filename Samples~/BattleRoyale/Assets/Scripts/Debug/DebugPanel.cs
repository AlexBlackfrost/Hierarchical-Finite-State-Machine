using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugPanel : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI currentStateText;
    [SerializeField] private Image healthBar;
    [SerializeField] private Gradient healtGradient;

    private Camera mainCamera;
    private RectTransform rectTransform;
    private RectTransform parentRectTransform;

    private void Awake() {
        mainCamera = Camera.main;
        rectTransform = GetComponent<RectTransform>();
        parentRectTransform = rectTransform.parent.GetComponentInParent<RectTransform>();
    }

    public void UpdatePosition(Vector3 characterPosition) {
        Vector2 viewportPosition = mainCamera.WorldToViewportPoint(characterPosition);
        Vector2 screenPosition = new Vector2(
            viewportPosition.x * parentRectTransform.sizeDelta.x,
            viewportPosition.y * parentRectTransform.sizeDelta.y
        );
        rectTransform.position = screenPosition;
    }

    public void UpdateCurrentState(string text) {
        currentStateText.text = text;
    }

    public void UpdateHealth(float currentHealth, float maxHealth) {
        float normalizedAmount = currentHealth / maxHealth;
        healthBar.fillAmount = normalizedAmount;
        healthBar.color = healtGradient.Evaluate(normalizedAmount);
    }
}


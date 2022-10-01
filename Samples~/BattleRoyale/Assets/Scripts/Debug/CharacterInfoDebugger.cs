using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfoDebugger : MonoBehaviour {
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject debugPanelPrefab;

    private GameObject debugPanel;
    private DebugPanel debugInfo;
    private AnimalController animalController;

    private void Awake() {
        debugPanel = Instantiate(debugPanelPrefab, canvas.transform);
        debugInfo = debugPanel.GetComponent<DebugPanel>();
        animalController = GetComponent<AnimalController>();
    }

    public void UpdateCurrentState(string text) {
        debugInfo.UpdateCurrentState(text);
        debugInfo.UpdateHealth(animalController.aliveStateMachineSettings.CurrentHealth,
                            animalController.aliveStateMachineSettings.MaxHealth);
    }

    public void UpdatePosition() {
        debugInfo.UpdatePosition(transform.position + new Vector3(0, 1, 0));
    }

    private void OnDestroy() {
        Destroy(debugPanel);
    }
}


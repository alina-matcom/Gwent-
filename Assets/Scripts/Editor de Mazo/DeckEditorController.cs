using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Asegúrate de incluir el namespace de TextMeshPro
using System.IO;

public class DeckEditorController : MonoBehaviour
{
    public GameObject deckEditorPanel;
    public TMP_InputField deckInputField; // Cambia InputField a TMP_InputField
    public Button openEditorButton;

    private string deckFilePath;

    // Definir el evento
    public delegate void DeckSavedHandler();
    public static event DeckSavedHandler OnDeckSaved;

    void Start()
    {
        openEditorButton.onClick.AddListener(OpenDeckEditor);
        deckInputField.onSubmit.AddListener(OnSubmit); // Cambia onEndEdit a onSubmit
        deckEditorPanel.SetActive(false); // Asegúrate de que el panel esté oculto al inicio

        // Define la ruta del archivo donde se guardará el mazo
        deckFilePath = Path.Combine(Application.streamingAssetsPath, "deck.dsl");
    }

    public void OpenDeckEditor()
    {
        deckEditorPanel.SetActive(true);
    }

    private void OnSubmit(string deckText)
    {
        SaveDeck(deckText);
    }

    private void SaveDeck(string deckText)
    {
        File.WriteAllText(deckFilePath, deckText);
        Debug.Log("Mazo guardado en: " + deckFilePath);
        deckEditorPanel.SetActive(false); // Cierra el panel después de guardar
        // Disparar el evento
        OnDeckSaved?.Invoke();
    }
}
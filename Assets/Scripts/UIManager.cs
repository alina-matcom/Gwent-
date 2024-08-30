using UnityEngine;
using TMPro; // Asegúrate de incluir el espacio de nombres de TextMeshPro
using System.Collections;

public class UIManager : MonoBehaviour
{
     public TextMeshProUGUI winnerText;

    // Método para mostrar el mensaje de ganador
    public IEnumerator ShowWinnerMessage(string message)
    {
        winnerText.text = message; // Actualiza el texto
        winnerText.gameObject.SetActive(true); // Muestra el texto
        yield return new WaitForSeconds(5); // Espera 5 segundos
        winnerText.gameObject.SetActive(false); // Oculta el texto
    }
}
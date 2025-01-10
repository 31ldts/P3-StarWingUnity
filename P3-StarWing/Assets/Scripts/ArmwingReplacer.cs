using UnityEngine;

public class ArmwingReplacer : MonoBehaviour
{
    public GameObject blueMarinePrefab; // Asignado desde el Editor
    public GameObject redSupportPrefab; // Asignado desde el Editor

    void Start()
    {
        string shipName = GameData.ShipName;

        // Buscar el objeto "armwing" dentro del Player
        Transform armwingTransform = transform.Find("armwing");

        if (armwingTransform == null)
        {
            Debug.LogError("No se encontró el objeto 'armwing' dentro de 'Player'.");
            return;
        }

        GameObject newModel = null;

        // Determinar qué modelo usar según el nombre de la nave
        if (shipName == "blue_marine")
        {
            newModel = blueMarinePrefab;
        }
        else if (shipName == "red_support")
        {
            newModel = redSupportPrefab;
        }

        if (newModel == null)
        {
            Debug.Log($"No se reemplazó 'armwing', se mantiene el objeto original o no se encontró el modelo para '{shipName}'.");
            return;
        }

        // Destruir el objeto "armwing" existente
        Destroy(armwingTransform.gameObject);

        // Instanciar el nuevo objeto y ajustar sus valores de transform
        GameObject instantiatedModel = Instantiate(newModel, transform);
        instantiatedModel.transform.localPosition = Vector3.zero; // Posición (0, 0, 0)
        instantiatedModel.transform.localRotation = Quaternion.identity; // Rotación (0, 0, 0)
        instantiatedModel.transform.localScale = Vector3.one; // Escala (1, 1, 1)

        Debug.Log($"Se reemplazó 'armwing' por '{shipName}' con valores de transform reseteados.");
    }
}

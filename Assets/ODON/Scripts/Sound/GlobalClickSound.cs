using UnityEngine;
using UnityEngine.EventSystems;

public class GlobalClickSound : MonoBehaviour
{
    public string clickSoundName = "ClickSound"; // Nom du son dans l'AudioManager

    private void Awake()
    {
        // Gérer les clics UI
        EventSystem.current?.gameObject.AddComponent<GlobalClickSoundHandler>();
    }

    void Update()
    {
        // Détecter les clics hors UI (clic dans la scène 3D)
        if (Input.GetMouseButtonDown(0)) // Clique gauche
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.Play(clickSoundName);
            }
        }
    }
}

public class GlobalClickSoundHandler : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        // Jouer le son de clic UI via l'AudioManager
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.Play("ClickSound");
        }
    }
}

using UnityEngine;

[RequireComponent(typeof(Feedback))]
public class OdonFeedBackManager : MonoBehaviour
{

    private Feedback feedback;

    private void Start()
    {
        feedback = GetComponent<Feedback>();
    }

    /// <summary>
    /// Ajoute le nom d'utilisateur au formulaire de données.
    /// </summary>
    /// <param name="nomPrenom"> nom d'utilisateur </param>
    public void AddFeedBack(string nomPrenom)
    {
        feedback.AssignData(0, nomPrenom);
    }

    /// <summary>
    /// Ajoute une donnée dont la réponse est pré-enregistré au formulaire de données.
    /// </summary>
    /// <param name="feedbackName"> identifiant de la donnée : "PoseEffectuee" | "Patient". </param>
    /// <param name="feedbackAnswerId"> identifiant de la réponse ex: 0 -> pose classique, 1 -> pose parachute (voir fonction pour plus de details...) </param>
    public void AddFeedBack(string feedbackName, int feedbackAnswerId)
    {
        switch (feedbackName)
        {
            case "PoseEffectuee":
                feedback.AssignData(0, feedbackAnswerId == 0 ? "Classique" : "Parachute");
                break;
            case "Patient":
                feedback.AssignData(1, feedbackAnswerId == 0 ? "George, 75 ans, dent 44" : feedbackAnswerId == 1 ? "Thierry, 46 ans, dent 41" : "Claire, 33 ans, dent 47");
                break;
        }
    }

    /// <summary>
    /// Ajoute les équipements équipées du joueur au formulaire de données.
    /// </summary>
    /// <param name="equipements"> liste de 3 booléens correspondant aux trois équipements équipés ou non par le joueur. </param>
    public void AddFeedBack(bool[] equipements)
    {
        feedback.AssignData(2, (equipements[0] ? "Gants ok, " : "Pas de gants, ") + (equipements[1] ? "blouse ok, " : "pas de blouse, ") + (equipements[2] ? "masque ok." : "pas de masque."));
    }

    /// <summary>
    ///  Ajoute une donnée dont la réponse est correcte ou non.
    /// </summary>
    /// <param name="feedbackName"> identifiant de la donnée : "BonDessinDigue" | "BonnePoseCrampon" | "BonPoseDigue" | "CorrectionFilDentaire" | "PoseCadreEnU". </param>
    /// <param name="feedbackAnswer"></param>
    public void AddFeedBack(string feedbackName, bool feedbackAnswer)
    {
        switch (feedbackName)
        {
            case "BonDessinDigue":
                feedback.AssignData(3, feedbackAnswer ? "oui" : "non");
                break;
            case "BonnePoseCrampon":
                feedback.AssignData(4, feedbackAnswer ? "oui" : "non");
                break;
            case "BonPoseDigue":
                feedback.AssignData(5, feedbackAnswer ? "oui" : "non");
                break;
            case "CorrectionFilDentaire":
                feedback.AssignData(6, feedbackAnswer ? "oui" : "non");
                break;
            case "PoseCadreEnU":
                feedback.AssignData(7, feedbackAnswer ? "oui" : "non");
                break;
        }
    }
}

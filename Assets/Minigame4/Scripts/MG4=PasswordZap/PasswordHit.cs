using UnityEngine;
using UnityEngine.EventSystems;

public class PasswordHit : MonoBehaviour, IPointerClickHandler
{
    public bool isWeak = true;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isWeak)
            ScoreManager.Instance.AddScore(1);
        else
            ScoreManager.Instance.AddScore(-1);

        Destroy(gameObject);
    }
}

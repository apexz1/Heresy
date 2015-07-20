using UnityEngine;
using System.Collections;

public class CardIdentity : MonoBehaviour
{
    public int id;

    public string GetName()
    {
        return CardLibrary.Get().GetCard(id).GetName();
    }
}

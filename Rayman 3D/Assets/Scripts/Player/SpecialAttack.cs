using UnityEngine;
using UnityEngine.UI;

public class SpecialAttack : MonoBehaviour
{
    public int specialAttacks;
    public int numOfHands;

    public Image[] hands;
    public Sprite fullHand;
    public Sprite noHand;

    void Update()
    {
        for (int i = 0; i < hands.Length; i++) {
            if(specialAttacks > numOfHands)
            {
                specialAttacks = numOfHands;
            }
            if(i < specialAttacks)
            {
                hands[i].sprite = fullHand;
            }
            else
            {
                hands[i].sprite = noHand;
            }
            if(i < numOfHands)
            {
                hands[i].enabled = true;
            }
            else
            {
                hands[i].enabled = false;
            }
        }
    }
}

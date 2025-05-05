using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] Card cardPrefab;
    [SerializeField] Transform gridTransform;
    [SerializeField] Sprite[] sprites;

    private List<Sprite> spritePairs;

    Card firstSelected;
    Card secondSelected;

    public Button restartButton;
    public TextMeshProUGUI completed;
    public TextMeshProUGUI failed;
    public TextMeshProUGUI errors;
    public TextMeshProUGUI score;

    int matchCounts;
    int errorCounts;
    int scoreCounts;
    void Start()
    {
        restartButton.gameObject.SetActive(false);
        completed.gameObject.SetActive(false);
        failed.gameObject.SetActive(false);

        PrepareSprites();
        CreateCards();

        restartButton.onClick.AddListener(RestartScene);
    }

    public void RestartScene()
    {
        restartButton.gameObject.SetActive(false);
        completed.gameObject.SetActive(false);
        failed.gameObject.SetActive(false);

        PrepareSprites();
        CreateCards();
    }

    private void PrepareSprites()
    {
        spritePairs = new List<Sprite>();
        
        for (int i = 0; i < sprites.Length; i++)
        {
            spritePairs.Add(sprites[i]);
            spritePairs.Add(sprites[i]);
        }

        ShuffleSprites(spritePairs);
    }

    void CreateCards()
    {
        ClearCards();

        for (int i = 0; i < spritePairs.Count; i++)
        {
            Card card = Instantiate(cardPrefab, gridTransform);
            card.SetIconSprite(spritePairs[i]);
            card.gameManager = this;
        }

        matchCounts = 0;
        errorCounts = 0;
        errors.text = "Errors: " + errorCounts + "/5";

        scoreCounts = 0;
        score.text = "Score: " + scoreCounts;
    }

    private void ClearCards()
    {
        foreach (Transform child in gridTransform)
        {
            Destroy(child.gameObject);
        }
    }

    public void SetSelected(Card card)
    {
        if (card.isClicked == false)
        {
            card.Show();

            if (firstSelected == null)
            {
                firstSelected = card;
                return;
            }

            if (secondSelected == null)
            {
                secondSelected = card;
                StartCoroutine(CheckMatches(firstSelected, secondSelected));
                firstSelected = null;
                secondSelected = null;
            }

        }
    }

    IEnumerator CheckMatches(Card a, Card b)
    {
        yield return new WaitForSeconds(0.5f);
        
        if (a.icon == b.icon)
        {
            matchCounts++;
            scoreCounts += 100;
            score.text = "Score: " + scoreCounts;

            if (matchCounts >= spritePairs.Count / 2)
            {
                completed.gameObject.SetActive(true);
                restartButton.gameObject.SetActive(true);
            }
        }

        else
        {
            a.Hide();
            b.Hide();

            errorCounts++;
            errors.text = "Errors: " + errorCounts + "/5";

            if (errorCounts >= 5)
            {
                failed.gameObject.SetActive(true);
                restartButton.gameObject.SetActive(true);
            }
        }
    }

    void ShuffleSprites(List<Sprite> spriteList)
    {
        for (int i = spriteList.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);

            Sprite temp = spriteList[i];
            spriteList[i] = spriteList[randomIndex];
            spriteList[randomIndex] = temp;
        }
    }
}

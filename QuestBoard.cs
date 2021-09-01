using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestBoard : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] GameObject[] pages;
    [SerializeField] Button left;
    [SerializeField] Button right;
    [SerializeField] GameObject board;

    private int pagesLength;
    private int currentPage;

    public bool questBoardOn;

    // Start is called before the first frame update
    void Start()
    {
        pagesLength = pages.Length;
        board = transform.GetChild(0).gameObject;

        if (board.activeSelf) {
            board.SetActive(false);
        }

        for (int i = 0; i < pagesLength; i++) {
            if (pages[i].activeSelf) {
                pages[i].SetActive(false);
            }
        }

        currentPage = 0;
        questBoardOn = false;

        left.onClick.AddListener(backPage);
        right.onClick.AddListener(nextPage);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toggleQuestBoard() {
        if (!questBoardOn) {
            board.SetActive(true);
            pages[currentPage].SetActive(true);

            questBoardOn = true;
        }
        else if (questBoardOn) {
            board.SetActive(false);
            pages[currentPage].SetActive(false);

            currentPage = 0;
            questBoardOn = false;
        }
    }

    public void nextPage() {
        if (currentPage < pagesLength - 1) {
            pages[currentPage].SetActive(false);
            currentPage++;
            if (!left.gameObject.activeSelf) {
                left.gameObject.SetActive(true);
            }
            pages[currentPage].SetActive(true);
        }
        if (currentPage == pagesLength - 1) {
            right.gameObject.SetActive(false);
        }
    }

    public void backPage() {
        if (currentPage > 0) {
            pages[currentPage].SetActive(false);
            currentPage--;
            if (!right.gameObject.activeSelf) {
                right.gameObject.SetActive(true);
            }
            pages[currentPage].SetActive(true);
        }
        if (currentPage == 0) {
            left.gameObject.SetActive(false);
        }
    }
}

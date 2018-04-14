using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneChangeComponent_Image : MonoBehaviour {

    private GameObject SB_Top;
    private GameObject SB_Info_Score1;
    private GameObject SB_Info_Score2;
    private GameObject SB_Info_Score3;
    private GameObject SB_Res_Score1;
    private GameObject SB_Res_Score2;
    private GameObject SB_Res_Score3;
    private GameObject SB_ResultRank;
    private GameObject SB_ResultStar;
    private GameObject SB_Btn;

    public UI_Script SB_Time;
    public bool skip = false;

    private static int TIMEBONUS = 2500;
    // Use this for initialization
    void Start ()
    {
        SB_Top = GameObject.Find("SB_Top").gameObject;
        SB_Info_Score1 = GameObject.Find("SB_Inf_Main").gameObject;
        SB_Info_Score2 = GameObject.Find("SB_Inf_Time").gameObject;
        SB_Info_Score3 = GameObject.Find("SB_Inf_Haz").gameObject;
        SB_Res_Score1 = GameObject.Find("SB_Res_Main").gameObject;
        SB_Res_Score2 = GameObject.Find("SB_Res_Time").gameObject;
        SB_Res_Score3 = GameObject.Find("SB_Res_Haz").gameObject;
        SB_ResultStar = GameObject.Find("SB_RankEffect").gameObject;
        SB_ResultRank = GameObject.Find("SB_RankText").gameObject;
        SB_Btn = GameObject.Find("SB_OK_Btn").gameObject;
        DisableAll();

        Scene_Manager.EVSceneChange += DropImage;
	}

    private void DisableAll()
    {
        SB_Top.SetActive(false);
        SB_Info_Score1.SetActive(false);
        SB_Info_Score2.SetActive(false);
        SB_Info_Score3.SetActive(false);
        SB_Res_Score1.SetActive(false);
        SB_Res_Score2.SetActive(false);
        SB_Res_Score3.SetActive(false);
        SB_ResultRank.SetActive(false);
        SB_ResultStar.SetActive(false);
        SB_Btn.SetActive(false);
    }

    private void OnDestroy()
    {
        Scene_Manager.EVSceneChange -= DropImage;
    }

    public void ButtonCall()
    {
        SB_Btn.GetComponent<Button>().interactable = false;
        SB_Btn.GetComponentInChildren<Text>().text = "Preparing...";
        StartCoroutine("WaitForCompletion");
    }

    void DropImage()
    {
        GameObject.Find("Transition").GetComponent<Animator>().SetTrigger("Drop");
        if (GameObject.Find("OverlayCanvas").GetComponent<Scene_Manager>().scoreboardUp == true)
        {
            StartCoroutine("ScoreBoardCompletion");
        }
        else
        {
            StartCoroutine("WaitForCompletion");
        }
    }

    IEnumerator WaitForCompletion()
    {
        yield return new WaitForSeconds(1);
        DisableAll();
        GameObject.Find("OverlayCanvas").GetComponent<Scene_Manager>().StartLoad();
        while (GameObject.Find("OverlayCanvas").GetComponent<Scene_Manager>().inLoad == true)
        yield return new WaitForSeconds(0.05f);
        GameObject.Find("OverlayCanvas").GetComponent<Scene_Manager>().FinishedLoad();
        
    }

    IEnumerator ScoreBoardCompletion()
    {
        // This handles the showcase of scores:

        DataContainer_Character datac = GameObject.Find("CharacterData").GetComponent<DataContainer_Character>();
        skip = false;
        yield return new WaitForSeconds(0.60f);

        // Show the Passed Message.
        SB_Top.SetActive(true);
        List<int> ranks = datac.scoresawarder;
        if (!skip)
        {
            yield return new WaitForSeconds(0.60f);
        }

        // Show Scores;
        SB_Info_Score1.SetActive(true);
        SB_Res_Score1.SetActive(true);
        List<int> scores = datac.actualscores;
        int pts = 0;
        SB_Res_Score1.GetComponent<Text>().text = string.Format("{0, 5}", pts);
        while (scores.Count > 0)
        {
            pts += scores[scores.Count - 1];
            scores.RemoveAt(scores.Count - 1);
            SB_Res_Score1.GetComponent<Text>().text = string.Format("{0, 5}", pts);
            if (!skip)
            {
                yield return new WaitForSeconds(0.04f);
            }
        }

        if (!skip)
        {
            yield return new WaitForSeconds(0.60f);
        }
        int remainingtime = SB_Time.timecounter;
        int maxtime = SB_Time.timemax;
        int persecond = (TIMEBONUS / maxtime);
        ranks.Add(7 - Mathf.FloorToInt((remainingtime * 7) / maxtime));
        SB_Info_Score2.SetActive(true);
        SB_Res_Score2.SetActive(true);
        pts = 0;
        while (remainingtime > 0)
        {
            pts += persecond;
            remainingtime--;
            SB_Res_Score2.GetComponent<Text>().text = string.Format("{0, 5}", pts);
            if (!skip)
            {
                yield return new WaitForSeconds(0.02f);
            }
        }
        datac.ChangePoints(pts);

        if (!skip)
        {
            yield return new WaitForSeconds(0.60f);
        }
        SB_Info_Score3.SetActive(true);
        SB_Res_Score3.SetActive(true);

        if (!skip)
        {
            yield return new WaitForSeconds(0.60f);
        }
        int finalrank = 0;
        for (int i = 0; i < ranks.Count; i++)
        {
            finalrank += ranks[i];
        }
        finalrank = (finalrank / ranks.Count);

        SB_ResultRank.SetActive(true);
        SB_ResultStar.SetActive(true);
        if (!skip)
        {
            int suspenserank = Random.Range(0, 8);
            int suspensetime = Random.Range(10, 20);
        
            for (int i = 0; i < suspensetime; i++)
            {
                suspenserank += 1;
                if (suspenserank > 7)
                {
                    suspenserank = 0;
                }
                SB_ResultRank.GetComponent<Text>().text = datac.GetRankName(suspenserank);
                SB_ResultRank.GetComponent<Text>().color = datac.GetRankColor2(suspenserank);
                SB_ResultStar.GetComponent<Image>().color = datac.GetRankColor1(suspenserank);
                yield return new WaitForSeconds(0.05f);
            }
        }
        SB_ResultRank.GetComponent<Text>().text = datac.GetRankName(finalrank);
        SB_ResultRank.GetComponent<Text>().color = datac.GetRankColor2(finalrank);
        SB_ResultStar.GetComponent<Image>().color = datac.GetRankColor1(finalrank);

        if (!skip)
        {
            yield return new WaitForSeconds(0.60f);
        }
        SB_Btn.GetComponentInChildren<Text>().text = "Continue!";
        SB_Btn.SetActive(true);
        SB_Btn.GetComponent<Button>().interactable = true;
        skip = false;
    }
}

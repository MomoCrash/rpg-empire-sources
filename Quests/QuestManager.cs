using System.Linq;
using TMPro;
using UnityEngine;

public class QuestManager : MonoBehaviour
{

    [Header("Quests System")]

    public QuestGroup[] QuestsGroups;
    public int CurrentGroupProgression;
    public int CurrentQuestProgression;

    [Header("Quest Parameters")]
    public ProgressBar QuestProgressBar = new ProgressBar();
    public TextMeshProUGUI QuestText;

    [Header("Current Quest Attributes")]
    public Quest CurrentQuest;
    public int InQuestProgression;

    public void Start()
    {
        CurrentQuest = QuestsGroups[0].Quests[0];
        CurrentGroupProgression = 0;
        CurrentQuestProgression = 0;

        UpdateGraphics();

    }

    public void UpdateGraphics()
    {
        QuestProgressBar.SetValue(CurrentQuest.ObjectivesAmount, InQuestProgression);
        QuestText.text = CurrentQuest.Name;
    }

    public void UpdateQuestProgression()
    {

        UpdateGraphics();

        if (InQuestProgression == CurrentQuest.ObjectivesAmount)
        {

            NextQuest();

        }

    }

    public void NextQuest()
    {
        if (CurrentQuestProgression+1 < QuestsGroups[CurrentGroupProgression].Quests.Length)
        {

            var newQuest = QuestsGroups[CurrentGroupProgression].Quests[CurrentQuestProgression+1];

            CurrentQuest = newQuest;
            InQuestProgression = 0;


            CurrentQuestProgression++;
            UpdateGraphics();

        } else
        {

            if (CurrentGroupProgression+1 < QuestsGroups.Length)
            {

                var newGroup = QuestsGroups[CurrentGroupProgression + 1];

                CurrentQuestProgression = 0;
                CurrentGroupProgression++;

                CurrentQuest = newGroup.Quests[0];
                UpdateGraphics();

            } else
            {
                QuestText.text = "Plus de quêtes pour le moment !";
            }

        }
    }

    public void CheckQuestObjectives(int id, Objectives objective)
    {
        if (CurrentQuest.ObjectivesID.Contains(id) && objective == CurrentQuest.Objective)
        {
            InQuestProgression++;
            UpdateQuestProgression();
        }
    }

    public void KillMob(int id)
    {
        CheckQuestObjectives(id, Objectives.KILL);
    }

    public void CollectItem(int id)
    {
        CheckQuestObjectives(id, Objectives.COLLECT);
    }

    public void BuildBuilding(int id)
    {
        CheckQuestObjectives(id, Objectives.BUILD);
    }

    public void BreakRessource(int id)
    {
        CheckQuestObjectives(id, Objectives.BREAK);
    }

}

[System.Serializable]
public class QuestGroup
{

    public string Name = "";
    public string Description;
    public Quest[] Quests;

}

[System.Serializable]
public class Quest
{

    public string Name = "";
    public string Description;
    public Objectives Objective;

    public int ObjectivesAmount = 1;
    public int[] ObjectivesID;

}


public enum Objectives
{

    COLLECT,
    BREAK,
    BUILD,
    KILL

}
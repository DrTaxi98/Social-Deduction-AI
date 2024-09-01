using System.Collections.Generic;
using UnityEngine;

public class Meeting : MonoBehaviour
{
    public MeetingPanel meetingPanel;

    private List<Agent> aliveAgents = null;
    private int turn = 0;

    public void StartMeeting(List<Agent> aliveAgents, Agent startingAgent)
    {
        this.aliveAgents = aliveAgents;
        turn = aliveAgents.IndexOf(startingAgent);
        gameObject.SetActive(true);
    }

    public void NextTurn()
    {
        Agent agent = aliveAgents[turn];
        string message = agent.TakeTurnInMeeting();

        meetingPanel.DisplayAgentMessage(agent, message);

        if (++turn == aliveAgents.Count)
        {
            turn = 0;
        }
    }
}

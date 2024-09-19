using System.Collections.Generic;
using UnityEngine;

public class Meeting : MonoBehaviour
{
    private const string MESSAGE_AND = " and ";
    private const string MESSAGE_COMMA = ", ";
    private const string MESSAGE_SO = MESSAGE_COMMA + "so ";
    private const string MESSAGE_END = ".";

    [Range(1, 50)] public int maxTurn = 20;
    public MeetingPanel meetingPanel;

    private MeetingController meetingController;

    private Agent currentAgent = null;
    private int setupTurn = 0;
    private int turn = 0;
    private int votingTurn = 0;
    private int startingAgentIndex;

    private Dictionary<Agent, int> votes = new Dictionary<Agent, int>();

    public List<Agent> AliveAgents { get; set; } = null;

    public void StartMeeting(List<Agent> aliveAgents, Agent startingAgent)
    {
        meetingController = GetComponent<MeetingController>();

        AliveAgents = aliveAgents;
        startingAgentIndex = aliveAgents.IndexOf(startingAgent);

        foreach (Agent agent in AliveAgents)
        {
            agent.AddMeetingData();
            votes.Add(agent, 0);
        }

        gameObject.SetActive(true);
    }

    public void NextTurn()
    {
        if (setupTurn < AliveAgents.Count)
        {
            Setup();
        }
        else if (turn < maxTurn)
        {
            TakeTurn();
        }
        else if (votingTurn < AliveAgents.Count)
        {
            Vote();
        }
        else
        {
            EndMeeting();
        }
    }

    public void DisplayMessage(List<RBSDatum> startData, List<RBSDatum> endData)
    {
        string message = DataToMessage(startData) + MESSAGE_SO + DataToMessage(endData) + MESSAGE_END;
        DisplayMessage(message);
    }

    public void DisplayMessage(RBSDatum datum)
    {
        string message = DatumToMessage(datum) + MESSAGE_END;
        DisplayMessage(message);
    }

    public void DisplayMessage(string message)
    {
        meetingPanel.DisplayAgentMessage(currentAgent, message);
    }

    public void ShareData(List<RBSDatum> data)
    {
        foreach (Agent agent in AliveAgents)
        {
            if (agent != currentAgent)
                agent.MeetingAgent.AddData(data);
        }
    }

    public void ShareDatum(RBSDatum datum)
    {
        foreach (Agent agent in AliveAgents)
        {
            if (agent != currentAgent)
                agent.MeetingAgent.AddDatum(datum);
        }
    }

    public void Vote(Agent agent)
    {
        votes.TryGetValue(agent, out int value);
        votes[agent] = ++value;

        string message = "I voted for " + agent.name + MESSAGE_END;
        DisplayMessage(message);

        Debugger.Instance.VoteDebug(agent, value);
    }

    private void Setup()
    {
        currentAgent = AliveAgents[(startingAgentIndex + setupTurn) % AliveAgents.Count];
        if (setupTurn == 0)
        {
            currentAgent.MeetingAgent.ShareDeadBodyInfo();
        }
        else
        {
            currentAgent.MeetingAgent.ShareSelfInfo();
        }
        setupTurn++;
    }

    private void TakeTurn()
    {
        currentAgent = AliveAgents[(startingAgentIndex + turn) % AliveAgents.Count];
        currentAgent.MeetingAgent.TakeTurn();
        turn++;
    }

    private void Vote()
    {
        currentAgent = AliveAgents[(startingAgentIndex + votingTurn) % AliveAgents.Count];
        currentAgent.MeetingAgent.Vote();
        votingTurn++;
    }

    private string DataToMessage(List<RBSDatum> data)
    {
        string message = "";
        foreach (RBSDatum datum in data)
        {
            message += DatumToMessage(datum) + MESSAGE_AND;
        }

        return message.Substring(0, message.Length - MESSAGE_AND.Length);
    }

    private string DatumToMessage(RBSDatum datum)
    {
        return (datum.value is Info info) ?
            info.ToMessage(currentAgent) :
            datum.ToString();
    }

    private void EndMeeting()
    {
        int maxVotes = GetMaxVotedAgents(out List<Agent> maxVotedAgents);
        DisplayEndMessage(maxVotedAgents, maxVotes);

        meetingController.enabled = false;
    }

    private int GetMaxVotedAgents(out List<Agent> maxVotedAgents)
    {
        maxVotedAgents = new List<Agent>();
        int maxVotes = 0;

        foreach (Agent agent in AliveAgents)
        {
            if (votes.TryGetValue(agent, out int value))
            {
                if (value == maxVotes)
                {
                    maxVotedAgents.Add(agent);
                }
                else if (value > maxVotes)
                {
                    maxVotedAgents.Clear();
                    maxVotedAgents.Add(agent);
                    maxVotes = value;
                }
            }
        }

        return maxVotes;
    }

    private void DisplayEndMessage(List<Agent> maxVotedAgents, int maxVotes)
    {
        Agent messageAgent;
        string message = "";

        if (maxVotedAgents.Count == 1)
        {
            messageAgent = maxVotedAgents[0];

            message += messageAgent.name + " was voted out with " + maxVotes + " votes!\n";

            if (messageAgent != GameManager.Instance.Killer)
            {
                message += messageAgent.name + " was not the killer! ";
            }
        }
        else
        {
            messageAgent = GameManager.Instance.Killer;

            for (int i = 0; i < maxVotedAgents.Count; i++)
            {
                message += maxVotedAgents[i].name;

                if (i < maxVotedAgents.Count - 2)
                    message += MESSAGE_COMMA;
                else if (i == maxVotedAgents.Count - 2)
                    message += MESSAGE_AND;
            }

            message += " received " + maxVotes + " votes, so no one was voted out!\n";
        }

        message += GameManager.Instance.Killer.name + " was the killer!";
        meetingPanel.DisplayAgentMessage(messageAgent, message);
    }
}

using System.Collections.Generic;
using UnityEngine;

public class Meeting : MonoBehaviour
{
    private const string MESSAGE_DATA_SEPARATOR = " and ";
    private const string START_END_SEPARATOR = ", so ";
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
        string message = DataToMessage(startData) + START_END_SEPARATOR + DataToMessage(endData) + MESSAGE_END;
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
            message += DatumToMessage(datum) + MESSAGE_DATA_SEPARATOR;
        }

        return message.Substring(0, message.Length - MESSAGE_DATA_SEPARATOR.Length);
    }

    private string DatumToMessage(RBSDatum datum)
    {
        string message = datum.ToString();

        if ((datum.value is ViewInfo viewInfo && viewInfo.SeeingAgent == currentAgent) ||
            (datum.value is LocationInfo locationInfo && locationInfo.Agent == currentAgent))
            message = ReplaceName(message, currentAgent.name, "I");

        if (datum.value is ViewInfo info && info.Agent == currentAgent)
            message = ReplaceName(message, currentAgent.name, "me");

        return message;
    }

    private string ReplaceName(string message, string name, string replacement)
    {
        int nameIndex = message.IndexOf(name);
        string result = message.Substring(0, nameIndex) + replacement +
            message.Substring(nameIndex + currentAgent.name.Length);
        return result;
    }

    private void EndMeeting()
    {
        Agent maxVotedAgent = null;
        int maxVotes = 0;

        foreach (Agent agent in AliveAgents)
        {
            if (votes.TryGetValue(agent, out int value) && value > maxVotes)
            {
                maxVotedAgent = agent;
                maxVotes = value;
            }
        }

        string message = maxVotedAgent.name + " was voted out!\n" +
            maxVotedAgent.name + " was ";

        if (maxVotedAgent is Killer)
        {
            message += "the killer!";
        }
        else
        {
            message += "not the killer! It was " + GameManager.Instance.Killer.name + "!";
        }

        meetingPanel.DisplayAgentMessage(maxVotedAgent, message);

        meetingController.enabled = false;
    }
}

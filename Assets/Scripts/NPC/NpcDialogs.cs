using UnityEngine;

public class NpcDialogs : MonoBehaviour
{
    public Sprite _avatar;

    public string _questDescription;
    [Multiline(10)] public string _questActualDescription;
    [Multiline(10)] public string _questHelpDescription;
    [Multiline(10)] public string _questDone;
    [Multiline(10)] public string _greetings;
    [Multiline(10)] public string _attention;
    [Multiline(10)] public string _questCompete;
    public string _DoneQuestDescription;

    public string _barterText;
    public string _questText;
    public string _exitText;
    public string _doneText;

    public bool _isLiar;
    public bool _isTrader;
    public bool _isQuest;
    public bool _isAdventure;
}

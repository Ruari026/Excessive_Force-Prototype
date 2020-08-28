using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DialogueParser
{
    public static string defaultFilePath = Application.streamingAssetsPath + "/DialogueSets/";

    public static DialogueTree LoadFromFilePath(string filePath)
    {
        DialogueTree newDialogueTree = new DialogueTree();

        string jsonString = File.ReadAllText(filePath);
        newDialogueTree = JsonUtility.FromJson<DialogueTree>(jsonString);

        return newDialogueTree;
    }
}

[System.Serializable]
public class DialogueTree
{
    public List<DialogueOption> dialogueOptions = new List<DialogueOption>();
}

[System.Serializable]
public struct DialogueOption
{
    public DialogueType type;

    public string npcText;
    public List<string> playerOptions;
    public List<int> dialogueConnections;

    public string endEventCode;
}

public enum DialogueType
{ 
    DIALOGUE_NORMAL,
    DIALOGUE_OPTIONS,
    DIALOGUE_END
}
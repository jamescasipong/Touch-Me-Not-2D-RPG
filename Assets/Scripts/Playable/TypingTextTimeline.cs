using UnityEngine;
using UnityEngine.Playables;

public class TypingTextTimeline : PlayableAsset
{
    public string dialogueText;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<TypingTextPlayable>.Create(graph);

        TypingTextPlayable behaviour = playable.GetBehaviour();
        behaviour.dialogueText = dialogueText;
        

        return playable;
    }
}

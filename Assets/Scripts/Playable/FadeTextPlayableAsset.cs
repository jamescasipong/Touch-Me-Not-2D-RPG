using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class FadeTextPlayableAsset : PlayableAsset
{
    public string dialogueText;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<FadeTextPlayableBehavior>.Create(graph);

        FadeTextPlayableBehavior behaviour = playable.GetBehaviour();
        behaviour.dialogueText = dialogueText;


        return playable;
    }
}

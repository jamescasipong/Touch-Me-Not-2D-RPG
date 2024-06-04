using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class NarrationTextPlayableAsset : PlayableAsset
{
    public string dialogueText;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<NarrationTextPlayableBehavior>.Create(graph);

        NarrationTextPlayableBehavior behaviour = playable.GetBehaviour();
        behaviour.dialogueText = dialogueText;


        return playable;
    }
}

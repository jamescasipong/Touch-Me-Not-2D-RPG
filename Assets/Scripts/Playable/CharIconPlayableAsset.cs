using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CharIconPlayableAsset : PlayableAsset
{
    public Sprite charIcon;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<CharIconPlayableBehavior>.Create(graph);

        CharIconPlayableBehavior behaviour = playable.GetBehaviour();
        behaviour.charIcon = charIcon;


        return playable;
    }
}

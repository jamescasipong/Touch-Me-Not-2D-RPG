using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

[System.Serializable]
public class Dialog
{
    [SerializeField] private List<DialogLine> npcLines;


    /*[SerializeField] private List<string> lines;

    public List<string> Lines
    {
        get { return lines; }
    }*/

    public List<DialogLine> NPCLines
    {
        get { return npcLines; }
    }

}

[System.Serializable]
public class DialogLine
{
    [SerializeField] private string characterName;
    [SerializeField] private Sprite characterIcon;
    [SerializeField] private string text;

    public string CharacterName
    {
        get { return characterName; }
    }

    public Sprite CharacterIcon
    {
        get { return characterIcon; }
    }

    public string Text
    {
        get { return ModifyText(text); }
    }


    private string ModifyText(string originalText)
    {
        originalText = Regex.Replace(originalText, "san diego", "San Diego", RegexOptions.IgnoreCase);
        originalText = Regex.Replace(originalText, "indio", "Indio", RegexOptions.IgnoreCase);
        originalText = Regex.Replace(originalText, "isabel", "Isabel", RegexOptions.IgnoreCase);
        originalText = Regex.Replace(originalText, "Maraming Salamat", "Maraming salamat", RegexOptions.IgnoreCase);
        originalText = Regex.Replace(originalText, "Walang Anuman", "Walang anuman", RegexOptions.IgnoreCase);
        originalText = Regex.Replace(originalText, "europa", "Europa", RegexOptions.IgnoreCase);
        originalText = originalText.Replace("Okay", "Sige").Replace("okay", "sige").Replace("gathering", "pagtitipon");

        

        // Combine the sentences back into the modified text

        /*char[] sentenceEndSymbols = { '.', '!', '?'};
        if (!sentenceEndSymbols.Any(originalText.EndsWith) && !string.IsNullOrWhiteSpace(originalText))
        {
            originalText += ".";
        }*/



        if (MainMenu.instance.genderState == GenderState.Female)
        {
            originalText = originalText.Replace("Iho", "Iha").Replace("iho", "iha");
        }

        

        return originalText;
    }
}

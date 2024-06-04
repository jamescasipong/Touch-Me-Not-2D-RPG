using UnityEngine;
using CodeMonkey.Utils;
using UnityEngine.UI;

public class QuestPointer : MonoBehaviour
{
    //private Vector3 targetPosition;
    public Transform targetPosition;
    public GameObject targetGameObject;
    private RectTransform pointerRectTransform;
    QuestList questList;
    public QuestBase quest;

    private void Start()
    {
        questList = QuestList.GetQuestList();
    }

    private void Awake()
    {

        pointerRectTransform = transform.Find("Pointer").GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (questList.IsStarted(quest.Name))
        {
            pointerRectTransform.GetComponent<Image>().enabled = true;


            {
                Vector3 toPosition = targetPosition.position;
                Vector3 fromPosition = Camera.main.transform.position;

                fromPosition.z = 0f;

                Vector3 dir = (toPosition - fromPosition).normalized;
                float angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) % 360;


                pointerRectTransform.localEulerAngles = new Vector3(0, 0, angle);
            }

            if (questList.IsCompleted(quest.Name))
            {
                pointerRectTransform.GetComponent<Image>().enabled = false;
            }

        }
        else
        {
            pointerRectTransform.GetComponent<Image>().enabled = false;
        }
    }
}
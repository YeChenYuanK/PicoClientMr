using UnityEngine;
using System.Collections;

[System.Serializable]
public class StepParam {

    public enum OpratateType
    {
        show = 0,
        hide = 1,
        showAndPlay = 2,
        trigger = 3
    }

    public float ExeTime;
    public GameObject target;
        //private BaseEventObj eventComponent;
        //public bool ShowAndHide;
    //    public bool LightFlashing;
    //    public HighlighterFlashing highlighter;
    public OpratateType type;
    //public string ClipName;

    public bool IsExe;
    public bool IsUpdateAnimParam = false;

    public void Init(Animation anim, int index)
        {
            AnimationClip clip = anim.clip;
            //eventComponent = target.GetComponent<BaseEventObj>();
            //        highlighter = target.GetComponent<HighlighterFlashing>();
            //ShowEvent
            AnimationEvent aniEvent = new AnimationEvent();
            aniEvent.functionName = "EventCallback";
            //aniEvent.time = StartTime;
            aniEvent.stringParameter = index.ToString();
            aniEvent.intParameter = (int)OpratateType.show;
            clip.AddEvent(aniEvent);

            aniEvent = new AnimationEvent();
            aniEvent.functionName = "EventCallback";
            //aniEvent.time = EndTime;
            aniEvent.stringParameter = index.ToString();
            aniEvent.intParameter = (int)OpratateType.hide;
            clip.AddEvent(aniEvent);

        }

    public void Exe()
    {
        if(type == OpratateType.show)
        {
            ObjectUtil.UpdateObjectActive(target, true);
        }else if (type == OpratateType.hide)
        {
            ObjectUtil.UpdateObjectActive(target, false);
        }
        else if(type == OpratateType.showAndPlay)
        {

        }else if(type == OpratateType.trigger)
        {
            target.GetComponent<StepTrigger>().OnTrigger();
        }
    }

        public void EventCallback(AnimationEvent aevent)
        {
            //if (ShowAndHide)
            //{
            //    ObjectUtil.UpdateObjectActive(target, aevent.intParameter == (int)OpratateType.show);
            //}
            //        if(LightFlashing)
            //        {
            //            if(highlighter == null)
            //            {
            //                Debug.Log(target.name + " object attack Highlighter Component");
            //            }
            //            if (aevent.intParameter == (int)OpratateType.show)
            //                highlighter.enabled = true;
            //            else
            //            {
            //                target.GetComponent<HighlightingSystem.Highlighter>().enabled = false;
            //            }
            //        }
        }

    }
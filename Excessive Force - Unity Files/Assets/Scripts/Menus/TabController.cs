using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TabController : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public Animator tabAnimatorController;
    public UnityEvent onTabOpen;
    public UnityEvent onTabClose;

    // If Target == An Instance Of A Tab Controller it should be assumed that that tab is open
    private static TabController target;


    /*
    ====================================================================================================
    UI Interaction
    ====================================================================================================
    */
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (target != this)
        {
            tabAnimatorController.SetTrigger("Highlighted");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // If The Tab Is Open Close It
        if (target == this)
        {
            CloseTab(this);
        }
        // If The Tab Is Closed Open it
        else
        {
            // If A Tab Is Already Open
            if (target != null)
            {
                CloseTab(target); 
            }

            // Set This Tab To Open
            OpenTab(this);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (target != this)
        {
            tabAnimatorController.SetTrigger("Normal");
        }
    }


    /*
    ====================================================================================================
    Tab Controls
    ====================================================================================================
    */
    public void OpenTab(TabController tabToOpen)
    {
        tabAnimatorController.SetTrigger("Pressed");
        target = this;
        if (onTabOpen != null)
        {
            onTabOpen.Invoke();
        }
    }

    public void CloseTab(TabController tabToClose)
    {
        target = null;

        tabToClose.tabAnimatorController.SetTrigger("Normal");
        if (tabToClose.onTabClose != null)
        {
            tabToClose.onTabClose.Invoke();
        }
    }
}

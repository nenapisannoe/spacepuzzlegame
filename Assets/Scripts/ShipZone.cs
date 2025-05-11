using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ShipZone : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] public FactionType HostFaction;
    [SerializeField] GameObject lockIcon;
    [SerializeField] GameObject Mark;
    [SerializeField] public int relationshipWithFactionRequired;
    [SerializeField] int zoneHubSceneIndex;
    private int _x;
    private int _y;

    private bool enterLocked = true;
    private bool exitLocked = false;

    public void SetZoneCoordinates(int x, int y)
    {
        _x = x;
        _y = y;
    }

    public bool IsExitLocked()
    {
        return exitLocked;
    }

    public void Unlock()
    {
        lockIcon.SetActive(false);
        enterLocked = false;
    }
    public void Lock()
    {
        lockIcon.SetActive(true);
        exitLocked = true;
    }

    public void AddMark()
    {
        Mark.SetActive(true);
    }
    public void RemoveMark()
    {
        Mark.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(!enterLocked && !MapUI.Instance.GetCurrentZone().IsExitLocked())
            MoveToZone();
        else
            Debug.Log("They dont want you here mate");
    }

    private void MoveToZone()
    {
        if(IsAdjacent(_x, _y))
        {
            MapUI.Instance.MarkOnMap(_x,_y);
            SceneManager.LoadScene(zoneHubSceneIndex);
        }
    }
    private bool IsAdjacent(int targetX, int targetY)
    {
        int deltaX = Mathf.Abs(targetX - MapUI.Instance.GetCurrentZoneX());
        int deltaY = Mathf.Abs(targetY - MapUI.Instance.GetCurrentZoneY());

        return (deltaX == 1 && deltaY == 0) || (deltaX == 0 && deltaY == 1);
    }
}

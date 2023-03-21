using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Animator anim;
    int openID;

    void Start()
    {
        anim = GetComponent<Animator>();
        openID = Animator.StringToHash("Open");
        GameManager.ReigsterDoor(this);
    }

    public void Open()
    {
        anim.SetTrigger(openID);
        AudioManger.PlayDoorOpenAudio();
    }


}

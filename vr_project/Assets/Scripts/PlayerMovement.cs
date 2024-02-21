using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class PlayerMovement : MonoBehaviour
{
    //Создаем переменную для хранения координаты касания пользователем точпада
    private SteamVR_Action_Vector2 touchpad = null;
    //Создаем переменнуб для определения коснулся ли пользователь точпада
    private SteamVR_Action_Boolean m_Boolean = null;
    //Создаем переменную для компонента Character Controller объекта
    private CharacterController controller = null;
    //Создаем переменную, котора определяет скорость перемещения
    public float speed = 1.0f;
    //Создаем переменную, которая определяет может ли пользователь в данный момент ходить
    private bool checkWalk = false;

    private void Awake()
    {
        //Получаем действие Touchpad из настроек действий SteamVR профиля default 
        touchpad = SteamVR_Actions._default.Touchpad;
        //Получаем действие TouchClick из настроек действий SteamVR профиля default 
        m_Boolean = SteamVR_Actions._default.TouchClick;
        //Получаем компонент CharacterController
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        //Если палец пользователя находится на трекпаде, то пользователь может ходить
        if (m_Boolean.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            checkWalk = true;
        }

        //Если же пользователь уберет палец с трекпада, то пользователь перестанет двигаться
        if (m_Boolean.GetStateUp(SteamVR_Input_Sources.RightHand))
        {
            checkWalk = false;
        }

        //Если палец находится на удалении от центра трекпада
        if (touchpad.axis.magnitude > 0.1f)
        {
            //если пользователь может ходить
            if (checkWalk)
            {
                //то осуществляется перемещение игрока
                Vector3 direction = Player.instance.hmdTransform.TransformDirection(new Vector3(touchpad.axis.x, 0, touchpad.axis.y));
                controller.Move(speed * Time.deltaTime * Vector3.ProjectOnPlane(direction, Vector3.up) - new Vector3(0, 10f, 0) * Time.deltaTime);
            }
        }
    }
}

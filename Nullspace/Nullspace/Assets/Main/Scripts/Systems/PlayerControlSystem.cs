using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

public partial class PlayerControlSystem : SystemBase
{
    PlayerInput InputControl;
    bool isMouseLocked;
    protected override void OnCreate()
    {
        InputControl = new PlayerInput();
        InputControl.Enable();
        InputControl.InShip.Enable();
        isMouseLocked = false;
        //isMouseLocked = true;
    }
    protected override void OnUpdate()
    {
        foreach (var (playerTag, movementAspect) in SystemAPI.Query<PlayerControlledTag, MovementAspect>())
        {
            var thrustInput = InputControl.InShip.Thrust.ReadValue<float>();
            var verticalInput = InputControl.InShip.Vertical.ReadValue<float>();
            var horizontalInput = InputControl.InShip.Horizontal.ReadValue<float>();
            var rollInput = InputControl.InShip.Roll.ReadValue<float>();
            var mouseLockInput = InputControl.InShip.MouseLock.ReadValue<float>();
            var pitchYawInput = InputControl.InShip.PitchYaw.ReadValue<Vector2>();

            var propulsionData = movementAspect.propulsionData;
            var handlingData = movementAspect.handlingData;

            if (thrustInput != 0)
            {
                propulsionData.ValueRW.Thrust[0] = Mathf.Clamp(
                    propulsionData.ValueRW.Thrust[0] + propulsionData.ValueRW.Thrust[2] * thrustInput * SystemAPI.Time.DeltaTime,
                    -propulsionData.ValueRW.Thrust[1],
                    propulsionData.ValueRW.Thrust[1]
                );
            }

            if (verticalInput != 0)
            {
                propulsionData.ValueRW.Vertical[0] = Mathf.Clamp(
                    propulsionData.ValueRW.Vertical[0] + propulsionData.ValueRW.Vertical[2] * verticalInput * SystemAPI.Time.DeltaTime,
                    -propulsionData.ValueRW.Vertical[1],
                    propulsionData.ValueRW.Vertical[1]
                );
            }

            if (horizontalInput != 0)
            {
                propulsionData.ValueRW.Horizontal[0] = Mathf.Clamp(
                    propulsionData.ValueRW.Horizontal[0] + propulsionData.ValueRW.Horizontal[2] * horizontalInput * SystemAPI.Time.DeltaTime,
                    -propulsionData.ValueRW.Horizontal[1],
                    propulsionData.ValueRW.Horizontal[1]
                );
            }

            if (rollInput != 0)
            {
                handlingData.ValueRW.Roll[0] = Mathf.Clamp(
                    handlingData.ValueRW.Roll[0] + handlingData.ValueRW.Roll[2] * rollInput * SystemAPI.Time.DeltaTime,
                    -handlingData.ValueRW.Roll[1],
                    handlingData.ValueRW.Roll[1]
                );
            }

            /* if (mouseLockInput != 0)
                isMouseLocked = !isMouseLocked; */

            if (pitchYawInput != Vector2.zero && isMouseLocked)
            {
                handlingData.ValueRW.Pitch[0] = Mathf.Clamp(
                    handlingData.ValueRW.Pitch[0] + handlingData.ValueRW.Pitch[2] * -pitchYawInput.y * SystemAPI.Time.DeltaTime,
                    -handlingData.ValueRW.Pitch[1],
                    handlingData.ValueRW.Pitch[1]
                );

                handlingData.ValueRW.Yaw[0] = Mathf.Clamp(
                    handlingData.ValueRW.Yaw[0] + handlingData.ValueRW.Yaw[2] * pitchYawInput.x * SystemAPI.Time.DeltaTime,
                    -handlingData.ValueRW.Yaw[1],
                    handlingData.ValueRW.Yaw[1]
                );
            }
        }
    }
}

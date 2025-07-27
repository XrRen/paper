using UnityEngine;

public class DesignDrivenDeadzoneMove : MonoBehaviour
{
    [Header("移动参数")]
    public float maxSpeed = 5f;
    public float decelRate = 10f;

    [Header("死区校准")]
    [Tooltip("0~1,表示校准进度;可在代码里或 Inspector 手动推进")]
    [Range(0f, 1f)] public float calibration = 0f;
    [Tooltip("设计者根据游戏进度自由调整的最大死区比例")]
    [Range(0f, 1f)] public float maxDeadzone = 0.6f;

    float currentVelocity = 0f;

    void Update()
    {
        // 读输入
        float input = 0f;
        if (Input.GetKey(KeyCode.A)) input -= 1f;
        if (Input.GetKey(KeyCode.D)) input += 1f;

        // 用设计参数计算死区
        float deadzone = calibration * maxDeadzone;
        if (Mathf.Abs(input) < deadzone) input = 0f;

        // 平滑速度
        float targetVel = input * maxSpeed;
        currentVelocity = Mathf.MoveTowards(
            currentVelocity, targetVel, decelRate * Time.deltaTime);

        // 应用移动
        transform.Translate(
            Vector3.right * currentVelocity * Time.deltaTime, Space.World);
    }

    // 设计师/关卡事件可以调用这个方法来推进 calibration
    public void IncreaseCalibration(float amount)
    {
        calibration = Mathf.Clamp01(calibration + amount);
    }
}

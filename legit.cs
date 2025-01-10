using System;
using System.Diagnostics;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using BepInEx;
using GorillaLocomotion;
using UnityEngine;

[BepInPlugin("anarxy.s.speedboost.fweh", "SpedboobsFweh", "1.0")]
public class Plugin : BaseUnityPlugin
{
    private void Update()
    {
        Plugin.GripSpeedBoost();
        Plugin.BoneESP();
    }

    public static void BoneESP()
    {
        foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
        {
            bool flag = vrrig != null && !vrrig.isOfflineVRRig;
            if (flag)
            {
                Color color = vrrig.mainSkin.material.name.Contains("fected") ? Color.red : Color.green;
                Material material = new Material(Shader.Find("GUI/Text Shader"));
                material.color = color;
                Plugin.DrawCartoonishSkull(vrrig, material);
                Plugin.DrawSpine(vrrig, material);
                for (int i = 0; i < Plugin.bones.Length; i += 2)
                {
                    Plugin.DrawLine(vrrig.mainSkin.bones[Plugin.bones[i]].gameObject, material, vrrig.mainSkin.bones[Plugin.bones[i]].position, vrrig.mainSkin.bones[Plugin.bones[i + 1]].position);
                }
                Plugin.CreateArmObject(vrrig, "LeftArm", Plugin.bones[10], Plugin.bones[11], material);
                Plugin.CreateArmObject(vrrig, "RightArm", Plugin.bones[6], Plugin.bones[7], material);
            }
        }
    }

    private static void DrawLine(GameObject target, Material material, Vector3 start, Vector3 end)
    {
        GameObject gameObject = new GameObject("Line");
        gameObject.transform.SetParent(target.transform);
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.03f;
        lineRenderer.endWidth = 0.03f;
        lineRenderer.material = material;
        lineRenderer.useWorldSpace = true;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
        UnityEngine.Object.Destroy(gameObject, Time.deltaTime);
    }

    private static void DrawCartoonishSkull(VRRig vrrig, Material material)
    {
        GameObject gameObject = new GameObject("Skull");
        gameObject.transform.SetParent(vrrig.head.rigTarget.transform);
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.localRotation = Quaternion.identity;
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.useWorldSpace = false;
        lineRenderer.loop = true;
        lineRenderer.startWidth = 0.03f;
        lineRenderer.endWidth = 0.03f;
        lineRenderer.material = material;
        Vector3[] array = new Vector3[]
        {
            new Vector3(0f, 0.1f, 0f),
            new Vector3(-0.06f, 0.09f, 0f),
            new Vector3(-0.09f, 0.06f, 0f),
            new Vector3(-0.1f, 0.02f, 0f),
            new Vector3(-0.09f, -0.04f, 0f),
            new Vector3(0f, -0.07f, 0f),
            new Vector3(0.09f, -0.04f, 0f),
            new Vector3(0.1f, 0.02f, 0f),
            new Vector3(0.09f, 0.06f, 0f),
            new Vector3(0.06f, 0.09f, 0f)
        };
        lineRenderer.positionCount = array.Length;
        lineRenderer.SetPositions(array);
        UnityEngine.Object.Destroy(gameObject, Time.deltaTime);
    }

    private static void DrawSpine(VRRig vrrig, Material material)
    {
        GameObject gameObject = new GameObject("Spine");
        gameObject.transform.SetParent(vrrig.head.rigTarget.gameObject.transform);
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.endWidth = 0.03f;
        lineRenderer.startWidth = 0.03f;
        lineRenderer.material = material;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, vrrig.head.rigTarget.transform.position + new Vector3(0f, 0.16f, 0f));
        lineRenderer.SetPosition(1, vrrig.head.rigTarget.transform.position - new Vector3(0f, 0.4f, 0f));
        for (int i = 0; i < Plugin.bones.Length; i += 2)
        {
            Plugin.DrawLine(vrrig.mainSkin.bones[Plugin.bones[i]].gameObject, material, vrrig.mainSkin.bones[Plugin.bones[i]].position, vrrig.mainSkin.bones[Plugin.bones[i + 1]].position);
        }
        UnityEngine.Object.Destroy(gameObject, Time.deltaTime);
    }

    private static void CreateArmObject(VRRig vrrig, string name, int bone1, int bone2, Material material)
    {
        GameObject gameObject = new GameObject(name);
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.material = material;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, vrrig.mainSkin.bones[bone1].position);
        lineRenderer.SetPosition(1, vrrig.mainSkin.bones[bone2].position);
        gameObject.transform.SetParent(vrrig.transform);
        UnityEngine.Object.Destroy(gameObject, Time.deltaTime);
    }

    public static void GripSpeedBoost()
    {
        bool leftGrab = ControllerInputPoller.instance.leftGrab;
        bool rightGrab = ControllerInputPoller.instance.rightGrab;
        bool flag = ControllerInputPoller.instance.leftControllerPrimaryButton || ControllerInputPoller.instance.leftControllerSecondaryButton || ControllerInputPoller.instance.rightControllerPrimaryButton || ControllerInputPoller.instance.rightControllerSecondaryButton;
        bool flag2 = rightGrab && leftGrab && !flag;
        if (flag2)
        {
            Player.Instance.maxJumpSpeed = 9f;
            Player.Instance.jumpMultiplier = 1.4f;
        }
    }

    public static bool rightHand = false;

    public static int[] bones = new int[]
    {
        4,
        3,
        5,
        4,
        19,
        18,
        20,
        19,
        3,
        18,
        21,
        20,
        22,
        21,
        25,
        21,
        29,
        21,
        31,
        29,
        27,
        25,
        24,
        22,
        6,
        5,
        7,
        6,
        10,
        6,
        14,
        6,
        16,
        14,
        12,
        10,
        9,
        7
    };
}

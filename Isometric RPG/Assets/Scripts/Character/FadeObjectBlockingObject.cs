using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeObjectBlockingObject : MonoBehaviour
{
    [SerializeField]
    private LayerMask LayerMask;

    [SerializeField]
    private Transform Target;

    [SerializeField]
    private Camera Camera;

    [SerializeField]
    [Range(0, 1f)]
    private float FadedAlpha = 0.33f;

    [SerializeField]
    private bool RetainShadows = true;

    [SerializeField]
    private Vector3 TargetPositionOffset = Vector3.up;

    [SerializeField]
    private float FadeSpeed = 1;

    [Header("Read Only Data")]
    [SerializeField]
    private List<FadingObject> ObjectsBlockingView = new List<FadingObject>();
    private Dictionary<FadingObject, Coroutine> RunningCoroutines =
        new Dictionary<FadingObject, Coroutine>();

    private List<RaycastHit> Hits = new List<RaycastHit>();

    private void Awake()
    {
        Target = GameObject.FindGameObjectWithTag("Player").transform;
        Camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        StartCoroutine(CheckForObjects());
    }

    private IEnumerator CheckForObjects()
    {
        while (true)
        {
            Hits.Clear();
            Vector3 targetPosition = Target.transform.position + TargetPositionOffset;

            float gridSize = 0.1f; // Adjust this value to change the density of the rays
            Vector3 screenPoint = Camera.WorldToViewportPoint(targetPosition);

            for (float x = -0.5f; x <= 0.5f; x += gridSize)
            {
                for (float y = -0.5f; y <= 0.5f; y += gridSize)
                {
                    Vector3 offset = new Vector3(x, y, 0);
                    Vector3 rayOrigin = Camera.ViewportToWorldPoint(
                        new Vector3(
                            screenPoint.x + offset.x,
                            screenPoint.y + offset.y,
                            screenPoint.z
                        )
                    );
                    Vector3 direction = (targetPosition - rayOrigin).normalized;
                    float maxDistance = Vector3.Distance(rayOrigin, targetPosition);

                    if (
                        Physics.Raycast(
                            rayOrigin,
                            direction,
                            out RaycastHit hit,
                            maxDistance,
                            LayerMask
                        )
                    )
                    {
                        Hits.Add(hit);
                        FadingObject fadingObject = GetFadingObjectFromHit(hit);

                        if (fadingObject != null && !ObjectsBlockingView.Contains(fadingObject))
                        {
                            if (RunningCoroutines.ContainsKey(fadingObject))
                            {
                                if (RunningCoroutines[fadingObject] != null)
                                {
                                    StopCoroutine(RunningCoroutines[fadingObject]);
                                }

                                RunningCoroutines.Remove(fadingObject);
                            }

                            RunningCoroutines.Add(
                                fadingObject,
                                StartCoroutine(FadeObjectOut(fadingObject))
                            );
                            ObjectsBlockingView.Add(fadingObject);
                        }
                    }
                }
            }

            FadeObjectsNoLongerBeingHit();
            yield return null;
        }
    }

    private void FadeObjectsNoLongerBeingHit()
    {
        List<FadingObject> objectsToRemove = new List<FadingObject>(ObjectsBlockingView.Count);

        foreach (FadingObject fadingObject in ObjectsBlockingView)
        {
            bool objectIsBeingHit = false;
            foreach (RaycastHit hit in Hits)
            {
                FadingObject hitFadingObject = GetFadingObjectFromHit(hit);
                if (hitFadingObject != null && fadingObject == hitFadingObject)
                {
                    objectIsBeingHit = true;
                    break;
                }
            }

            if (!objectIsBeingHit)
            {
                if (RunningCoroutines.ContainsKey(fadingObject))
                {
                    if (RunningCoroutines[fadingObject] != null)
                    {
                        StopCoroutine(RunningCoroutines[fadingObject]);
                    }
                    RunningCoroutines.Remove(fadingObject);
                }

                RunningCoroutines.Add(fadingObject, StartCoroutine(FadeObjectIn(fadingObject)));
                objectsToRemove.Add(fadingObject);
            }
        }

        foreach (FadingObject removeObject in objectsToRemove)
        {
            ObjectsBlockingView.Remove(removeObject);
        }
    }

    private IEnumerator FadeObjectOut(FadingObject FadingObject)
    {
        foreach (Material material in FadingObject.Materials)
        {
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.SetInt("_Surface", 1);

            material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

            material.SetShaderPassEnabled("DepthOnly", false);
            material.SetShaderPassEnabled("SHADOWCASTER", RetainShadows);

            material.SetOverrideTag("RenderType", "Transparent");

            material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
        }

        float time = 0;

        while (FadingObject.Materials[0].color.a > FadedAlpha)
        {
            foreach (Material material in FadingObject.Materials)
            {
                if (material.HasProperty("_Color"))
                {
                    material.color = new Color(
                        material.color.r,
                        material.color.g,
                        material.color.b,
                        Mathf.Lerp(FadingObject.InitialAlpha, FadedAlpha, time * FadeSpeed)
                    );
                }
            }

            time += Time.deltaTime;
            yield return null;
        }

        if (RunningCoroutines.ContainsKey(FadingObject))
        {
            StopCoroutine(RunningCoroutines[FadingObject]);
            RunningCoroutines.Remove(FadingObject);
        }
    }

    private IEnumerator FadeObjectIn(FadingObject FadingObject)
    {
        float time = 0;

        while (FadingObject.Materials[0].color.a < FadingObject.InitialAlpha)
        {
            foreach (Material material in FadingObject.Materials)
            {
                if (material.HasProperty("_Color"))
                {
                    material.color = new Color(
                        material.color.r,
                        material.color.g,
                        material.color.b,
                        Mathf.Lerp(FadedAlpha, FadingObject.InitialAlpha, time * FadeSpeed)
                    );
                }
            }

            time += Time.deltaTime;
            yield return null;
        }

        foreach (Material material in FadingObject.Materials)
        {
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            material.SetInt("_ZWrite", 1);
            material.SetInt("_Surface", 0);

            material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;

            material.SetShaderPassEnabled("DepthOnly", true);
            material.SetShaderPassEnabled("SHADOWCASTER", true);

            material.SetOverrideTag("RenderType", "Opaque");

            material.DisableKeyword("_SURFACE_TYPE_TRANSPARENT");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        }

        if (RunningCoroutines.ContainsKey(FadingObject))
        {
            StopCoroutine(RunningCoroutines[FadingObject]);
            RunningCoroutines.Remove(FadingObject);
        }
    }

    private FadingObject GetFadingObjectFromHit(RaycastHit Hit)
    {
        return Hit.collider != null ? Hit.collider.GetComponent<FadingObject>() : null;
    }

    private void OnDrawGizmos()
    {
        if (Camera != null && Target != null)
        {
            Gizmos.color = Color.red;
            Vector3 targetPosition = Target.transform.position + TargetPositionOffset;
            Gizmos.DrawLine(Camera.transform.position, targetPosition);

            Gizmos.color = Color.blue;
            foreach (RaycastHit hit in Hits)
            {
                Gizmos.DrawSphere(hit.point, 0.1f);
            }
        }
    }
}

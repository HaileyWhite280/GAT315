using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class UVScroll : MonoBehaviour
{
    [SerializeField] Vector2 scroll;
    MeshRenderer meshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 offset = Time.time * scroll;
        meshRenderer.material.SetTextureOffset("_MainTex", offset);
    }
}

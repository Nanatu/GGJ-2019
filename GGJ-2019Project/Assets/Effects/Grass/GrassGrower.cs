using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassGrower : MonoBehaviour
{
    private MeshRenderer[] grassStrips;

    public float rad;

    // Start is called before the first frame update
    void Start()
    {
        grassStrips = GetComponentsInChildren<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var grass in grassStrips)
        {
            updateMaterialBlock(grass);
        }
    }

    private void updateMaterialBlock(MeshRenderer spriteRenderer)
    {
        var materialSettings = new MaterialPropertyBlock();

        spriteRenderer.GetPropertyBlock(materialSettings);
        //        var relativeLocal = spriteRenderer.transform.InverseTransformDirection(this.transform.position);

        //if all the things are parented to this control object, the center is the origin
        var relativeLocal = spriteRenderer.transform.InverseTransformPoint(this.transform.position);

        materialSettings.SetVector("_growthOrigin", new Vector4(relativeLocal.x, relativeLocal.y, relativeLocal.z, 1) );
        materialSettings.SetFloat("_growthRadius", rad);

        spriteRenderer.SetPropertyBlock(materialSettings);

    }
}

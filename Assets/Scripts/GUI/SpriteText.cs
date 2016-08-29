using UnityEngine;

public class SpriteText : MonoBehaviour
{

    Site site;
    TextMesh text;
    void Start()
    {
        Transform parent = transform.parent;
        site = parent.parent.GetComponent<Site>();

        Renderer parentRenderer = parent.GetComponent<Renderer>();

        Renderer renderer = GetComponent<Renderer>();
        renderer.sortingLayerID = parentRenderer.sortingLayerID;
        renderer.sortingOrder = parentRenderer.sortingOrder + 1;

        Transform spriteTransform = parent.transform;
        text = GetComponent<TextMesh>();
        Vector3 pos = spriteTransform.position;
        text.text = site.workers.Count.ToString();
    }

    void Update()
    {
        text.text = site.workers.Count.ToString();
    }

}
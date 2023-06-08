using System.Collections;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    private GameObject currentBlock = null;
    private float blockBreakingTime = 1.0f;
    private Shader originalShader = null;

    public Material TestShader;

    // The renderer of the current block
    private Renderer currentBlockRenderer;

    public void PickBlock()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.CompareTag("Block"))
            {
                currentBlock = hit.transform.gameObject;
                currentBlockRenderer = currentBlock.GetComponent<MeshRenderer>(); // Get Renderer

                if (currentBlockRenderer == null)
                {
                    Debug.LogError("No Renderer found on the block object!");
                    return;
                }

                // Get the Material separately
                Material blockMaterial = currentBlockRenderer.material;

                // Save the original shader before changing it to TestShader
                originalShader = blockMaterial.shader;

                // Swap to the TestShader and assign the block's main texture to _MainTex
                blockMaterial.shader = TestShader.shader;
                Texture blockTexture = blockMaterial.mainTexture;
                blockMaterial.SetTexture("_MainTex", blockTexture);

                // Start the coroutine with the material as a parameter
                StartCoroutine(BreakBlock(blockMaterial));
            }
        }
    }

    private IEnumerator BreakBlock(Material blockMaterial)
    {
        float holdTime = 0;

        while (holdTime < blockBreakingTime)
        {
            // If the mouse button has been released, stop the breaking process
            if (!Input.GetMouseButton(0))
            {
                // Reset the DamageLevel when the mouse button is released
                blockMaterial.SetFloat("_DamageLevel", 0);
                Debug.Log("Breaking process stopped. DamageLevel reset to 0.");
                blockMaterial.shader = originalShader;
                yield break; // Stop this coroutine
            }

            holdTime += Time.deltaTime;

            // Update the DamageLevel based on how long the mouse button has been held down
            float damageLevel = holdTime / blockBreakingTime;
            blockMaterial.SetFloat("_DamageLevel", damageLevel);
            Debug.Log("_DamageLevel set to: " + damageLevel);

            yield return null;
        }

        // After block breaking time, destroy the block
        Destroy(currentBlock);
        currentBlock = null;
        originalShader = null;
        Debug.Log("Block destroyed.");
    }
}
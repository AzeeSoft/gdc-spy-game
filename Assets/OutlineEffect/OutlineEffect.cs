/*
//  Copyright (c) 2015 José Guerreiro. All rights reserved.
//
//  MIT license, see http://www.opensource.org/licenses/mit-license.php
//  
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
//  
//  The above copyright notice and this permission notice shall be included in
//  all copies or substantial portions of the Software.
//  
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//  THE SOFTWARE.
*/

using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Rendering;
using UnityEngine.VR;

namespace cakeslice
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Camera))]
    [ExecuteInEditMode]
    public class OutlineEffect : MonoBehaviour
    {
        

        private readonly LinkedSet<Outline> outlines = new LinkedSet<Outline>();

        [ReadOnly] public float OutlineCount;

        [Range(1.0f, 6.0f)]
        public float lineThickness = 1.25f;
        [Range(0, 10)]
        public float lineIntensity = .5f;
        [Range(0, 1)]
        public float fillAmount = 0.2f;

        public Color lineColor0 = Color.red;
        public Color lineColor1 = Color.green;
        public Color lineColor2 = Color.blue;

        public bool SeeThrough = false;

        public bool additiveRendering = false;

        public bool backfaceCulling = true;

        [Header("These settings can affect performance!")]
        public bool cornerOutlines = false;
        public bool addLinesBetweenColors = false;

        [Header("Advanced settings")]
        public bool scaleWithScreenSize = true;
        [Range(0.1f, .9f)]
        public float alphaCutoff = .5f;
        public bool flipY = false;
        public Camera sourceCamera;

        [HideInInspector]
        public Camera outlineCamera;
        Material outline1Material;
        Material outline2Material;
        Material outline3Material;
        Material outlineEraseMaterial;
        Material outlineDepthMaskMaterial;
        Shader outlineShader;
        Shader outlineBufferShader;
        Shader outlineDepthMaskBufferShader;
        [HideInInspector]
        public Material outlineShaderMaterial;
        [HideInInspector]
        public RenderTexture renderTexture;
        [HideInInspector]
        public RenderTexture depthMaskRenderTexture;
        [HideInInspector]
        public RenderTexture extraRenderTexture;

        CommandBuffer commandBuffer;

        Material GetMaterialFromID(int ID)
        {
            if(ID == 0)
                return outline1Material;
            else if(ID == 1)
                return outline2Material;
            else
                return outline3Material;
        }
        List<Material> materialBuffer = new List<Material>();
        Material CreateMaterial(Color emissionColor)
        {
            Material m = new Material(outlineBufferShader);
            m.SetColor("_Color", emissionColor);
            m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            m.SetInt("_ZWrite", 0);
            m.DisableKeyword("_ALPHATEST_ON");
            m.EnableKeyword("_ALPHABLEND_ON");
            m.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            m.renderQueue = 1000;
            return m;
        }

        private void Awake()
        {
//            m_instance = this;
        }

        void Start()
        {
            CreateMaterialsIfNeeded();
            UpdateMaterialsPublicProperties();

            if(sourceCamera == null)
            {
                sourceCamera = GetComponent<Camera>();

                if(sourceCamera == null)
                    sourceCamera = Camera.main;
            }

            if(outlineCamera == null)
            {
                foreach (Camera camera in GetComponentsInChildren<Camera>())
                {
                    if (camera.gameObject.name.Equals("Outline Camera"))
                    {
                        outlineCamera = camera;
                    }
                }
            }

            if (outlineCamera == null)
            {
                GameObject cameraGameObject = new GameObject("Outline Camera");
                cameraGameObject.transform.parent = sourceCamera.transform;
                outlineCamera = cameraGameObject.AddComponent<Camera>();
            }

            outlineCamera.enabled = false;

            renderTexture = new RenderTexture(sourceCamera.pixelWidth, sourceCamera.pixelHeight, 16, RenderTextureFormat.Default);
            depthMaskRenderTexture = new RenderTexture(sourceCamera.pixelWidth, sourceCamera.pixelHeight, 16, RenderTextureFormat.Default);
            extraRenderTexture = new RenderTexture(sourceCamera.pixelWidth, sourceCamera.pixelHeight, 16, RenderTextureFormat.Default);
            UpdateOutlineCameraFromSource();

            commandBuffer = new CommandBuffer();
            commandBuffer.name = "OutlineSource Command Buffer";
//            outlineCamera.AddCommandBuffer(CameraEvent.BeforeImageEffects, commandBuffer);
            sourceCamera.AddCommandBuffer(CameraEvent.BeforeImageEffects, commandBuffer);
        }

        void Update()
        {
            OutlineCount = outlines.Count();

            if (Input.GetKeyDown(KeyCode.Period))
            {
                Debug.Break();
            }
        }

        public void OnPreRender()
        {
            PrepareOutlineSource();
        }

        public void PrepareOutlineSource()
        {
            if(commandBuffer == null)
                return;

            CreateMaterialsIfNeeded();

            if(renderTexture == null || renderTexture.width != sourceCamera.pixelWidth || renderTexture.height != sourceCamera.pixelHeight)
            {
                renderTexture = new RenderTexture(sourceCamera.pixelWidth, sourceCamera.pixelHeight, 16, RenderTextureFormat.Default);
                depthMaskRenderTexture = new RenderTexture(sourceCamera.pixelWidth, sourceCamera.pixelHeight, 16, RenderTextureFormat.Default);
                extraRenderTexture = new RenderTexture(sourceCamera.pixelWidth, sourceCamera.pixelHeight, 16, RenderTextureFormat.Default);
                outlineCamera.targetTexture = renderTexture;
            }

//            renderTexture.DiscardContents();
            renderTexture.Release();
            depthMaskRenderTexture.Release();

            UpdateMaterialsPublicProperties();
            UpdateOutlineCameraFromSource();
            outlineCamera.targetTexture = renderTexture;

            commandBuffer.Clear();

            commandBuffer.SetRenderTarget(depthMaskRenderTexture);
            commandBuffer.ClearRenderTarget(false, true, Color.black);
            if (outlines != null)
            {
                foreach (Outline outline in outlines)
                {
                    LayerMask l = sourceCamera.cullingMask;

                    if (outline != null && outline.CanBeDrawn() && l == (l | (1 << outline.originalLayer)))
                    {
                        for (int v = 0; v < outline.Renderer.sharedMaterials.Length; v++)
                        {
                            Material m = outlineDepthMaskMaterial;

                            SetLineThicknessOnMaterial(m);

                            m.SetInt("_CornerOutlines", cornerOutlines ? 1 : 0);
                            m.SetInt("_SeeThrough", SeeThrough ? 1 : 0);

                            commandBuffer.DrawRenderer(outline.GetComponent<Renderer>(), m, 0, 0);
                            MeshFilter mL = outline.GetComponent<MeshFilter>();
                            if (mL)
                            {
                                for (int i = 1; i < mL.sharedMesh.subMeshCount; i++)
                                    commandBuffer.DrawRenderer(outline.GetComponent<Renderer>(), m, i, 0);
                            }
                            SkinnedMeshRenderer sMR = outline.GetComponent<SkinnedMeshRenderer>();
                            if (sMR)
                            {
                                for (int i = 1; i < sMR.sharedMesh.subMeshCount; i++)
                                    commandBuffer.DrawRenderer(outline.GetComponent<Renderer>(), m, i, 0);
                            }
                        }
                    }
                }
            }

            commandBuffer.SetRenderTarget(renderTexture);
            commandBuffer.ClearRenderTarget(false, true, Color.black);
            if (outlines != null)
            {
                foreach(Outline outline in outlines)
                {
                    LayerMask l = sourceCamera.cullingMask;

                    if(outline != null && outline.CanBeDrawn() && l == (l | (1 << outline.originalLayer)))
                    {
                        for(int v = 0; v < outline.Renderer.sharedMaterials.Length; v++)
                        {
                            Material m = null;

                            if(outline.Renderer.sharedMaterials[v].mainTexture != null && outline.Renderer.sharedMaterials[v])
                            {
                                foreach(Material g in materialBuffer)
                                {
                                    if(g.mainTexture == outline.Renderer.sharedMaterials[v].mainTexture)
                                    {
                                        if(outline.eraseRenderer && g.color == outlineEraseMaterial.color)
                                            m = g;
                                        else if(g.color == GetMaterialFromID(outline.color).color)
                                            m = g;
                                    }
                                }

                                if(m == null)
                                {
                                    if(outline.eraseRenderer)
                                        m = new Material(outlineEraseMaterial);
                                    else
                                        m = new Material(GetMaterialFromID(outline.color));
                                    m.mainTexture = outline.Renderer.sharedMaterials[v].mainTexture;
                                    materialBuffer.Add(m);
                                }
                            }
                            else
                            {
                                if(outline.eraseRenderer)
                                    m = outlineEraseMaterial;
                                else
                                    m = GetMaterialFromID(outline.color);
                            }

                            if(backfaceCulling)
                                m.SetInt("_Culling", (int)UnityEngine.Rendering.CullMode.Back);
                            else
                                m.SetInt("_Culling", (int)UnityEngine.Rendering.CullMode.Off);

                            m.SetTexture("_OutlineDepthMask", depthMaskRenderTexture);

                            commandBuffer.DrawRenderer(outline.GetComponent<Renderer>(), m, 0, 0);
                            MeshFilter mL = outline.GetComponent<MeshFilter>();
                            if(mL)
                            {
                                for(int i = 1; i < mL.sharedMesh.subMeshCount; i++)
                                    commandBuffer.DrawRenderer(outline.GetComponent<Renderer>(), m, i, 0);
                            }
                            SkinnedMeshRenderer sMR = outline.GetComponent<SkinnedMeshRenderer>();
                            if(sMR)
                            {
                                for(int i = 1; i < sMR.sharedMesh.subMeshCount; i++)
                                    commandBuffer.DrawRenderer(outline.GetComponent<Renderer>(), m, i, 0);
                            }
                        }
                    }
                }
            }

            //            outlineCamera.Render();
        }

        private void OnEnable()
        {
            Outline[] o = FindObjectsOfType<Outline>();

            foreach(Outline outline in o)
            {
                if (outline.enabled)
                {
                    outline.BroadcastOutline(this, true);
                }
            }
        }

        void OnDestroy()
        {
            if(renderTexture != null)
                renderTexture.Release();
            if (depthMaskRenderTexture != null)
                depthMaskRenderTexture.Release();
            if (extraRenderTexture != null)
                extraRenderTexture.Release();
            DestroyMaterials();
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            outlineShaderMaterial.SetTexture("_OutlineSource", renderTexture);
//            outlineShaderMaterial.SetTexture("_OutlineDepth", depthMaskRenderTexture);

            if(addLinesBetweenColors)
            {
                Graphics.Blit(source, extraRenderTexture, outlineShaderMaterial, 0);
                outlineShaderMaterial.SetTexture("_OutlineSource", extraRenderTexture);
            }
            Graphics.Blit(source, destination, outlineShaderMaterial, 1);
        }

        private void CreateMaterialsIfNeeded()
        {
            if (outlineShader == null)
            {
                outlineShader = Shader.Find("Hidden/OutlineEffect");
//                outlineShader = Resources.Load<Shader>("Azee/OutlineShader");
                if (outlineShader == null)
                {
                    throw new Exception("Critical Error: \"Hidden/OutlineEffect\" shader is missing. Make sure it is included in \"Always Included Shaders\" in ProjectSettings/Graphics.");
                }
            }

            if(outlineBufferShader == null)
            {
                outlineBufferShader = Shader.Find("Hidden/OutlineBufferEffect");
//                outlineBufferShader = Resources.Load<Shader>("Azee/OutlineBufferShader");
                if (outlineBufferShader == null)
                {
                    throw new Exception("Critical Error: \"Hidden/OutlineBufferEffect\" shader is missing. Make sure it is included in \"Always Included Shaders\" in ProjectSettings/Graphics.");
                }
            }
            if (outlineDepthMaskBufferShader == null)
            {
                outlineDepthMaskBufferShader = Shader.Find("Hidden/OutlineDepthBuffer");
//                outlineDepthMaskBufferShader = Resources.Load<Shader>("Azee/OutlineDepthMaskBufferShader");
                //                outlineDepthMaskBufferShader = Shader.Find("Hidden/Render Depth");
                if (outlineDepthMaskBufferShader == null)
                {
                    throw new Exception("Critical Error: \"Hidden/OutlineDepthBuffer\" shader is missing. Make sure it is included in \"Always Included Shaders\" in ProjectSettings/Graphics.");
                }
            }
            if (outlineShaderMaterial == null)
            {
                outlineShaderMaterial = new Material(outlineShader);
                outlineShaderMaterial.hideFlags = HideFlags.HideAndDontSave;
                UpdateMaterialsPublicProperties();
            }
            if(outlineEraseMaterial == null)
                outlineEraseMaterial = CreateMaterial(new Color(0, 0, 0, 0));
            if(outline1Material == null)
                outline1Material = CreateMaterial(new Color(1, 0, 0, 0));
            if(outline2Material == null)
                outline2Material = CreateMaterial(new Color(0, 1, 0, 0));
            if(outline3Material == null)
                outline3Material = CreateMaterial(new Color(0, 0, 1, 0));
            if (outlineDepthMaskMaterial == null)
            {
                outlineDepthMaskMaterial = new Material(outlineDepthMaskBufferShader);
            }
        }

        private void DestroyMaterials()
        {
            foreach(Material m in materialBuffer)
                DestroyImmediate(m);
            materialBuffer.Clear();
            DestroyImmediate(outlineShaderMaterial);
            DestroyImmediate(outlineEraseMaterial);
            DestroyImmediate(outline1Material);
            DestroyImmediate(outline2Material);
            DestroyImmediate(outline3Material);
            outlineShader = null;
            outlineBufferShader = null;
            outlineDepthMaskBufferShader = null;
            outlineShaderMaterial = null;
            outlineEraseMaterial = null;
            outline1Material = null;
            outline2Material = null;
            outline3Material = null;
        }

        private void SetLineThicknessOnMaterial(Material material)
        {
            float scalingFactor = 1;
            if (scaleWithScreenSize)
            {
                // If Screen.height gets bigger, outlines gets thicker
                scalingFactor = Screen.height / 360.0f;
            }

            // If scaling is too small (height less than 360 pixels), make sure you still render the outlines, but render them with 1 thickness
            if (scaleWithScreenSize && scalingFactor < 1)
            {
                if (UnityEngine.XR.XRSettings.isDeviceActive && sourceCamera.stereoTargetEye != StereoTargetEyeMask.None)
                {
                    material.SetFloat("_LineThicknessX", (1 / 1000.0f) * (1.0f / UnityEngine.XR.XRSettings.eyeTextureWidth) * 1000.0f);
                    material.SetFloat("_LineThicknessY", (1 / 1000.0f) * (1.0f / UnityEngine.XR.XRSettings.eyeTextureHeight) * 1000.0f);
                }
                else
                {
                    material.SetFloat("_LineThicknessX", (1 / 1000.0f) * (1.0f / Screen.width) * 1000.0f);
                    material.SetFloat("_LineThicknessY", (1 / 1000.0f) * (1.0f / Screen.height) * 1000.0f);
                }
            }
            else
            {
                if (UnityEngine.XR.XRSettings.isDeviceActive && sourceCamera.stereoTargetEye != StereoTargetEyeMask.None)
                {
                    material.SetFloat("_LineThicknessX", scalingFactor * (lineThickness / 1000.0f) * (1.0f / UnityEngine.XR.XRSettings.eyeTextureWidth) * 1000.0f);
                    material.SetFloat("_LineThicknessY", scalingFactor * (lineThickness / 1000.0f) * (1.0f / UnityEngine.XR.XRSettings.eyeTextureHeight) * 1000.0f);
                }
                else
                {
                    material.SetFloat("_LineThicknessX", scalingFactor * (lineThickness / 1000.0f) * (1.0f / Screen.width) * 1000.0f);
                    material.SetFloat("_LineThicknessY", scalingFactor * (lineThickness / 1000.0f) * (1.0f / Screen.height) * 1000.0f);
                }
            }
        }

        public void UpdateMaterialsPublicProperties()
        {
            if(outlineShaderMaterial)
            {
                SetLineThicknessOnMaterial(outlineShaderMaterial);
                
                outlineShaderMaterial.SetFloat("_LineIntensity", lineIntensity);
                outlineShaderMaterial.SetFloat("_FillAmount", fillAmount);
                outlineShaderMaterial.SetColor("_LineColor1", lineColor0 * lineColor0);
                outlineShaderMaterial.SetColor("_LineColor2", lineColor1 * lineColor1);
                outlineShaderMaterial.SetColor("_LineColor3", lineColor2 * lineColor2);
                if(flipY)
                    outlineShaderMaterial.SetInt("_FlipY", 1);
                else
                    outlineShaderMaterial.SetInt("_FlipY", 0);
                if(!additiveRendering)
                    outlineShaderMaterial.SetInt("_Dark", 1);
                else
                    outlineShaderMaterial.SetInt("_Dark", 0);
                if(cornerOutlines)
                    outlineShaderMaterial.SetInt("_CornerOutlines", 1);
                else
                    outlineShaderMaterial.SetInt("_CornerOutlines", 0);

                Shader.SetGlobalFloat("_OutlineAlphaCutoff", alphaCutoff);
            }
        }

        void UpdateOutlineCameraFromSource()
        {
            outlineCamera.CopyFrom(sourceCamera);
            outlineCamera.renderingPath = RenderingPath.Forward;
            outlineCamera.backgroundColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            outlineCamera.clearFlags = CameraClearFlags.SolidColor;
            outlineCamera.rect = new Rect(0, 0, 1, 1);
            outlineCamera.cullingMask = 0;
            outlineCamera.targetTexture = renderTexture;
            outlineCamera.enabled = false;
#if UNITY_5_6_OR_NEWER
            outlineCamera.allowHDR = false;
#else
            outlineCamera.hdr = false;
#endif
        }

        public void AddOutline(Outline outline)
        {
            if(!outlines.Contains(outline))
                outlines.Add(outline);
        }

        public void RemoveOutline(Outline outline)
        {
            if(outlines.Contains(outline))
                outlines.Remove(outline);
        }
    }
}
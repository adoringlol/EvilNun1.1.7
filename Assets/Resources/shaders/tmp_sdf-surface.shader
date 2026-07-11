Shader "TextMeshPro/Distance Field (Surface)" {
	Properties {
		_FaceTex ("Fill Texture", 2D) = "white" {}
		_FaceUVSpeedX ("Face UV Speed X", Range(-5, 5)) = 0
		_FaceUVSpeedY ("Face UV Speed Y", Range(-5, 5)) = 0
		_FaceColor ("Fill Color", Vector) = (1,1,1,1)
		_FaceDilate ("Face Dilate", Range(-1, 1)) = 0
		_OutlineColor ("Outline Color", Vector) = (0,0,0,1)
		_OutlineTex ("Outline Texture", 2D) = "white" {}
		_OutlineUVSpeedX ("Outline UV Speed X", Range(-5, 5)) = 0
		_OutlineUVSpeedY ("Outline UV Speed Y", Range(-5, 5)) = 0
		_OutlineWidth ("Outline Thickness", Range(0, 1)) = 0
		_OutlineSoftness ("Outline Softness", Range(0, 1)) = 0
		_Bevel ("Bevel", Range(0, 1)) = 0.5
		_BevelOffset ("Bevel Offset", Range(-0.5, 0.5)) = 0
		_BevelWidth ("Bevel Width", Range(-0.5, 0.5)) = 0
		_BevelClamp ("Bevel Clamp", Range(0, 1)) = 0
		_BevelRoundness ("Bevel Roundness", Range(0, 1)) = 0
		_BumpMap ("Normalmap", 2D) = "bump" {}
		_BumpOutline ("Bump Outline", Range(0, 1)) = 0.5
		_BumpFace ("Bump Face", Range(0, 1)) = 0.5
		_ReflectFaceColor ("Face Color", Vector) = (0,0,0,1)
		_ReflectOutlineColor ("Outline Color", Vector) = (0,0,0,1)
		_Cube ("Reflection Cubemap", Cube) = "black" {}
		_EnvMatrixRotation ("Texture Rotation", Vector) = (0,0,0,0)
		_SpecColor ("Specular Color", Vector) = (0,0,0,1)
		_FaceShininess ("Face Shininess", Range(0, 1)) = 0
		_OutlineShininess ("Outline Shininess", Range(0, 1)) = 0
		_GlowColor ("Color", Vector) = (0,1,0,0.5)
		_GlowOffset ("Offset", Range(-1, 1)) = 0
		_GlowInner ("Inner", Range(0, 1)) = 0.05
		_GlowOuter ("Outer", Range(0, 1)) = 0.05
		_GlowPower ("Falloff", Range(1, 0)) = 0.75
		_WeightNormal ("Weight Normal", Float) = 0
		_WeightBold ("Weight Bold", Float) = 0.5
		_ShaderFlags ("Flags", Float) = 0
		_ScaleRatioA ("Scale RatioA", Float) = 1
		_ScaleRatioB ("Scale RatioB", Float) = 1
		_ScaleRatioC ("Scale RatioC", Float) = 1
		_MainTex ("Font Atlas", 2D) = "white" {}
		_TextureWidth ("Texture Width", Float) = 512
		_TextureHeight ("Texture Height", Float) = 512
		_GradientScale ("Gradient Scale", Float) = 5
		_ScaleX ("Scale X", Float) = 1
		_ScaleY ("Scale Y", Float) = 1
		_PerspectiveFilter ("Perspective Correction", Range(0, 1)) = 0.875
		_VertexOffsetX ("Vertex OffsetX", Float) = 0
		_VertexOffsetY ("Vertex OffsetY", Float) = 0
	}
	// Reconstructed distance-field SURFACE body (the AssetRipper export shipped a
	// dummy pass-through that dumped the raw SDF atlas -> solid black box on world-
	// space text like boards/signs). Lit surface shader: atlas alpha holds the signed
	// distance (0.5 = glyph edge) -> AA coverage; face lit by the scene via Lambert so
	// 3D text integrates with baked/ambient lighting instead of glowing.
	SubShader {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		LOD 200
		Cull Off

		CGPROGRAM
		#pragma surface surf Lambert alpha:fade vertex:vert
		#pragma target 3.0

		// _MainTex_ST is auto-declared by the surface-shader compiler because Input
		// uses uv_MainTex; declaring it here too is a redefinition error.
		sampler2D _MainTex;
		fixed4 _FaceColor;
		float _FaceDilate;
		fixed4 _OutlineColor;
		float _OutlineWidth;

		struct Input {
			float2 uv_MainTex;
			fixed4 vertColor;
		};

		void vert(inout appdata_full v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.vertColor = v.color;
		}

		void surf(Input IN, inout SurfaceOutput o) {
			// Alpha8 SDF atlas: distance stored in .a, 0.5 == glyph edge.
			float dist = tex2D(_MainTex, IN.uv_MainTex).a;
			float aa = max(fwidth(dist), 0.0001);
			float faceThreshold = 0.5 - _FaceDilate * 0.5;
			float faceCoverage = smoothstep(faceThreshold - aa, faceThreshold + aa, dist);

			fixed4 face = IN.vertColor * _FaceColor;
			fixed3 albedo = face.rgb;
			float alpha = face.a * faceCoverage;
			if (_OutlineWidth > 0.0001 && _OutlineColor.a > 0.0001) {
				float outlineThreshold = faceThreshold - _OutlineWidth * 0.5;
				float outlineCoverage = smoothstep(outlineThreshold - aa, outlineThreshold + aa, dist);
				albedo = lerp(_OutlineColor.rgb, face.rgb, faceCoverage);
				alpha = max(face.a * faceCoverage, _OutlineColor.a * outlineCoverage);
			}
			o.Albedo = albedo;
			o.Alpha = alpha;
		}
		ENDCG
	}
	Fallback "TextMeshPro/Distance Field"
	//CustomEditor "TMPro.EditorUtilities.TMP_SDFShaderGUI"
}
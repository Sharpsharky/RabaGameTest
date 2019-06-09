// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/Sprite HBSC"
{
    Properties
    {
		[PerRendererData] _MainTex ("Base (RGB), Alpha (A)", 2D) = "black" {}
		_Color ("Color", Color) = (1,1,1,1)
        _Hue ("Hue", Range(0, 6.284)) = 0
        _Brightness ("Brightness", Range(-1,1)) = 0
        _Saturation ("Saturation", Range(0,2)) = 1
        _Contrast ("Contrast", Range(0,2)) = 1
    }
 
    SubShader
    {
        LOD 100
 
        Tags
        {
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
        }
     
        //Cull Off
        //Lighting Off
        ZWrite Off
        //Fog { Mode Off }
        //Offset -1, -1
        Blend SrcAlpha OneMinusSrcAlpha
 
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
             
            #include "UnityCG.cginc"
 
            struct appdata_t
            {
                fixed4 vertex : POSITION;
                fixed2 texcoord : TEXCOORD0;
                fixed4 color : COLOR;
            };
 
            struct v2f
            {
                fixed4 vertex : SV_POSITION;
                fixed2 texcoord : TEXCOORD0;
                fixed4 color : COLOR;
            };
 
            sampler2D _MainTex;
            fixed _Hue;
            fixed _Brightness;
	        fixed _Contrast;
	        fixed _Saturation;
			fixed4 _Color;
            
            inline fixed3 applyHue(fixed3 aColor, fixed aHue)
			{
				fixed3 k = fixed3(0.57735, 0.57735, 0.57735);
			    fixed angle = aHue;
			    fixed cosAngle = 0;
			    fixed sinAngle = 0;
			    sincos (angle, sinAngle, cosAngle);
			    return aColor * cosAngle + cross(k, aColor) * sinAngle + k * dot(k, aColor) * (1 - cosAngle);
			}
			 
			inline fixed4 applyHSBEffect(fixed4 startColor)
			{
			    fixed4 outputColor = startColor;
			    outputColor.rgb = applyHue(outputColor.rgb, _Hue);
			    outputColor.rgb = (outputColor.rgb - 0.5f) * (_Contrast) + 0.5f;
			    outputColor.rgb = outputColor.rgb + _Brightness;      
			    fixed3 intensity = dot(outputColor.rgb, fixed3(0.299,0.587,0.114));
			    outputColor.rgb = lerp(intensity, outputColor.rgb, _Saturation);
			 
			    return outputColor;
			}
         
            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                o.color = v.color;
                return o;
            }
             
            fixed4 frag (v2f i) : COLOR
            {
                fixed4 startColor = tex2D(_MainTex, i.texcoord);
                fixed4 hsbColor = applyHSBEffect(startColor) * _Color;
                return hsbColor;
            }
            ENDCG
        }
    }
	FallBack "Diffuse"
    
}
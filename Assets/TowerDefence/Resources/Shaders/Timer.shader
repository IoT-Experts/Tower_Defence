// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Timer"
{
   Properties
   {
      _MainTex ("Texture Image", 2D) = "white" {} 
          _SecondTex ("Second Image", 2D) = "white" {} 
          _MaskTime ("Time", Range (0, 1)) = 0
          _MPow ("Pow", Range (5, 50)) = 5
   }
   SubShader
   {
      Pass
          {    
         CGPROGRAM
 
         #pragma vertex vert  
         #pragma fragment frag 
 
         sampler2D _MainTex;
                 sampler2D _SecondTex;
                 fixed _MaskTime;
                 fixed _MPow;
 
         struct vertexInput
                 {
            float4 vertex : POSITION;
            fixed4 texcoord : TEXCOORD0;
         };
         struct vertexOutput
                 {
            fixed4 pos : SV_POSITION;
            fixed2 tex : TEXCOORD0;
         };
 
         vertexOutput vert (vertexInput input) 
         {
            vertexOutput output;
 
            output.tex = input.texcoord;
            output.pos = UnityObjectToClipPos (input.vertex);
            return output;
         }

         float4 frag (vertexOutput input) : COLOR
         {
                        fixed3 c0 = tex2D (_MainTex, fixed2 (input.tex));
                        fixed3 c1 = tex2D (_SecondTex, fixed2 (input.tex));

                        fixed dot1 = dot (normalize (input.tex.xy-fixed2 (0.5h, 0.5h)), fixed2 (0h, 1h));

                        half ang = acos (dot1);

                        ang = degrees (ang);
                        ang = (input.tex.x<0.5h)?360h-ang:ang;


                        fixed pos = min ((ang/360h), 360h);
                        pos = pos+0.9h-_MaskTime+(0.2h*(1h-_MaskTime));
                        pos = saturate (pow (pos, _MPow*_MPow));


                        fixed3 c = lerp (c1.rgb, c0, pos);

                        return fixed4 (c, 1h);
                        //return pos;
         }
 
         ENDCG
      }
   }
   }
 
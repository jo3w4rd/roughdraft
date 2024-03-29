uniform sampler2D texture;
uniform sampler2D   vDistortion;
uniform sampler2D   hDistortion;

varying vec2 distortionLookup;
varying vec4 vertColor;
varying vec4 vertTexCoord;

const vec4 decoderCoefficients = vec4(1.0, 1.0/255.0, 1.0/(255.0*255.0), 1.0/(255.0*255.0*255.0));

void main() {
  vec4 vEncoded = texture2D(vDistortion, vertTexCoord.st);
  vec4 hEncoded = texture2D(hDistortion, vertTexCoord.st);
  float vIndex = dot(vEncoded, decoderCoefficients) * 2.3 - 0.6;
  float hIndex = dot(hEncoded, decoderCoefficients) * 2.3 - 0.6;
  
  if(vIndex > 0.0 && vIndex < 1.0
        && hIndex > 0.0 && hIndex < 1.0)
  {  
      gl_FragColor = texture2D(texture, vec2(hIndex, vIndex)) * vertColor;
  } else {
      gl_FragColor = vec4(1.0, 0, 0, 1.0);
  }
}

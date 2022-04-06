#define PROCESSING_TEXTURE_SHADER

//Values from Processing:
uniform sampler2D texture; //The texture to use
uniform vec2 texOffset; //Size of a "pixel"

//Values from Vertex Shader:
varying vec4 vertTexCoord; //UV value at this pixel
varying vec4 vertColor; //Vertex color at this pixel

uniform float time; //Time in seconds

//Runs once per pixel
void main(){
	float ratio = texOffset.x / texOffset.y;
	
	vec2 uv = vertTexCoord.xy - vec2(.5, .5);
	
	float mag = length(uv); //Dis from center
	float rad = atan(uv.y, uv.x); //Angle from center
	
	//mag -= .01 * sin(time);
	rad += .05 * sin(time);

	uv.x = mag * cos(rad);
	uv.y = mag * sin(rad);

	uv += vec2(.5, .5); //Move origin back to (0, 0)	

	//Lookup pixel color at UV coordinate:
	vec4 color = texture2D(texture, uv);

	//Set the pixel color of gl_FragColor
	gl_FragColor = color;
}
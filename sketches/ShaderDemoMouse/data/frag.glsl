#define PROCESSING_TEXTURE_SHADER

//Values from Processing:
uniform sampler2D texture; //The texture to use
uniform vec2 texOffset; //Size of a "pixel"

//Values from Vertex Shader:
varying vec4 vertTexCoord; //UV value at this pixel
varying vec4 vertColor; //Vertex color at this pixel

uniform vec2 mouse;

//Runs once per pixel
void main(){
	float ratio = texOffset.x / texOffset.y;
	
	vec2 uv = vertTexCoord.xy - mouse;	
	float dis = length(uv);

	//Lookup pixel color at UV coordinate:
	vec4 color = vec4(dis, dis, dis, 1);

	//Set the pixel color of gl_FragColor
	gl_FragColor = color;
}
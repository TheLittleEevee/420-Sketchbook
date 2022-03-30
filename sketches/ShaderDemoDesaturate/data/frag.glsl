#define PROCESSING_TEXTURE_SHADER

//Values from Processing:
uniform sampler2D texture; //The texture to use
uniform vec2 texOffset; //Size of a "pixel"

//Values from Vertex Shader:
varying vec4 vertTexCoord; //UV value at this pixel
varying vec4 vertColor; //Vertex color at this pixel

//Runs once per pixel
void main(){
	//Lookup pixel color at UV coordinate:
	vec4 color = texture2D(texture, vertTexCoord.xy);
	
	//To Do: Desaturate
	float v = (color.r + color.g + color.b)/3;
	color.rgb = vec3(v);

	//Set the pixel color of gl_FragColor
	gl_FragColor = color;
}
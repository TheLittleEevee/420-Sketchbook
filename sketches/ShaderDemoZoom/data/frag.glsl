#define PROCESSING_TEXTURE_SHADER

//Values from Processing:
uniform sampler2D texture; //The texture to use
uniform vec2 texOffset; //Size of a "pixel"

//Values from Vertex Shader:
varying vec4 vertTexCoord; //UV value at this pixel
varying vec4 vertColor; //Vertex color at this pixel

//Runs once per pixel
void main(){
	float ratio = texOffset.x / texOffset.y;
	
	vec2 uv = vertTexCoord.xy;
	
	float mag = length(uv); //Dis from (0, 0)
	float rad = atan(uv.y, uv.x); //Angle from (0, 0)

	mag += .01;

	uv.x = mag * cos(rad);
	uv.y = mag * sin(rad);
	
	//Lookup pixel color at UV coordinate:
	vec4 color = texture2D(texture, uv);

	//Set the pixel color of gl_FragColor
	gl_FragColor = color;
}
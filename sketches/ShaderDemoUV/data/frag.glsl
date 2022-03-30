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
	
	//Lookup pixel color at UV coordinate:
	vec4 color = texture2D(texture, vertTexCoord.xy);
	
	//Make red border
	vec2 uv = vertTexCoord.xy;
	if (uv.x < .1 || uv.x > .9 || uv.y < .1 || uv.y > .9){
		color = vec4(1, 0, 0, 1);
	}

	//Make blue circle
	vec2 dis = vec2(.5) - uv;
	dis.x /= ratio;
	float d = length(dis); //Units = percentage
	
	if (d < .1){
		color = vec4(0, 0, 1, 1);
	}

	//Set the pixel color of gl_FragColor
	gl_FragColor = color;
}
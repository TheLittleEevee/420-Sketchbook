#define PROCESSING_TEXTURE_SHADER

//values from Processing:
uniform mat4 transform;
uniform mat4 texMatrix;

attribute vec4 vertex; //Position in local-space
attribute vec4 color; //Vertex color
attribute vec2 texCoord; //UV

varying vec4 vertColor;
varying vec4 vertTexCoord;

//Runs once per vertex
void main(){
	//gl_Position to the final vertex screen-space position
	gl_Position = transform * vertex; //Can't flip because that would be a vector times a matrix, which isn't allowed

	vertColor = color;
	vertTexCoord = texMatrix * vec4(texCoord, 1, 1); //Vector must be as long as the matrix is wide
}
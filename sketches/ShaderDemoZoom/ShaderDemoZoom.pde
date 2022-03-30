PImage img;
PShader shader;

void setup(){
  size(800, 600, P2D);
  
  img = loadImage("Repede.png");
  imageMode(CENTER);
  
  shader = loadShader("frag.glsl", "vert.glsl");
}

void draw(){
  //background(100);
  image(img, mouseX, mouseY);
  filter(shader);
}
